namespace CVAS.AudioEngine
{
    public interface IAudioFile : IAudioClip
    {
        public string path { get; }
        public long offset { get; }
        public long length { get; }
    }
}
