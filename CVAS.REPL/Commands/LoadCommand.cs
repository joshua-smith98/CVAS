using CVAS.Core;
using CVAS.FileSystem;

namespace CVAS.REPL
{
    /// <summary>
    /// An <see cref="Command"/> that loads a library from a folder into the current REPL context.
    /// </summary>
    internal class LoadCommand : Command
    {
        public override string Str => "load";

        public override string Description => "Loads a folder of audio files as a library of phrases.";

        public override string[] Usage { get; } = { "load [path]" };

        public override Command[] SubCommands { get; } = { };

        public override Argument[] Arguments { get; } =
        {
            new StringArgument("path", true), // Library path
        };

        protected override void VerifyArgsAndRun()
        {
            var Path = (string)Arguments[0].Value!;

            // Validity check: path must point to a folder
            if (!Directory.Exists(Path)) throw new ArgumentNotValidException($"Couldn't find directory: '{Path}'");

            // Load library into REPL context
            REPL.Instance.CurrentLibrary?.Dispose();
            REPL.Instance.CurrentLibrary = LibraryFolder.LoadFrom(Path).Construct();
        }
    }
}
