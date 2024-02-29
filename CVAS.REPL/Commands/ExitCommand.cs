namespace CVAS.REPL
{
    internal class ExitCommand : ICommand
    {
        public string Str => "exit";

        public string Description => "Exits the program.";

        public string Usage => "exit";

        public ICommand? SubCommand { get; }

        public IArgument[] Arguments { get; } = { };

        public void RunFrom(ref string str)
        {
            // Validity check: str must begin with "load"
            if (!str.StartsWith(Str)) throw new CommandNotValidException();

            // Try to read arguments
            var temp_str = str.Substring(Str.Length);

            foreach (IArgument argument in Arguments) argument.ReadFrom(ref temp_str); // If this fails, an ArgumentNotValidException will be thrown, then caught by the REPL class.

            // Validity check: str must be empty after all arguments are read
            if (temp_str != "") throw new CommandNotValidException();

            // Run command
            REPL.Instance.isRunning = false;
        }
    }
}
