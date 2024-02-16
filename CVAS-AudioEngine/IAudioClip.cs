using NAudio.Wave;

namespace CVAS.AudioEngine
{
    public interface IAudioClip
    {
        public WaveFormat WaveFormat { get; }

        public IWaveProvider toWaveProvider();
    }
}
