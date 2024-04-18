using System.Runtime.InteropServices;

namespace CVAS.AudioEngine
{
    /// <summary>
    /// Represents a playable piece of silent audio, with a given length in milliseconds.
    /// </summary>
    public interface ISilence : IAudioClip
    {
        /// <summary>
        /// The length of this <see cref="ISilence"/> in milliseconds.
        /// </summary>
        public int Milliseconds { get; }

        /// <summary>
        /// Creates a new instance of <see cref="ISilence"/> with the given length.
        /// <br/>Selects between implementations based on OS (NAudio for Windows, BASS for all others).
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public static ISilence New(int milliseconds)
            => RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? new NAudio.Silence(milliseconds)
            : new BASS.Silence(milliseconds);
    }
}
