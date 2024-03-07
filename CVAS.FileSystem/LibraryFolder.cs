using CVAS.AudioEngine;
using CVAS.Core;
using SysPath = System.IO.Path;

namespace CVAS.FileSystem
{
    public class LibraryFolder : IFolder<Library>
    {
        public string? Path { get; private set; }

        public string[] FileNames { get; }

        public LibraryCacheFile? LibraryCacheFile { get; private set; }

        private LibraryFolder(string[] FileNames)
        {
            this.FileNames = FileNames;
        }

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
            throw new NotImplementedException();
        }

        public void SaveTo(string path)
        {
            throw new TransmutationNotAllowedException();
        }
    }
}
