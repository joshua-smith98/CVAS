using NAudio.Wave;

namespace CVAS.WinAudioEngineNS
{
    /// <summary>
    /// Abstract class representing any playable piece of audio.
    /// </summary>
    public abstract class AudioClip
    {
        internal abstract IWaveProvider ToWaveProvider();
    }
}
