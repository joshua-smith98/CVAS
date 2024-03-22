using CVAS.AudioEngineNS;

namespace CVAS.REPL
{
    /// <summary>
    /// A <see cref="Command"/> that stops all audio playback.
    /// </summary>
    internal class StopCommand : Command
    {
        public override string Str => "stop";

        public override string Description => "Stops all audio playback.";

        public override string[] Usage { get; } = { "stop" };

        public override Command[] SubCommands { get; } = Array.Empty<Command>();

        public override Argument[] Arguments { get; } = Array.Empty<Argument>();

        protected override void VerifyArgsAndRun()
        {
            AudioEngine.Instance.StopAll();
        }
    }
}
