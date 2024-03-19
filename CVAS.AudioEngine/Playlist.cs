using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace CVAS.AudioEngine
{
    /// <summary>
    /// An piece of playable audio made up of concatenated <see cref="IAudioClip"/>s.
    /// </summary>
    public class Playlist : IAudioClip
    {
        public IAudioClip[] AudioClips { get; }

        public Playlist(params IAudioClip[] audioClips)
        {
            AudioClips = audioClips;
        }

        public IWaveProvider ToWaveProvider()
        {
            ConcatenatingSampleProvider sampleProvider;
            IWaveProvider[] waveProviders = new IWaveProvider[AudioClips.Length]; // Collection of WaveProviders to concatenate

            for (int i = 0; i < AudioClips.Length; i++)
            {
                // Initialise waveProvider
                IWaveProvider waveProvider = AudioClips[i].ToWaveProvider();

                // Check for resampling
                if (!waveProvider.WaveFormat.Equals(AudioEngine.Instance.WaveFormat))
                {
                    waveProvider = new MediaFoundationResampler(waveProvider, AudioEngine.Instance.WaveFormat);
                }

                // Assign to waveProviders
                waveProviders[i] = waveProvider;
            }

            // Initialise ConcatenatingSampleProvider & return
            sampleProvider = new ConcatenatingSampleProvider(waveProviders.Select(x => x.ToSampleProvider()));
            return sampleProvider.ToWaveProvider();
        }

        public void Dispose()
        {
            foreach (IAudioClip audioClip in AudioClips)
            {
                audioClip.Dispose();
            }
        }
    }
}
