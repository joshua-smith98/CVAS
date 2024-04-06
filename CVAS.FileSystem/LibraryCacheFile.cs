using CVAS.AudioEngineNS;
using CVAS.Core;
using CVAS.TerminalNS;
using System.Security.Cryptography;
using System.Text;
using SysPath = System.IO.Path;

namespace CVAS.FileSystem
{
    /// <summary>
    /// Represents the data contained within a Library Cache File. See CVAS Trello page for rough specification.
    /// </summary>
    public class LibraryCacheFile : IFile<Library> // This file will be moved back into CVAS.FileFormats (soon to be CVAS.IO) as part of an upcoming refactor.
    {
        public string? Path { get; private set; }
        /// <summary>
        /// The default path for every <see cref="LibraryCacheFile"/> in a <see cref="LibraryFolder"/>.
        /// </summary>
        public const string DefaultPath = "cache.lbc";

        public char[] Header => "CVASLBCH".ToArray();
        public static int Version = 1;

        internal int NumPhrases { get; private set; }
        internal PhraseTableRow[] PhraseTable { get; private set; }

        internal struct PhraseTableRow
        {
            public string Str;
            public int NumInflections;
            public InflectionTableRow[] InflectionTable;
        }

        internal struct InflectionTableRow
        {
            public int Inflection;
            public string AudioFileName;
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

            // Terminal feedback
            // We'll just use a message here, since the process is far too quick to report progress.
            Terminal.MessageSingle("Cache file found.", ConsoleColor.Yellow);

