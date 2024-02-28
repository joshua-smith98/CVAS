namespace CVAS.REPL
{
    public class CommandArgument : IArgument
    {
        public object? Value { get; private set; }

        public Type ValueType => typeof(ICommand);

        public void ReadFrom(ref string str)
        {
            // Validity check: str must not be empty, and must start with an ICommand.Str
            var strFirst = str.Split().First();

            if (str == "" || !Command.Commands.Any(x => x.Str == strFirst)) throw new ArgumentNotValidException();

            Value = Command.Commands.Where(x => x.Str == strFirst).First();

            str = str.Substring(strFirst.Length).TrimStart();
        }
    }
}
