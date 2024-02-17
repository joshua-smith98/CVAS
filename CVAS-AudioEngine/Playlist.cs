using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace CVAS.AudioEngine
{
    public class Playlist : IAudioClip
    {
        public WaveFormat WaveFormat { get; }

        public IAudioClip[] audioClips { get; }

        public Playlist(params IAudioClip[] audioClips)
        {
            this.audioClips = audioClips;

            WaveFormat = AudioPlayer.instance.WaveFormat;
        }

        public IWaveProvider toWaveProvider()
        {
            ConcatenatingSampleProvider sampleProvider;
            IWaveProvider[] waveProviders = new IWaveProvider[audioClips.Length];

            for (int i = 0; i < audioClips.Length; i++)
            {
                // Initialise waveProvider
                IWaveProvider waveProvider = audioClips[i].toWaveProvider();

                // Check for resampling
                if (!waveProvider.WaveFormat.Equals(WaveFormat))
                {
                    waveProvider = new MediaFoundationResampler(waveProvider, WaveFormat);
                }

                // Assign to waveProviders
                waveProviders[i] = waveProvider;
            }

            // Create ConcatenatingSampleProvider & return
            sampleProvider = new ConcatenatingSampleProvider(waveProviders.Select(x => x.ToSampleProvider()));
            return sampleProvider.ToWaveProvider();
        }
    }
}
