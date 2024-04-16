using NAudio.Wave;

namespace CVAS.AudioEngine.NAudio
{
    /// <summary>
    /// Abstract class representing any playable piece of audio.
    /// </summary>
    public abstract class AudioClip
    {
        internal abstract IWaveProvider ToWaveProvider();
    }
}
