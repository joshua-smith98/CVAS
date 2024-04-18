namespace CVAS.AudioEngine.BASS
{
    /// <summary>
    /// BASS implementation of <see cref="IAudioClip"/>.
    /// </summary>
    internal abstract class AudioClip : IAudioClip
    {
        internal abstract int GetStreamHandle();
    }
}
