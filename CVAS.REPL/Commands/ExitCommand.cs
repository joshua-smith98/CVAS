namespace CVAS.REPL
{
    /// <summary>
    /// An <see cref="Command"/> that sets <see cref="REPL.IsRunning"/> to <see cref="false"/>, and therefore halts the REPL.
    /// </summary>
    internal class ExitCommand : Command
    {
        public override string Str => "exit";

        public override string Description => "Exits the program.";

        public override string[] Usage { get; } = { "exit" };

        public override Command[] SubCommands { get; } = { };

        public override Argument[] Arguments { get; } = { };

        protected override void VerifyArgsAndRun()
        {
            // End REPL loop
            REPL.Instance.IsRunning = false;
        }
    }
}
