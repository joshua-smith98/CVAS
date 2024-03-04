namespace CVAS.FileFormats
{
    public interface IFile
    {
        public string? Path { get; }
        public Type Type { get; }
        public char[] Header { get; }

        public static abstract IFile LoadFrom(string path);
        public static abstract IFile Deconstruct(object o);

        public void SaveTo(string path);
        public object Construct();
    }
}