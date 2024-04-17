using NAudio.Wave;

namespace CVAS.AudioEngine.NAudio
{
    /// <summary>
    /// NAudio implementation of <see cref="IAudioClip"/>.
    /// </summary>
    internal abstract class AudioClip : IAudioClip
    {
        internal abstract IWaveProvider ToWaveProvider();
    }
}
