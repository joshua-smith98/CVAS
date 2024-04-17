using System.Runtime.InteropServices;

namespace CVAS.AudioEngine
{
    /// <summary>
    /// Represents a playable queue of <see cref="IAudioClip"/>s.
    /// </summary>
    public interface IPlaylist : IAudioClip
    {
        /// <summary>
        /// The collection of <see cref="IAudioClip"/>s to be played.
        /// </summary>
        public IAudioClip[] AudioClips { get; }

        /// <summary>
        /// Creates a new instance of <see cref="IPlaylist"/>.
        /// <br/>Selects between implementations based on OS (NAudio for Windows, BASS for all others).
        /// </summary>
        /// <param name="audioClips"></param>
        /// <returns></returns>
        public static IPlaylist New(IAudioClip[] audioClips)
            => RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? new NAudio.Playlist(audioClips.Cast<NAudio.AudioClip>().ToArray())
            : new BASS.Playlist(audioClips.Cast<BASS.AudioClip>().ToArray());
    }
}
