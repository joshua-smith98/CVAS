namespace CVAS.AudioEngine.NAudio
{
    /// <summary>
    /// NAudio implementation of <see cref="IAudioFile"/>.
    /// </summary>
    internal abstract class AudioFile : AudioClip, IAudioFile
    {
        public abstract string Path { get; }
    }
}
