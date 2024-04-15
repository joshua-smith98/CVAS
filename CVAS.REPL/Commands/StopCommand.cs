// Conditional AudioEngine (NAudio for Windows and BASS for other OS)
#if Windows
using CVAS.WinAudioEngineNS;
#else
using CVAS.AudioEngineNS;
#endif

namespace CVAS.REPL
{
    /// <summary>
    /// A <see cref="Command"/> that stops all audio playback.
    /// </summary>
    internal class StopCommand : Command
    {
        public override string Str => "stop";

        public override string Description => "Stops all audio playback.";

        public override string[] Usage { get; } = ["stop"];

        public override Command[] SubCommands { get; } = [];

        public override Argument[] Arguments { get; } = [];

        protected override void VerifyArgsAndRun()
        {
            AudioEngine.Instance.StopAll();
        }
    }
}
