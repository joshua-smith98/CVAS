namespace CVAS.FileSystem
{
    /// <summary>
    /// Class representing some folder, which can be transformed into or built from an object of class <see cref="T"/>.
    /// </summary>
    /// <typeparam name="T">The class this folder can be transformed into or built from.</typeparam>
    public interface IFolder<T> where T : class
    {
        /// <summary>
        /// The path to this folder.
        /// </summary>
        public string? Path { get; }

        /// <summary>
        /// Validates and loads the folder at the given path into an instance of <see cref="IFolder{T}"/>.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static abstract IFolder<T> LoadFrom(string path);
        /// <summary>
        /// Deconstructs an instance of class <see cref="T"/> into an instance <see cref="IFolder{T}"/>.
        /// </summary>
        /// <param name="Object">The object to deconstruct.</param>
        /// <returns></returns>
        public static abstract IFolder<T> Deconstruct(T Object);

        /// <summary>
        /// Saves this instance into the folder at the given path.
        /// </summary>
        /// <param name="path"></param>
        public void SaveTo(string path);
        /// <summary>
        /// Constructs an object of class <see cref="T"/> from this instance of <see cref="IFolder{T}"/>.
        /// </summary>
        /// <returns></returns>
        public T Construct();
    }
}
