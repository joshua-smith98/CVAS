using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace CVAS.AudioEngine
{
    /// <summary>
    /// A playable piece of silent audio, with a given length in milliseconds.
    /// </summary>
    public class Delay : IAudioClip
    {
        public WaveFormat WaveFormat => AudioPlayer.instance.WaveFormat; // Just automatically set this to the global WaveFormat

        public int milliseconds { get; }

        public Delay(int milliseconds)
        {
            this.milliseconds = milliseconds;
        }

        public IWaveProvider toWaveProvider()
        {
            return new DelaySampleProvider(AudioPlayer.instance.WaveFormat, milliseconds).ToWaveProvider();
        }
    }
}
