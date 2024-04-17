namespace CVAS.AudioEngine
{
    /// <summary>
    /// Represents a piece of playable audio originating from an audio file.
    /// </summary>
    public interface IAudioFile : IAudioClip
    {
        /// <summary>
        /// Gets the path to the source audio file.
        /// </summary>
        public abstract string Path { get; }
    }
}
