namespace CVAS.FileFormats
{
    public interface IFile<T> where T : class
    {
        public string? Path { get; }
        public char[] Header { get; }

        public static abstract IFile<T> LoadFrom(string path);
        public static abstract IFile<T> Deconstruct(T Object);

        public void SaveTo(string path);
        public T Construct();
    }
}