namespace CVAS.AudioEngine.BASS
{
    /// <summary>
    /// Abstract class representing any playable piece of audio.
    /// </summary>
    public abstract class AudioClip
    {
        internal abstract int GetStreamHandle();
    }
}
