using NAudio.Wave;

namespace CVAS.DataStructure
{
    public interface IAudioClip
    {
        public WaveFormat waveFormat { get; }

        public ISampleProvider sampleProvider { get; }

        internal void Reset();
    }
}
