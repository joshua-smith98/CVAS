namespace CVAS.REPL
{
    /// <summary>
    /// Interface representing an argument for a command, which can be read from a string.
    /// </summary>
    internal abstract class Argument
    {
        /// <summary>
        /// The name of this <see cref="Argument"/> instance. Used to refer to this argument when attempting to read it via an <see cref="Command"/>.
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// Value of the argument. Will be <see cref="null"/> until read.
        /// </summary>
        public object? Value { get; }

        /// <summary>
        /// The <see cref="Type"/> of this argument's value.
        /// </summary>
        public abstract Type ValueType { get; }

        public bool IsCompulsory { get; }

        public Argument(string name, bool isCompulsory)
        {
            Name = name;
            IsCompulsory = isCompulsory;
        }

        /// <summary>
        /// Validates, and attempts to read an argument from the given string. When successful, will also trim this argument from the given string.
        /// </summary>
        /// <param name="str"></param>
        public string ReadFrom(string str)
        {
            try
            {
                VerifyFrom(str);
                return ReadFromImpl(str);
            }
            catch(REPLException e)
            {
                if (IsCompulsory) throw e;
                else return str;
            }
        }

        private void VerifyFrom(string str)
        {
            if (str == "") throw new ArgumentNotValidException($"Expected argument [{Name}], found nothing!");
        }

        protected abstract string ReadFromImpl(string str);
    }
}