namespace CVAS.FileSystem
{
    public interface IFolder<T> where T : class
    {
        public string? Path { get; }

        public IFolder<T> LoadFrom(string path);
        public IFolder<T> Deconstruct(T Object);
        public void SaveTo(string path);
        public T Construct();
    }
}
