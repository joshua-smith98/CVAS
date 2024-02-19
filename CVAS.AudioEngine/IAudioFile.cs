namespace CVAS.AudioEngine
{
    /// <summary>
    /// Interface representing any playable piece of audio originating from a file.
    /// </summary>
    public interface IAudioFile : IAudioClip
    {
        public string path { get; }
        public long offset { get; }
        public long length { get; }
    }
}
