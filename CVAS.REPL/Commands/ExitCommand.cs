namespace CVAS.REPL
{
    /// <summary>
    /// A <see cref="Command"/> that sets <see cref="REPL.IsRunning"/> to <see cref="false"/>, and therefore halts the REPL.
    /// </summary>
    internal class ExitCommand : Command
    {
        public override string Str => "exit";

        public override string Description => "Exits the program.";

        public override string[] Usage { get; } = { "exit" };

        public override Command[] SubCommands { get; } = Array.Empty<Command>();

        public override Argument[] Arguments { get; } = Array.Empty<Argument>();

        protected override void VerifyArgsAndRun()
        {
            // End REPL loop
            REPL.Instance.IsRunning = false;
        }
    }
}
