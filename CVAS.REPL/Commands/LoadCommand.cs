using CVAS.Core;
using CVAS.FileSystem;

namespace CVAS.REPL
{
    /// <summary>
    /// An <see cref="Command"/> that loads a library from a folder into the current REPL context.
    /// </summary>
    internal class LoadCommand : Command
    {
        public string Str => "load";

        public string Description => "Loads a folder of audio files as a library of phrases.";

        public string[] Usage { get; } = { "load [path]" };

        public Command? SubCommand => null; // To be implemented at some point

        public Argument[] Arguments { get; } =
        {
            new StringArgument("path"), // Library path
        };

        internal LoadCommand() { }

        public void RunFrom(string str)
        {
            // Validity check: str must begin with "load"
            if (!str.StartsWith(Str)) throw new CommandNotValidException("Command does not match. This message should never be printed - if it was, open an issue on Github!");

            // Try to read arguments
            var temp_str = str.Substring(Str.Length).TrimStart();

            foreach (Argument argument in Arguments)
            {
                argument.ReadFrom(ref temp_str); // If this fails, an ArgumentNotValidException will be thrown, then caught by the REPL class.
            }

            // Validity check: str must be empty after all arguments are read
            if (temp_str != "") throw new ArgumentNotValidException($"Expected end of command, found: '{temp_str}'!");

            var Path = Arguments[0].Value as string;

            // Validity check: path must point to a folder
            if (!Directory.Exists(Path)) throw new ArgumentNotValidException($"Couldn't find directory: '{Path}'");

            // Load library into REPL context
            REPL.Instance.CurrentLibrary?.Dispose();
            REPL.Instance.CurrentLibrary = LibraryFolder.LoadFrom(Path).Construct();
        }
    }
}
