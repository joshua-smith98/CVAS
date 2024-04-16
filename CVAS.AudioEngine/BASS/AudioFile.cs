namespace CVAS.AudioEngine.BASS
{
    /// <summary>
    /// Abstract class representing any playable piece of audio originating from a file.
    /// </summary>
    internal abstract class AudioFile : AudioClip, IAudioFile
    {
        public abstract string Path { get; }
    }
}
