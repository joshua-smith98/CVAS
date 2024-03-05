﻿using CVAS.AudioEngine;
using CVAS.DataStructure;
using System.Security.Cryptography;
using System.Text;

namespace CVAS.FileFormats
{
    /// <summary>
    /// Represents the data contained within a Library Cache File. See CVAS Trello page for rough specification.
    /// </summary>
    public class LibraryCacheFile : IFile<Library> // This file will be moved back into CVAS.FileFormats (soon to be CVAS.IO) as part of an upcoming refactor.
    {
        public string? Path { get; private set; }

        public char[] Header => "CVASLBCH".ToArray();

        private int NumPhrases;
        private PhraseTableRow[] PhraseTable;

        private struct PhraseTableRow
        {
            public string Str;
            public int NumInflections;
            public InflectionTableRow[] InflectionTable;
        }

        private struct InflectionTableRow
        {
            public int Inflection;
            public string AudioFilePath;
        }

        private LibraryCacheFile(string path, int numPhrases, PhraseTableRow[] phraseTable)
        {
            Path = path;
            NumPhrases = numPhrases;
            PhraseTable = phraseTable;
        }

        private LibraryCacheFile(int numPhrases, PhraseTableRow[] phraseTable)
        {
            NumPhrases = numPhrases;
            PhraseTable = phraseTable;
        }

        /// <summary>
        /// Attempts to load an instance of <see cref="LibraryCacheFile"/> from the disk.
        /// </summary>
        /// <param name="path">The path of the file to load from.</param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="InvalidFileHeaderException"></exception>
        /// <exception cref="InvalidFileHashException"></exception>
        /// <exception cref="InvalidFileFormatException"></exception>
        public static IFile<Library> LoadFrom(string path)
        {
            // Validity check: file exists
            if (!File.Exists(path)) throw new FileNotFoundException();

            LibraryCacheFile ret;

            // Open File
            using (BinaryReader br = new(File.OpenRead(path)))
            {
                #region Validity Checks
                {
                    // Validity check: header
                    if (!br.ReadChars(8).SequenceEqual("CVASLBCH".ToArray())) throw new InvalidFileHeaderException();

                    // Validity check: folder hash
                    var filenames = Directory.GetFiles(System.IO.Path.GetDirectoryName(path)).Select(x => System.IO.Path.GetFileName(x)); // TODO: handle possible null reference
                    var filenames_string = "";
                    foreach (var filename in filenames)
                        filenames_string += filename;
                    byte[] folderHash;
                    using (var md5 = MD5.Create())
                        folderHash = md5.ComputeHash(Encoding.ASCII.GetBytes(filenames_string));

                    if (!br.ReadBytes(16).SequenceEqual(folderHash)) throw new InvalidFileHashException();

                    // Validity check: table integrity (iterate through tables and check EOF)
                    try
                    {
                        int numPhrases = br.ReadInt32();
                        for (int i = 0; i < numPhrases; i++)
                        {
                            var str = br.ReadString(); // Phrase str
                            int numInflections = br.ReadInt32();
                            for (int j = 0; j < numInflections; j++)
                            {
                                br.ReadInt32(); // Inflection
                                br.ReadString(); // Audio file path
                            }
                        }
                    }
                    catch (EndOfStreamException) // Case: the table is shorter than expected
                    {
                        throw new InvalidFileFormatException();
                    }

                    if (br.BaseStream.Position != br.BaseStream.Length) throw new InvalidFileFormatException(); // Case: there is extra data in the file
                }
                #endregion

                // Reset position after checking file validity
                br.BaseStream.Position = 0;
                Console.WriteLine($"Loading from cache...");

                #region Load from file and construct
                {
                    // Skip 2 x 4 byte headers and 16 byte folder hash
                    br.BaseStream.Position = 24;

                    // Read phrases
                    int numPhrases = br.ReadInt32();
                    PhraseTableRow[] phraseTable = new PhraseTableRow[numPhrases];

                    for (int i = 0; i < numPhrases; i++)
                    {
                        phraseTable[i] = new PhraseTableRow();
                        phraseTable[i].Str = br.ReadString();
                        phraseTable[i].NumInflections = br.ReadInt32();
                        phraseTable[i].InflectionTable = new InflectionTableRow[phraseTable[i].NumInflections];

                        // Read inflections
                        for (int j = 0; j < phraseTable[i].NumInflections; j++)
                        {
                            phraseTable[i].InflectionTable[j] = new InflectionTableRow();
                            phraseTable[i].InflectionTable[j].Inflection = br.ReadInt32();
                            phraseTable[i].InflectionTable[j].AudioFilePath = br.ReadString();
                        }
                    }

                    // Construct instance
                    ret = new LibraryCacheFile(path, numPhrases, phraseTable);
                }
                #endregion
            }

            return ret;
        }

