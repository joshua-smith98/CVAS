using NAudio.Wave;

namespace CVAS.AudioEngine
{
    /// <summary>
    /// A playable piece of silent audio, with a given length in milliseconds.
    /// </summary>
    public class Silence : IAudioClip
    {
        public WaveFormat WaveFormat => AudioPlayer.instance.WaveFormat; // Just automatically set this to the global WaveFormat

        public int milliseconds { get; }

        public Silence(int milliseconds)
        {
            this.milliseconds = milliseconds;
        }

        public IWaveProvider toWaveProvider()
        {
            return new SilenceSampleProvider(AudioPlayer.instance.WaveFormat, milliseconds).ToWaveProvider();
        }

        public void Dispose()
        {
            // Nothing to dispose of
        }
    }
}
