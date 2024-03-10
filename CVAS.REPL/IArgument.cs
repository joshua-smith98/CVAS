namespace CVAS.REPL
{
    /// <summary>
    /// Interface representing an argument for a command, which can be read from a string.
    /// </summary>
    internal interface IArgument
    {
        /// <summary>
        /// The name of this <see cref="IArgument"/> instance. Used to refer to this argument when attempting to read it via an <see cref="ICommand"/>.
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// Value of the argument. Will be <see cref="null"/> until read.
        /// </summary>
        public object? Value { get; }

        /// <summary>
        /// The <see cref="Type"/> of this argument's value.
        /// </summary>
        public Type ValueType { get; }

        /// <summary>
        /// Validates, and attempts to read an argument from the given string. When successful, will also trim this argument from the given string.
        /// </summary>
        /// <param name="str"></param>
        public void ReadFrom(ref string str);
    }
}