using NAudio.Wave;

namespace CVAS.AudioEngine
{
    /// <summary>
    /// A playable piece of silent audio, with a given length in milliseconds.
    /// </summary>
    public class Silence : IAudioClip
    {
        public WaveFormat WaveFormat => AudioPlayer.Instance.WaveFormat; // Just automatically set this to the global WaveFormat

        public int Milliseconds { get; }

        public Silence(int milliseconds)
        {
            Milliseconds = milliseconds;
        }

        public IWaveProvider ToWaveProvider()
        {
            return new SilenceSampleProvider(AudioPlayer.Instance.WaveFormat, Milliseconds).ToWaveProvider();
        }

        public void Dispose()
        {
            // Nothing to dispose of
        }
    }
}