            // Open File
            using (BinaryReader br = new(File.OpenRead(path)))
            {
                #region Validity Checks
                {
                    // Validity check: header
                    if (!br.ReadChars(8).SequenceEqual("CVASLBCH".ToArray())) throw new InvalidFileFormatException();

                    // Validity check: version number (current version is '1')
                    if (br.ReadInt32() != Version) throw new InvalidFileVersionException();

                    // Validity check: folder hash
                    // Path.GetDirectoryName() will never return null, as the cache file at 'path' will always been in a library folder
                    var filenames = Directory.GetFiles(SysPath.GetDirectoryName(path)!).Select(x => SysPath.GetFileName(x));
                    var filenames_string = "";
                    foreach (var filename in filenames)
                        filenames_string += filename;
                    byte[] folderHash = MD5.HashData(Encoding.ASCII.GetBytes(filenames_string));

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

                    // Case: there is extra data in the file
                    if (br.BaseStream.Position != br.BaseStream.Length) throw new InvalidFileFormatException(); 
                }
                #endregion

                // Reset position after checking file validity
                br.BaseStream.Position = 0;

                #region Load from file and construct
                {
                    // Skip 2 x 4 byte headers, 4 byte version no. and 16 byte folder hash
                    br.BaseStream.Position = 28;

                    // Read phrases
                    int numPhrases = br.ReadInt32();
                    PhraseTableRow[] phraseTable = new PhraseTableRow[numPhrases];

                    for (int i = 0; i < numPhrases; i++)
                    {
                        phraseTable[i] = new PhraseTableRow
                        {
                            Str = br.ReadString(),
                            NumInflections = br.ReadInt32(),
                        };
                        phraseTable[i].InflectionTable = new InflectionTableRow[phraseTable[i].NumInflections];

                        // Read inflections
                        for (int j = 0; j < phraseTable[i].NumInflections; j++)
                        {
                            phraseTable[i].InflectionTable[j] = new InflectionTableRow
                            {
                                Inflection = br.ReadInt32(),
                                AudioFileName = br.ReadString()
                            };
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
            List<PhraseTableRow> phraseTable = [];

            // Deconstruct phrases
            foreach (Phrase phrase in library.Phrases)
            {
                // Check if this phrase contains only non-IAudioFiles and if so don't include
                if (!phrase.Inflections.Select(x => x.AudioClip).Where(x => x is AudioFile).Any()) continue;

                PhraseTableRow phraseRow = new()
                {
                    Str = phrase.Str
                };

                // Deconstruct inflections
                List<InflectionTableRow> inflectionTable = [];
                foreach (InflectionType inflectionType in phrase.Inflections.Select(x => x.InflectionType))
                {
                    // Check if this inflection isn't IAudioFile, if so and don't include
                    if (phrase.GetAudioClip(inflectionType) is not AudioFile) continue;

                    var inflectionRow = new InflectionTableRow
                    {
                        Inflection = (int)inflectionType,
                        AudioFileName = SysPath.GetFileName(((AudioFile)phrase.GetAudioClip(inflectionType)).Path) // Gets the filename for the phrase's IAudioFile
                    };

                    inflectionTable.Add(inflectionRow);
                }

                phraseRow.NumInflections = inflectionTable.Count;
                phraseRow.InflectionTable = inflectionTable.ToArray();
                phraseTable.Add(phraseRow);
            }

            // Construct and return
            return new LibraryCacheFile(phraseTable.Count, phraseTable.ToArray());
        }

        /// <summary>
        /// Constructs a new instance of <see cref="Library"/> from this instance of <see cref="LibraryCacheFile"/>.
        /// </summary>
        /// <returns></returns>
        public Library Construct()
        {
            Terminal.BeginReport("Building from cache...");
            
            // Construct phrases
            List<Phrase> phrases = [];
            for (int i = 0; i < PhraseTable.Length; i++)
            {
                // Report progress
                if (i % 100 == 0)
                {
                    float percent = ((float)i / PhraseTable.Length) * 100;
                    Terminal.Report($"[{percent:0}%]");
                }
                
                bool unknownErrorNotified = false; // For the BEEG unknown error below
                
                // Construct inflections
                InflectionCollection inflections = [];
                foreach (InflectionTableRow inflectionRow in PhraseTable[i].InflectionTable)
                {
                    InflectionType inflectionType = (InflectionType)inflectionRow.Inflection;
                    AudioClip audioClip;
                    // In some specific circumstances the following statement throws a DirectoryNotFoundException, even if the directory is valid.
                    // I don't know what causes this, it only happens with certain directory names. I see no issue with my code, perhaps this is a bug with NAudio?
                    // I will print a message to the console notifying them to open an issue - if someone encounters it, it will be more information to diagnose with.
                    try
                    {
                        // Gets the path to the file, relative to this cache file's current directory.
                        // Path.GetDirectoryName() will never return null, as the cache file at 'path' will always been in a library folder
                        audioClip = new AudioFileStreaming(SysPath.Combine(SysPath.GetDirectoryName(Path)!, inflectionRow.AudioFileName));
                    }
                    catch (DirectoryNotFoundException)
                    {
                        // Pause report
                        Terminal.EndReport("--Build Paused--");

                        if (!unknownErrorNotified)
                        {
                            Terminal.BeginMessage();
                            Terminal.Message($"Couldn't load file: \"{inflectionRow.AudioFileName}\"", ConsoleColor.Red);
                            Terminal.Message();
                            Terminal.Message("This problem is known and occurs sometimes after moving or renaming a cached directory.");
                            Terminal.Message();
                            Terminal.Message("If you encounter this message, please open an issue on Github and include:");
                            Terminal.Message("\t- The name of the folder this Library used to be in");
                            Terminal.Message("\t- The name of the folder this Library is now in");
                            Terminal.Message("Or if you haven't moved the folder, let me know as well. The more data I have, the closer I'll be to fixing this!");
                            Terminal.Message();
                            Terminal.Message("The library will continue to load, but this file will be ignored.", ConsoleColor.Yellow);
                            Terminal.Message("You'll be notified of any further instances of this error.", ConsoleColor.Yellow);
                            Terminal.EndMessage();
                            Terminal.AwaitKey("Press any key to continue...");
                            unknownErrorNotified = true;
                        }
                        else
                        {
                            Terminal.MessageSingle($"Also couldn't load: {inflectionRow.AudioFileName}", ConsoleColor.Yellow);
                        }

                        // Resume report & continue
                        Terminal.BeginReport("Continuing to build from cache...");

                        continue;
                    }

                    inflections.Add(new Inflection(inflectionType, audioClip));
                }

                phrases.Add(new Phrase(PhraseTable[i].Str, inflections.ToArray()));
            }

            // Construct library and return
            Terminal.EndReport($"Successfully loaded {phrases.Count} phrases and {phrases.Select(x => x.Inflections.Length).Sum()} audio files.");
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
                bw.Write(Version); // Write the version number to the file

                // Compute and write folder hash
                // Path.GetDirectoryName() will never return null, as the cache file at 'path' will always been in a library folder
                var filenames = Directory.GetFiles(SysPath.GetDirectoryName(path)!).Select(x => SysPath.GetFileName(x));
                var filenames_string = "";
                foreach (var filename in filenames)
                    filenames_string += filename;
                byte[] folderHash = MD5.HashData(Encoding.ASCII.GetBytes(filenames_string));
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
                        bw.Write(inflection.AudioFileName);
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
