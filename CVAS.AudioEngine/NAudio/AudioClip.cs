using NAudio.Wave;

namespace CVAS.AudioEngine.NAudio
{
    /// <summary>
    /// Abstract class representing any playable piece of audio.
    /// </summary>
    internal abstract class AudioClip : IAudioClip
    {
        internal abstract IWaveProvider ToWaveProvider();
    }
}
