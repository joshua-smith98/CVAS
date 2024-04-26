using CVAS.FileSystem;
using CVAS.TerminalNS;

namespace CVAS.REPL
{
    /// <summary>
    /// A <see cref="Command"/> that loads a library from a folder into the current REPL context.
    /// </summary>
    internal class LoadCommand : Command
    {
        public override string Str => "load";

        public override string Description => "Loads a folder of audio files as a library of phrases.";

        public override string[] Usage { get; } = ["load [path]"];

        public override Command[] SubCommands { get; } = [];

        public override Argument[] Arguments { get; } =
        [
            new StringArgument("path", true), // Library path
        ];

        protected override void VerifyArgsAndRun()
        {
            // Get library path from relevant argument
            var Path = (string)Arguments[0].Value!;

            // Validity check: path must point to a folder
            if (!Directory.Exists(Path)) throw new CommandRunFailedException($"Couldn't find directory: '{Path}'");

            // Try to load library into REPL context
            try
            {
                REPL.Instance.CurrentLibrary = LibraryFolder.LoadFrom(Path).Construct();
            }
            catch(FileSystemException e)
            {
                throw new CommandRunFailedException($"Failed to load library: {e.Message}"); // Throw exception upon failure (this should only ever be for an empty folder)
            }
        }
    }
}
