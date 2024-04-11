using NAudio.Wave;

namespace CVAS.WinAudioEngineNS
{
    /// <summary>
    /// A playable piece of silent audio, with a given length in milliseconds.
    /// </summary>
    public class Silence(int milliseconds) : AudioClip
    {
        public int Milliseconds => milliseconds;

        internal override IWaveProvider ToWaveProvider()
        {
            return new SilenceSampleProvider(AudioEngine.WaveFormat, Milliseconds).ToWaveProvider();
        }
    }
}
