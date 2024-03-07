namespace CVAS.FileSystem
{
    public interface IFolder<T> where T : class
    {
        public string? Path { get; }

        public static abstract IFolder<T> LoadFrom(string path);
        public static abstract IFolder<T> Deconstruct(T Object);
        public void SaveTo(string path);
        public T Construct();
    }
}
