namespace CVAS.REPL
{
    internal class ClearCommand : ICommand
    {
        public string Str => "clear";

        public string Description => "Clears the currently loaded Library.";

        public string Usage => "clear";

        public ICommand? SubCommand { get; }

        public IArgument[] Arguments { get; } = { };

        public void RunFrom(string str)
        {
            // Validity check: str must begin with "load"
            if (!str.StartsWith(Str)) throw new CommandNotValidException();

            // Try to read arguments
            var temp_str = str.Substring(Str.Length).TrimStart();

            foreach (IArgument argument in Arguments) argument.ReadFrom(ref temp_str); // If this fails, an ArgumentNotValidException will be thrown, then caught by the REPL class.

            // Validity check: str must be empty after all arguments are read
            if (temp_str != "") throw new ArgumentNotValidException();

            // Clear current library
            REPL.Instance.CurrentLibrary = null;
            Console.WriteLine();
            Console.WriteLine("Current library cleared.");
            Console.WriteLine();
        }
    }
}
