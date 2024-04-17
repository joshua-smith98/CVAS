using NAudio.Wave;

namespace CVAS.AudioEngine.NAudio
{
    /// <summary>
    /// NAudio implementation of <see cref="ISilence"/>.
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
