namespace CVAS.AudioEngine
{
    public interface IPlaylist : IAudioClip
    {
        public IAudioClip[] AudioClips { get; }
    }
}
