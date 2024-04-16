namespace CVAS.AudioEngine.NAudio
{
    /// <summary>
    /// Abstract class representing any playable piece of audio originating from a file.
    /// </summary>
    internal abstract class AudioFile : AudioClip, IAudioFile
    {
        public abstract string Path { get; }
        // public abstract long Offset { get; } For future use ? accessing embedded audio files
        // public abstract long Length { get; }
    }
}
