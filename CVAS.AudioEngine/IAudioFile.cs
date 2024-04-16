namespace CVAS.AudioEngine
{
    public interface IAudioFile : IAudioClip
    {
        public abstract string Path { get; }
    }
}
