namespace CVAS.REPL
{
    /// <summary>
    /// An <see cref="Argument"/> that attempts to read a string prefixed by a dash (-). E.g. "command -option".
    /// </summary>
    internal class OptionArgument : Argument
    {
        public string Name { get; }
        
        public object? Value { get; private set; }

        public Type ValueType => typeof(string);

        internal OptionArgument(string name)
        {
            Name = name;
        }

        public void ReadFrom(ref string str)
        {
            // Validity check: string must not be empty, and must start with '-'
            if (str == "") throw new ArgumentNotValidException($"Expected argument [{Name}], found nothing!");
            if (str[0] != '-') throw new ArgumentNotValidException($"[{Name}] is of type OptionArgument, and must begin with a hyphen (-).");

            // Get option, and remove '-' from the beginning
            var option = str.Split().First().Substring(1);
            Value = option;

            // Trim ref str
            str = str.Substring(option.Length + 1).TrimStart();
        }
    }
}
