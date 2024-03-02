using CVAS.DataStructure;

namespace CVAS.FileFormats
{
    public class LibraryCacheFile : IFile
    {
        public string? Path { get; private set; }

        public Type Type => typeof(Library);

        public char[] Header => "CVASLBCH".ToArray();

        private byte[] FolderHash;
        private int NumPhrases;

        private PhraseTableRow[] PhraseTable;

        private LibraryCacheFile(string path, byte[] folderHash, int numPhrases, PhraseTableRow[] phraseTable)
        {
            Path = path;
            FolderHash = folderHash;
            NumPhrases = numPhrases;
            PhraseTable = phraseTable;
        }

        private LibraryCacheFile(byte[] folderHash, int numPhrases, PhraseTableRow[] phraseTable)
        {
            FolderHash = folderHash;
            NumPhrases = numPhrases;
            PhraseTable = phraseTable;
        }

        public static IFile Deconstruct(object o)
        {
            throw new NotImplementedException();
        }

        public static IFile LoadFrom(string path)
        {
            throw new NotImplementedException();
        }

        public static void Validate()
        {
            throw new NotImplementedException();
        }

        public void Construct(ref object o)
        {
            throw new NotImplementedException();
        }

        public void SaveTo(string path)
        {
            throw new NotImplementedException();
        }
    }
}
