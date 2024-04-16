using System.Runtime.InteropServices;

namespace CVAS.AudioEngine
{
    public interface ISilence : IAudioClip
    {
        public int Milliseconds { get; }

        public static ISilence New(int milliseconds)
            => RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? new NAudio.Silence(milliseconds)
            : new BASS.Silence(milliseconds);
    }
}
