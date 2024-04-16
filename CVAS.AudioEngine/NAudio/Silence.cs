using NAudio.Wave;

namespace CVAS.AudioEngine.NAudio
{
    /// <summary>
    /// A playable piece of silent audio, with a given length in milliseconds.
    /// </summary>
    internal class Silence(int milliseconds) : AudioClip, ISilence
    {
        public int Milliseconds => milliseconds;

        internal override IWaveProvider ToWaveProvider()
        {
            return new SilenceSampleProvider(AudioEngine.WaveFormat, Milliseconds).ToWaveProvider();
        }
    }
}
