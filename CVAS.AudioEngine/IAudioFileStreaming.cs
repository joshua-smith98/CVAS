using System.Runtime.InteropServices;

namespace CVAS.AudioEngine
{
    /// <summary>
    /// A playable piece of audio from a file, that is streamed directly from the disk.
    /// </summary>
    public interface IAudioFileStreaming : IAudioFile
    {
        /// <summary>
        /// Creates a new instance of <see cref="IAudioFileStreaming"/>.
        /// <br/>Selects between implementations based on OS (NAudio for Windows, BASS for all others).
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IAudioFileStreaming New(string path)
            => RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? new NAudio.AudioFileStreaming(path)
            : new BASS.AudioFileStreaming(path);
    }
}
