﻿using CVAS.AudioEngine;
using CVAS.Core;
using SysPath = System.IO.Path;

namespace CVAS.FileSystem
{
    /// <summary>
    /// Represents a folder of audio files, which can be loaded into a <see cref="Library"/>
    /// </summary>
    public class LibraryFolder : IFolder<Library>
    {
        public string? Path { get; private set; }

        internal string[] AudioFileNames { get; }

        internal LibraryCacheFile? LibraryCacheFile { get; private set; }

        private LibraryFolder(string path, string[] audioFileNames)
        {
            Path = path;
            AudioFileNames = audioFileNames;
        }

        /// <summary>
        /// Attempts to load an instance of <see cref="LibraryFolder"/> from the folder at the given path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public static IFolder<Library> LoadFrom(string path)
        {
            // Validity checks: path is a valid directory
            if (!Directory.Exists(path)) throw new DirectoryNotFoundException();

            Console.WriteLine();

            // Construct neccesary variables
            LibraryCacheFile libraryCacheFile;
            List<string> audioFileNames  = new List<string>();

            // Try to load LibraryCacheFile and extract AudioFileNames from that
            try
            {
                libraryCacheFile = (LibraryCacheFile)LibraryCacheFile.LoadFrom(SysPath.Combine(path, LibraryCacheFile.DefaultPath));

                foreach (LibraryCacheFile.PhraseTableRow phraseRow in libraryCacheFile.PhraseTable)
                {
                    foreach (LibraryCacheFile.InflectionTableRow inflectionRow in phraseRow.InflectionTable)
                    {
                        audioFileNames.Add(inflectionRow.AudioFileName);
                    }
                }

                return new LibraryFolder(path, audioFileNames.ToArray()) { LibraryCacheFile = libraryCacheFile };
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("A cache does not exist for this folder.");
            }
            catch (InvalidFileHashException)
            {
                Console.WriteLine("The folder contents have changed.");
            }

            // Otherwise, load filenames from Directory.GetFiles(), checking for validity as audio files
            foreach (string fileName in Directory.GetFiles(path))
            {
                try
                {
                    using var a = new AudioFileStreaming(fileName) ; // Test to see if it is a valid audio file

                    audioFileNames.Add(SysPath.GetFileName(fileName));
                }
                catch { } // Ignore all non-audiofiles
            }

            return new LibraryFolder(path, audioFileNames.ToArray());
        }

        /// <summary>
        /// Constructs a <see cref="Library"/> using the current instance of <see cref="LibraryFolder"/>.
        /// </summary>
        /// <returns></returns>
        public Library Construct()
        {
            // Try to construct from LibraryCacheFile first
            if (LibraryCacheFile is not null)
            {
                return LibraryCacheFile.Construct();
            }

            Console.WriteLine();
            Console.WriteLine("Performing phrase analysis...");
            
            // Straight copy-pasted from Library with a few changes. Once this refactor is done, it won't be there anymore!

            // Construct list of phrases to build library from
            List<Phrase> phrases = new List<Phrase>();

            // Assume we could have either a middle, end or both inflections.
            // First, iterate through all middle inflection files (no suffix), create phrases and attach any end inflections if they exist
            // If we attach an end reflection, remove it from the list of end inflection files
            // Finally, add all remaining end inflection files to their own phrase and construct library.

            // Get files
            string[] filePaths = AudioFileNames.Select(x => SysPath.Combine(Path, x)).ToArray(); // Convert filenames to full path
            List<string> files_ends = filePaths.Where(x => SysPath.GetFileNameWithoutExtension(x).EndsWith(".f")).ToList(); // List of files with end inflection
            string[] files_middles = filePaths.Where(x => !files_ends.Contains(x)).ToArray(); // List of files with middle inflection (all that aren't an end)

            // Iterate through all middle inflection files
            foreach (string file_middle in files_middles)
            {
                // Don't need to do a validity check, because we already did that while loading AudioFileNames
                IAudioClip audioClip_middle = new AudioFileStreaming(file_middle);

                // Phrase.str is file name without extension
                string str = SysPath.GetFileNameWithoutExtension(file_middle);

                // Check for ending inflection: construct new file path using directory, filename without extension, ".f" and extension
                string file_end = SysPath.Combine(
                    Path, // directory name - NOTE: Path will never be null
                    SysPath.GetFileNameWithoutExtension(file_middle) + ".f" + // Filename + ending suffice
                    SysPath.GetExtension(file_middle)
                    );

                // Load ending inflection if it exists, otherwise load null
                IAudioClip? audioClip_end = files_ends.Contains(file_end) ? new AudioFileStreaming(file_end) : null;

                // Construct new phrases and add to list
                if (audioClip_end is not null)
                {
                    phrases.Add(new Phrase(str, new Inflection(InflectionType.End, audioClip_end), new Inflection(InflectionType.Middle, audioClip_middle)));
                    files_ends.Remove(file_end); // If we add an end inflection to a phrase, remove it from the list of end inflection files
                }
                else phrases.Add(new Phrase(str, new Inflection(InflectionType.Middle, audioClip_middle)));
            }

            // Add all remaining end inflection files to their own phrase
            foreach (string file_end in files_ends)
            {
                // Audio file validity check
                IAudioClip audioClip_end;

                try
                {
                    audioClip_end = new AudioFileStreaming(file_end);
                }
                catch { continue; }

                string str = SysPath.GetFileNameWithoutExtension(file_end);
                str = str.Substring(0, str.Length - 2);

                phrases.Add(new Phrase(str, new Inflection(InflectionType.End, audioClip_end)));
            }

            // Construct library
            Library ret = new Library(phrases.ToArray());
            Console.WriteLine($"Successfully analysed {filePaths.Length} files and loaded {ret.Phrases.Length} phrases.");

            // Build cache
            LibraryCacheFile.Deconstruct(ret).SaveTo(SysPath.Combine(Path, LibraryCacheFile.DefaultPath)); // Path will never be null
            Console.WriteLine("Cache built. The next load will be much quicker!");
            Console.WriteLine();

            return ret;
        }

        /// <summary>
        /// Note: this action is not allowed, and will throw a <see cref="TransmutationNotAllowedException"/>.
        /// </summary>
        /// <param name="Object"></param>
        /// <returns></returns>
        /// <exception cref="TransmutationNotAllowedException"></exception>
        public static IFolder<Library> Deconstruct(Library Object)
        {
            throw new TransmutationNotAllowedException();
        }
        /// <summary>
        /// Note: this action is not allowed, and will throw a <see cref="TransmutationNotAllowedException"/>.
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="TransmutationNotAllowedException"></exception>
        public void SaveTo(string path)
        {
            throw new TransmutationNotAllowedException();
        }
    }
}