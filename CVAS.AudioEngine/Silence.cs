using NAudio.Wave;

namespace CVAS.AudioEngineNS
{
    /// <summary>
    /// A playable piece of silent audio, with a given length in milliseconds.
    /// </summary>
    public class Silence(int milliseconds) : IAudioClip
    {
        public int Milliseconds => milliseconds;

        internal IWaveProvider ToWaveProvider()
        {
            return new SilenceSampleProvider(AudioEngine.WaveFormat, Milliseconds).ToWaveProvider();
        }

        public void Dispose()
        {
            // Nothing to dispose of
            GC.SuppressFinalize(this);
        }
    }
}
