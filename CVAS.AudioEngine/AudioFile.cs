namespace CVAS.AudioEngineNS
{
    /// <summary>
    /// Abstract class representing any playable piece of audio originating from a file.
    /// </summary>
    public abstract class AudioFile : AudioClip
    {
        public abstract string Path { get; }
        // public abstract long Offset { get; } For future use ? accessing embedded audio files
        // public abstract long Length { get; }
    }
}
