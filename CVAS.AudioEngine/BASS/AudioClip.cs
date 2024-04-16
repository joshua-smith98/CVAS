namespace CVAS.AudioEngine.BASS
{
    /// <summary>
    /// Abstract class representing any playable piece of audio.
    /// </summary>
    internal abstract class AudioClip : IAudioClip
    {
        internal abstract int GetStreamHandle();
    }
}
