namespace CVAS.REPL
{
    /// <summary>
    /// An <see cref="Argument"/> that attempts to read a string prefixed by a dash (-). E.g. "command -option".
    /// </summary>
    internal class OptionArgument(string name, bool isCompulsory) : Argument(name, isCompulsory)
    {
        public override Type ValueType => typeof(string);

        protected override string ReadFromImpl(string str)
        {
            // Validity check: string must start with '-'
            if (str[0] != '-') throw new ArgumentValueNotValidException($"[{Name}] is of type OptionArgument, and must begin with a hyphen (-).");

            // Get option, and remove '-' from the beginning
            var option = str.Split().First()[1..];
            Value = option;

            // Return trimmed str
            return str[(option.Length + 1)..].TrimStart();
        }
    }
}
