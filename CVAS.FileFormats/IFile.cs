namespace CVAS.FileFormats
{
    /// <summary>
    /// Class representing some file, which can be transformed into or built from an object of class <see cref="T"/>.
    /// </summary>
    /// <typeparam name="T">The class this file can be transformed into or built from.</typeparam>
    public interface IFile<T> where T : class
    {
        /// <summary>
        /// The path to this file.
        /// </summary>
        public string? Path { get; }
        /// <summary>
        /// The header of this file.
        /// </summary>
        public char[] Header { get; }

        /// <summary>
        /// Validates and loads the file at the given path into an instance of <see cref="IFile{T}"/>.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static abstract IFile<T> LoadFrom(string path);
        /// <summary>
        /// Deconstructs an instance of class <see cref="T"/> into an instance <see cref="IFile{T}"/>.
        /// </summary>
        /// <param name="Object">The object to deconstruct.</param>
        /// <returns></returns>
        public static abstract IFile<T> Deconstruct(T Object);

        /// <summary>
        /// Saves this instance to the file at the given path, overwriting the existing file or creating new one if it doesn't exist.
        /// </summary>
        /// <param name="path"></param>
        public void SaveTo(string path);
        /// <summary>
        /// Constructs an object of class <see cref="T"/> from this instance of <see cref="IFile{T}"/>.
        /// </summary>
        /// <returns></returns>
        public T Construct();
    }
}