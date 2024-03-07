using CVAS.AudioEngine;
using CVAS.Core;
using SysPath = System.IO.Path;

namespace CVAS.FileSystem
{
    public class LibraryFolder : IFolder<Library>
    {
        public string? Path { get; private set; }

        public string[] AudioFileNames { get; }

        public LibraryCacheFile? LibraryCacheFile { get; private set; }

        private LibraryFolder(string[] audioFileNames)
        {
            AudioFileNames = audioFileNames;
        }

        public IFolder<Library> LoadFrom(string path)
        {
            // Validity checks: path is a valid directory
            if (!Directory.Exists(path)) throw new DirectoryNotFoundException();

            LibraryCacheFile libraryCacheFile;
            List<string> audioFileNames  = new List<string>();

            // Try to load LibraryCacheFile and extract AudioFileNames from that
            try
            {
                libraryCacheFile = (LibraryCacheFile)LibraryCacheFile.LoadFrom(SysPath.Combine(path, "cache.lbc"));

                foreach (LibraryCacheFile.PhraseTableRow phraseRow in libraryCacheFile.PhraseTable)
                {
                    foreach (LibraryCacheFile.InflectionTableRow inflectionRow in phraseRow.InflectionTable)
                    {
                        audioFileNames.Add(inflectionRow.AudioFileName);
                    }
                }

                return new LibraryFolder(audioFileNames.ToArray()) { LibraryCacheFile = libraryCacheFile };
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("A cache does not exist for this folder: a first-time analysis will be performed.");
            }
            catch (InvalidFileHashException)
            {
                Console.WriteLine("The folder contents have changed: a cache will be rebuilt.");
            }

            // Otherwise, load filenames from Directory.GetFiles(), checking for validity as audio files
            foreach (string fileName in Directory.GetFiles(path))
            {
                try
                {
                    using var a = new AudioFileStreaming(fileName) ; // Test to see if it is a valid audio file

                    audioFileNames.Add(SysPath.GetFileName(fileName));
                }
                catch { }
            }

            return new LibraryFolder(audioFileNames.ToArray());
        }

        public Library Construct()
        {
            throw new NotImplementedException();
        }

        public IFolder<Library> Deconstruct(Library Object)
        {
            throw new TransmutationNotAllowedException();
        }

        public void SaveTo(string path)
        {
            throw new TransmutationNotAllowedException();
        }
    }
}
