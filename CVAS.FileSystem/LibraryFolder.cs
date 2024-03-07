using CVAS.Core;

namespace CVAS.FileSystem
{
    public class LibraryFolder : IFolder<Library>
    {
        public string? Path { get; private set; }

        public Library Construct()
        {
            throw new NotImplementedException();
        }

        public IFolder<Library> Deconstruct(Library Object)
        {
            throw new TransmutationNotAllowedException();
        }

        public IFolder<Library> LoadFrom(string path)
        {
            throw new TransmutationNotAllowedException();
        }

        public void SaveTo(string path)
        {
            throw new TransmutationNotAllowedException();
        }
    }
}
