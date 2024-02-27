namespace CVAS.AudioEngine
{
    /// <summary>
    /// Interface representing any playable piece of audio originating from a file.
    /// </summary>
    public interface IAudioFile : IAudioClip
    {
        public string Path { get; }
        public long Offset { get; }
        public long Length { get; }
    }
}
