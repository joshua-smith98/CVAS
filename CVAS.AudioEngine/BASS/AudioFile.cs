namespace CVAS.AudioEngine.BASS
{
    /// <summary>
    /// Abstract class representing any playable piece of audio originating from a file.
    /// </summary>
    public abstract class AudioFile : AudioClip
    {
        public abstract string Path { get; }
    }
}
