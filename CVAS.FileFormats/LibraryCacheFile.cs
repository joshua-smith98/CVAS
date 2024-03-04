﻿using CVAS.AudioEngine;
using CVAS.DataStructure;
using System.Security.Cryptography;
using System.Text;

namespace CVAS.FileFormats
{
    public class LibraryCacheFile : IFile
    {
        public string? Path { get; private set; }

        public Type Type => typeof(Library);

        public char[] Header => "CVASLBCH".ToArray();

        private byte[]? FolderHash;
        private int NumPhrases;

        private PhraseTableRow[] PhraseTable;
        private struct PhraseTableRow
        {
            public string Str;
            public int NumInflections;
            public InflectionTableRow[] InflectionTable;
            public struct InflectionTableRow
            {
                public int Inflection;
                public string AudioFilePath;
            }
        }

        private LibraryCacheFile(string path, byte[] folderHash, int numPhrases, PhraseTableRow[] phraseTable)
        {
            Path = path;
            FolderHash = folderHash;
            NumPhrases = numPhrases;
            PhraseTable = phraseTable;
        }

        private LibraryCacheFile(int numPhrases, PhraseTableRow[] phraseTable)
        {
            NumPhrases = numPhrases;
            PhraseTable = phraseTable;
        }

        public static IFile LoadFrom(string path)
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
                    var folderHash = new byte[16];
                    using (var md5 = MD5.Create())
                        folderHash = md5.ComputeHash(Encoding.ASCII.GetBytes(filenames_string));

                    if (br.ReadBytes(16) != folderHash) throw new InvalidFileHashException();

                    // Validity check: table integrity (iterate through tables and check EOF)
                    try
                    {
                        int numPhrases = br.ReadInt32();
                        for (int i = 0; i < numPhrases; i++)
                        {
                            br.ReadString(); // Phrase str
                            int numInflections = br.ReadInt32();
                            for (int j = 0; j < numInflections; j++)
                            {
                                br.ReadInt32(); // Inflection
                                br.ReadString(); // Audio file path
                            }
                        }
                    }
                    catch (IndexOutOfRangeException) // Case: running through table 
                    {
                        throw new InvalidFileFormatException();
                    }

                    if (br.BaseStream.Position != br.BaseStream.Length) throw new InvalidFileFormatException(); // Case: there is extra data in the file
                }
                #endregion

                // Reset position after checking file validity
                br.BaseStream.Position = 0;

                #region Load from file and construct
                {
                    // Skip 2 x 4 byte headers
                    br.BaseStream.Position = 8;

                    // Load folder hash
                    byte[] folderHash = br.ReadBytes(16);

                    // Read phrases
                    int numPhrases = br.ReadInt32();
                    PhraseTableRow[] phraseTable = new PhraseTableRow[numPhrases];

                    for (int i = 0; i < numPhrases; i++)
                    {
                        phraseTable[i] = new PhraseTableRow();
                        phraseTable[i].Str = br.ReadString();
                        phraseTable[i].NumInflections = br.ReadInt32();
                        phraseTable[i].InflectionTable = new PhraseTableRow.InflectionTableRow[phraseTable[0].NumInflections];

                        // Read inflections
                        for (int j = 0; j < phraseTable[0].NumInflections; j++)
                        {
                            phraseTable[i].InflectionTable[j] = new PhraseTableRow.InflectionTableRow();
                            phraseTable[i].InflectionTable[j].Inflection = br.ReadInt32();
                            phraseTable[i].InflectionTable[j].AudioFilePath = br.ReadString();
                        }
                    }

                    ret = new LibraryCacheFile(path, folderHash, numPhrases, phraseTable);
                }
                #endregion
            }

            return ret;
        }

        public static IFile Deconstruct(object o)
        {
            // Validity check: o is a Library
            if (o is not Library) throw new InvalidObjectTypeException();

            Library library = (Library)o;

            // Initialise variables for LibraryCacheFile construction
            // NOTE: We will need to use lists here for the phraseTable and inflectionTable, due to our need to exclude non-IAudioFile phrases and inflections
            List<PhraseTableRow> phraseTable = new List<PhraseTableRow>();
            foreach (Phrase phrase in library.Phrases)
            {
                // Check if this phrase contains only non-IAudioFiles and if so don't include
                if (phrase.Inflections.Select(x => x.AudioClip).Where(x => x is IAudioFile).Count() == 0) continue;
                
                PhraseTableRow phraseRow = new PhraseTableRow();
                phraseRow.Str = phrase.Str;

                List<PhraseTableRow.InflectionTableRow> inflectionTable = new List<PhraseTableRow.InflectionTableRow>();
                foreach (InflectionType inflectionType in phrase.Inflections.Select(x => x.InflectionType))
                {
                    // Check if this inflection isn't IAudioFile, if so and don't include
                    if (phrase.GetAudioClip(inflectionType) is not IAudioFile) continue;
                    
                    var inflectionRow = new PhraseTableRow.InflectionTableRow();
                    inflectionRow.Inflection = (int)inflectionType;
                    inflectionRow.AudioFilePath = ((IAudioFile)phrase.GetAudioClip(inflectionType)).Path;

                    inflectionTable.Add(inflectionRow);
                }

                phraseRow.InflectionTable = inflectionTable.ToArray();
                phraseTable.Add(phraseRow);
            }

            return new LibraryCacheFile(phraseTable.Count(), phraseTable.ToArray());
        }

        public object Construct()
        {
            // Construct phrases
            List<Phrase> phrases = new List<Phrase>();
            foreach (PhraseTableRow phraseRow in PhraseTable)
            {
                Phrase phrase;
                if (phraseRow.NumInflections == 0) // Case: no included inflections (should never happen, but just in case)
                    phrase = new Phrase(phraseRow.Str);
                else if (phraseRow.NumInflections == 1)
            }
        }

        public void SaveTo(string path)
        {
            using (BinaryWriter bw = new(File.Create(path)))
            {
                // Write headers
                bw.Write("CVASLBCH");

                // Compute and write folder hash
                var filenames = Directory.GetFiles(System.IO.Path.GetDirectoryName(path)).Select(x => System.IO.Path.GetFileName(x)); // TODO: handle possible null reference
                var filenames_string = "";
                foreach (var filename in filenames)
                    filenames_string += filename;
                var folderHash = new byte[16];
                bw.Write(folderHash);

                // Write phrases & inflections
                bw.Write(NumPhrases);
                foreach (PhraseTableRow phrase in PhraseTable)
                {
                    bw.Write(phrase.Str);
                    bw.Write(phrase.NumInflections);
                    foreach (PhraseTableRow.InflectionTableRow inflection in phrase.InflectionTable)
                    {
                        bw.Write(inflection.Inflection);
                        bw.Write(inflection.AudioFilePath);
                    }
                }

                bw.Flush();
            }

            Path = path;
        }
    }
}
