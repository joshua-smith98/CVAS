namespace CVAS.REPL
{
    /// <summary>
    /// An <see cref="Argument"/> that attempts to read an <see cref="Command"/>. Used primarily in the 'help' command.
    /// </summary>
    internal class CommandArgument : Argument
    {
        public override Type ValueType => typeof(Command);

        internal CommandArgument(string name, bool isCompulsory) : base(name, isCompulsory) { }

        protected override string ReadFromImpl(string str)
        {
            // Validity check: str must start with a Command.Str
            var strFirst = str.Split().First();
            if (!REPL.Instance.CommandInstances.Any(x => x.Str == strFirst)) throw new ArgumentReadFailedException($"Command '{strFirst}' specified in argument [{Name}] does not exist!");

            // Store relevant Command
            Value = REPL.Instance.CommandInstances.Where(x => x.Str == strFirst).First();

            // Return trimmed string
            return str[strFirst.Length..].TrimStart();
        }
    }
}
