using CVAS.DataStructure;

namespace CVAS.REPL
{
    /// <summary>
    /// An <see cref="ICommand"/> that loads a library from a folder into the current REPL context.
    /// </summary>
    internal class LoadCommand : ICommand
    {
        public string Str => "load";

        public string Description => "Loads a folder of audio files as a library of phrases.";

        public string[] Usage { get; } = { "load [path]" };

        public ICommand? SubCommand => null; // To be implemented at some point

        public IArgument[] Arguments { get; } =
        {
            new StringArgument(), // Library path
        };

        internal LoadCommand() { }

        public void RunFrom(string str)
        {
            // Validity check: str must begin with "load"
            if (!str.StartsWith(Str)) throw new CommandNotValidException();

            // Try to read arguments
            var temp_str = str.Substring(Str.Length).TrimStart();

            foreach (IArgument argument in Arguments)
            {
                argument.ReadFrom(ref temp_str); // If this fails, an ArgumentNotValidException will be thrown, then caught by the REPL class.
            }

            // Validity check: str must be empty after all arguments are read
            if (temp_str != "") throw new ArgumentNotValidException();

            var Path = Arguments[0].Value as string;

            // Validity check: path must point to a folder
            if (!Directory.Exists(Path)) throw new ArgumentNotValidException();

            // Load library into REPL context
            REPL.Instance.CurrentLibrary = Library.LoadFromFolder(Path);
        }
    }
}
