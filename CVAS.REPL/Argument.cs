namespace CVAS.REPL
{
    /// <summary>
    /// Represents an argument for a command which can be read from a string.
    /// </summary>
    internal abstract class Argument(string name, bool isCompulsory)
    {
        /// <summary>
        /// The name of this <see cref="Argument"/> instance. Used to refer to this argument when attempting to read it via an <see cref="Command"/>.
        /// </summary>
        public string Name => name;

        /// <summary>
        /// Value of the argument. Will be <see cref="null"/> until read.
        /// </summary>
        public object? Value { get; protected set; }

        /// <summary>
        /// The <see cref="Type"/> of this argument's value.
        /// </summary>
        public abstract Type ValueType { get; }

        /// <summary>
        /// Determines whether this argument is required by the parent command. When set to <see cref="false"/>, no exceptions will be thrown if invalid.
        /// </summary>
        public bool IsCompulsory => isCompulsory;

        /// <summary>
        /// Validates, and attempts to read an argument from the given string. When successful, will also trim this argument from the given string.
        /// </summary>
        /// <param name="str"></param>
        /// <exception cref="ArgumentNotValidException"/>
        public string ReadFrom(string str)
        {
            try
            {
                VerifyFrom(str);
                return ReadFromImpl(str);
            }
            catch(REPLException)
            {
                Value = null;
                
                if (IsCompulsory) throw;
                else return str;
            }
        }

        /// <summary>
        /// Verifies that this argument exists.
        /// </summary>
        /// <param name="str"></param>
        /// <exception cref="ArgumentNotValidException"></exception>
        private void VerifyFrom(string str)
        {
            if (str == "") throw new ArgumentNotValidException($"Expected argument [{Name}], found nothing!");
        }

        /// <summary>
        /// Abstract method that provides the implementation for reading this argument, along with any further validation.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        protected abstract string ReadFromImpl(string str);
    }
}