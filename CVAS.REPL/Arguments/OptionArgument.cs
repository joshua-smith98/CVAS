namespace CVAS.REPL
{
    /// <summary>
    /// An <see cref="IArgument"/> that attempts to read a string prefixed by a dash (-). E.g. "command -option".
    /// </summary>
    internal class OptionArgument : IArgument
    {
        public object? Value { get; private set; }

        public Type ValueType => typeof(string);

        internal OptionArgument() { }

        public void ReadFrom(ref string str)
        {
            // Validity check: string must not be empty, and must start with '-'
            if (str == "" || str[0] != '-') throw new ArgumentNotValidException();

            // Get option, and remove '-' from the beginning
            var option = str.Split().First().Substring(1);
            Value = option;

            // Trim ref str
            str = str.Substring(option.Length + 1).TrimStart();
        }
    }
}
