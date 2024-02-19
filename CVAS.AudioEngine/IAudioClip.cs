using NAudio.Wave;

namespace CVAS.AudioEngine
{
    /// <summary>
    /// Interface representing any playable piece of audio.
    /// </summary>
    public interface IAudioClip
    {
        public WaveFormat WaveFormat { get; }

        public IWaveProvider toWaveProvider();
    }
}
