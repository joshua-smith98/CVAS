namespace CVAS.AudioEngineNS
{
    /// <summary>
    /// Interface representing any playable piece of audio originating from a file.
    /// </summary>
    public interface IAudioFile : IAudioClip
    {
        public string Path { get; }
        // public long Offset { get; } For future use ? accessing embedded audio files
        // public long Length { get; }
    }
}
