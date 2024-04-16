using System.Runtime.InteropServices;

namespace CVAS.AudioEngine
{
    public interface IAudioFileStreaming : IAudioFile
    {
        public static IAudioFileStreaming New(string path)
            => RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? new NAudio.AudioFileStreaming(path)
            : new BASS.AudioFileStreaming(path);
    }
}
