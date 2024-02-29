namespace CVAS.REPL
{
    internal class CommandArgument : IArgument
    {
        public object? Value { get; private set; }

        public Type ValueType => typeof(ICommand);

        internal CommandArgument() { }

        public void ReadFrom(ref string str)
        {
            // Validity check: str must not be empty, and must start with an ICommand.Str
            var strFirst = str.Split().First();

            if (str == "" || !REPL.Instance.CommandInstances.Any(x => x.Str == strFirst)) throw new ArgumentNotValidException();

            Value = REPL.Instance.CommandInstances.Where(x => x.Str == strFirst).First();

            str = str.Substring(strFirst.Length).TrimStart();
        }
    }
}