        /// <summary>
        /// Deconstructs the given <see cref="Library"/> into an instance of <see cref="LibraryCacheFile"/>.
        /// </summary>
        /// <param name="library"></param>
        /// <returns></returns>
        public static IFile<Library> Deconstruct(Library library)
        {
            // Initialise variables for LibraryCacheFile construction
            // NOTE: We will need to use lists here for the phraseTable and inflectionTable, due to our need to exclude non-IAudioFile phrases and inflections
            List<PhraseTableRow> phraseTable = new List<PhraseTableRow>();

            // Deconstruct phrases
            foreach (Phrase phrase in library.Phrases)
            {
                // Check if this phrase contains only non-IAudioFiles and if so don't include
                if (phrase.Inflections.Select(x => x.AudioClip).Where(x => x is IAudioFile).Count() == 0) continue;
                
                PhraseTableRow phraseRow = new PhraseTableRow();
                phraseRow.Str = phrase.Str;

                // Deconstruct inflections
                List<InflectionTableRow> inflectionTable = new List<InflectionTableRow>();
                foreach (InflectionType inflectionType in phrase.Inflections.Select(x => x.InflectionType))
                {
                    // Check if this inflection isn't IAudioFile, if so and don't include
                    if (phrase.GetAudioClip(inflectionType) is not IAudioFile) continue;
                    
                    var inflectionRow = new InflectionTableRow();
                    inflectionRow.Inflection = (int)inflectionType;
                    inflectionRow.AudioFilePath = ((IAudioFile)phrase.GetAudioClip(inflectionType)).Path;

                    inflectionTable.Add(inflectionRow);
                }

                phraseRow.NumInflections = inflectionTable.Count();
                phraseRow.InflectionTable = inflectionTable.ToArray();
                phraseTable.Add(phraseRow);
            }

            // Construct and return
            return new LibraryCacheFile(phraseTable.Count(), phraseTable.ToArray());
        }

        /// <summary>
        /// Constructs a new instance of <see cref="Library"/> from this instance of <see cref="LibraryCacheFile"/>.
        /// </summary>
        /// <returns></returns>
        public Library Construct()
        {
            // Construct phrases
            List<Phrase> phrases = new List<Phrase>();
            foreach (PhraseTableRow phraseRow in PhraseTable)
            {
                // Construct inflections
                InflectionCollection inflections = new InflectionCollection();
                foreach (InflectionTableRow inflectionRow in phraseRow.InflectionTable)
                {
                    InflectionType inflectionType = (InflectionType)inflectionRow.Inflection;
                    IAudioClip audioClip = new AudioFileStreaming(inflectionRow.AudioFilePath);
                    inflections.Add(new Inflection(inflectionType, audioClip));
                }

                phrases.Add(new Phrase(phraseRow.Str, inflections.ToArray()));
            }

            // Construct library and return
            Console.WriteLine($"Loaded {phrases.Count()} phrases and {phrases.Select(x => x.Inflections.Length).Sum()} audio files.");
            Console.WriteLine();
            return new Library(phrases.ToArray());
        }

        /// <summary>
        /// Saves this instance of <see cref="LibraryCacheFile"/> to the disk, overwriting the existing file or creating new one if it doesn't exist.
        /// </summary>
        /// <param name="path">The path of the file to save to.</param>
        public void SaveTo(string path)
        {
            // Create file
            using (BinaryWriter bw = new(File.Create(path)))
            {
                // Write headers
                bw.Write("CVASLBCH".ToArray()); // Convert to char[] so that BinaryWriter doesn't write a length int before the header

                // Compute and write folder hash
                var filenames = Directory.GetFiles(System.IO.Path.GetDirectoryName(path)).Select(x => System.IO.Path.GetFileName(x)); // TODO: handle possible null reference
                var filenames_string = "";
                foreach (var filename in filenames)
                    filenames_string += filename;
                byte[] folderHash;
                using (var md5 = MD5.Create())
                    folderHash = md5.ComputeHash(Encoding.ASCII.GetBytes(filenames_string));
                bw.Write(folderHash);

                // Write phrases & inflections
                bw.Write(NumPhrases);
                foreach (PhraseTableRow phrase in PhraseTable)
                {
                    bw.Write(phrase.Str);
                    bw.Write(phrase.NumInflections);
                    foreach (InflectionTableRow inflection in phrase.InflectionTable)
                    {
                        bw.Write(inflection.Inflection);
                        bw.Write(inflection.AudioFilePath);
                    }
                }

                // Ensure file is ready to close
                bw.Flush();
            }

            // Change path of this instance
            Path = path;
        }
    }
}
