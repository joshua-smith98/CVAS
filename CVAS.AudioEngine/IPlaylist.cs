using System.Runtime.InteropServices;

namespace CVAS.AudioEngine
{
    public interface IPlaylist : IAudioClip
    {
        public IAudioClip[] AudioClips { get; }

        public static IPlaylist New(IAudioClip[] audioClips)
            => RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? new NAudio.Playlist(audioClips.Cast<NAudio.AudioClip>().ToArray())
            : new BASS.Playlist(audioClips.Cast<BASS.AudioClip>().ToArray());
    }
}
