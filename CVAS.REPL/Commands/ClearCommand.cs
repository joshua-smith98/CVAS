using CVAS.TerminalNS;

namespace CVAS.REPL
{
    /// <summary>
    /// A <see cref="Command"/> that clears the current context.
    /// </summary>
    internal class ClearCommand : Command
    {
        public override string Str => "clear";

        public override string Description => "Clears the currently loaded Library and any memorised Sentence.";

        public override string[] Usage { get; } = { "clear" };

        public override Command[] SubCommands { get; } = { };

        public override Argument[] Arguments { get; } = { };

        protected override void VerifyArgsAndRun()
        {
            // Validity check: a library must be loaded
            if (REPL.Instance.CurrentLibrary is null) throw new ContextNotValidException("No library is currently loaded.");

            // Clear current sentence
            REPL.Instance.CurrentSentence?.Dispose();
            REPL.Instance.CurrentSentence = null;

            // Clear current library
            REPL.Instance.CurrentLibrary.Dispose();
            REPL.Instance.CurrentLibrary = null;
            GC.Collect(); // To free up any memory used by the library
            Terminal.MessageSingle("Current library cleared.");
        }
    }
}
