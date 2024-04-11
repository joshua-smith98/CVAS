using CVAS.WinAudioEngineNS;
using CVAS.Core;
using CVAS.TerminalNS;
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

            // Construct neccesary variables
            LibraryCacheFile libraryCacheFile;
            List<string> audioFileNames  = new();

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
                Terminal.MessageSingle("A cache does not exist for this folder - a first time analysis will be executed.", ConsoleColor.Yellow);
            }
            catch (InvalidFileFormatException)
            {
                Terminal.MessageSingle("The cache file for this folder is of an invalid format, and will be rebuilt.");
            }
            catch (InvalidFileVersionException)
            {
                Terminal.MessageSingle("The cache file for this folder is of an old format, and will be rebuilt.");
            }
            catch (InvalidFileHashException)
            {
                Terminal.MessageSingle("The folder contents have changed - the cache file will be rebuilt.", ConsoleColor.Yellow);
            }

            // Otherwise, load filenames from Directory.GetFiles(), checking for validity as audio files
            Terminal.BeginReport("Searching for audio files...");
            var files = Directory.GetFiles(path);

            for (int i = 0; i < files.Length; i++)
            {
                // Make report
                if (i % 100 == 0)
                {
                    float percent = (float)(i + 1) / files.Length;
                    percent *= 100f;
                    Terminal.Report($"[{percent:0}%]");
                }

                // Only add the file if it is an audio file, otherwise ignore it
                if (AudioEngine.IsAudioFile(files[i])) audioFileNames.Add(SysPath.GetFileName(files[i]));
            }
            Terminal.EndReport($"Successfully found {audioFileNames.Count} audio files.");

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

            Terminal.BeginReport("Performing phrase analysis...");
            
            // Straight copy-pasted from Library with a few changes. Once this refactor is done, it won't be there anymore!

            // Construct list of phrases to build library from
            List<Phrase> phrases = new();

            // Assume we could have either a middle, end or both inflections.
            // First, iterate through all middle inflection files (no suffix), create phrases and attach any end inflections if they exist
            // If we attach an end reflection, remove it from the list of end inflection files
            // Finally, add all remaining end inflection files to their own phrase and construct library.

            // Get files
            string[] filePaths = AudioFileNames.Select(x => SysPath.Combine(Path!, x)).ToArray(); // Convert filenames to full path
            List<string> files_ends = filePaths.Where(x => SysPath.GetFileNameWithoutExtension(x).EndsWith(".f")).ToList(); // List of files with end inflection
            string[] files_middles = filePaths.Where(x => !files_ends.Contains(x)).ToArray(); // List of files with middle inflection (all that aren't an end)

            // Iterate through all middle inflection files
            for (int i = 0; i < files_middles.Length; i++)
            {
                if (i % 100 == 0)
                {
                    float percent = (float)(i + 1) / files_middles.Length;
                    percent *= 100f;
                    Terminal.Report($"[{percent:0}%] (1/2) Analysing middle inflections...");
                }

                // Don't need to do a validity check, because we already did that while loading AudioFileNames
                AudioClip audioClip_middle = new AudioFileStreaming(files_middles[i]);

                // Phrase.str is file name without extension
                string str = SysPath.GetFileNameWithoutExtension(files_middles[i]);

                // Check for ending inflection: construct new file path using directory, filename without extension, ".f" and extension
                string file_end = SysPath.Combine(
                    Path!, // directory name - NOTE: Path will never be null
                    SysPath.GetFileNameWithoutExtension(files_middles[i]) + ".f" + // Filename + ending suffice
                    SysPath.GetExtension(files_middles[i])
                    );

                // Load ending inflection if it exists, otherwise load null
                AudioClip? audioClip_end = files_ends.Contains(file_end) ? new AudioFileStreaming(file_end) : null;

                // Construct new phrases and add to list
                if (audioClip_end is not null)
                {
                    phrases.Add(new Phrase(str, new Inflection(InflectionType.End, audioClip_end), new Inflection(InflectionType.Middle, audioClip_middle)));
                    files_ends.Remove(file_end); // If we add an end inflection to a phrase, remove it from the list of end inflection files
                }
                else phrases.Add(new Phrase(str, new Inflection(InflectionType.Middle, audioClip_middle)));
            }

            // Add all remaining end inflection files to their own phrase
            for (int i = 0; i < files_ends.Count; i++)
            {
                if (i % 100 == 0)
                {
                    float percent = (float)(i + 1) / files_ends.Count;
                    percent *= 100f;
                    Terminal.Report($"[{percent:0}%] (2/2) Analysing end inflections...");
                }

                // Audio file validity check
                AudioClip audioClip_end;

                try
                {
                    audioClip_end = new AudioFileStreaming(files_ends[i]);
                }
                catch { continue; }

                string str = SysPath.GetFileNameWithoutExtension(files_ends[i]);
                str = str[..^2];

                phrases.Add(new Phrase(str, new Inflection(InflectionType.End, audioClip_end)));
            }

            // Construct library
            Library ret = new(phrases.ToArray());
            Terminal.EndReport($"Successfully analysed {filePaths.Length} files and loaded {ret.Phrases.Length} phrases.");

            // Build cache
            LibraryCacheFile.Deconstruct(ret).SaveTo(SysPath.Combine(Path!, LibraryCacheFile.DefaultPath)); // Path will never be null
            Terminal.BeginMessage();
            Terminal.Message("Cache built. The next load will be much quicker!");
            Terminal.EndMessage();

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
