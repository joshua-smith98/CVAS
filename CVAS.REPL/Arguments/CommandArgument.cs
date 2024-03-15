namespace CVAS.REPL
{
    /// <summary>
    /// An <see cref="Argument"/> that attempts to read an <see cref="Command"/>. Used primarily in the 'help' command.
    /// </summary>
    internal class CommandArgument : Argument
    {
        public string Name { get; }

        public object? Value { get; private set; }

        public Type ValueType => typeof(Command);

        internal CommandArgument(string name)
        {
            Name = name;
        }

        public void ReadFrom(ref string str)
        {
            // Validity check: str must not be empty, and must start with an ICommand.Str
            var strFirst = str.Split().First();

            if (str == "") throw new ArgumentNotValidException($"Expected argument [{Name}], found nothing!");
            if (!REPL.Instance.CommandInstances.Any(x => x.Str == strFirst)) throw new ArgumentNotValidException($"Command '{strFirst}' specified in argument [{Name}] does not exist!");

            // Store relevant ICommand
            Value = REPL.Instance.CommandInstances.Where(x => x.Str == strFirst).First();

            // Trim ref str
            str = str.Substring(strFirst.Length).TrimStart();
        }
    }
}
