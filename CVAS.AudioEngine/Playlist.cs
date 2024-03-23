using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace CVAS.AudioEngineNS
{
    /// <summary>
    /// An piece of playable audio made up of concatenated <see cref="IAudioClip"/>s.
    /// </summary>
    public class Playlist(params IAudioClip[] audioClips) : IAudioClip
    {
        public IAudioClip[] AudioClips => audioClips;

        public IWaveProvider ToWaveProvider()
        {
            ConcatenatingSampleProvider sampleProvider;
            IWaveProvider[] waveProviders = new IWaveProvider[AudioClips.Length]; // Collection of WaveProviders to concatenate

            for (int i = 0; i < AudioClips.Length; i++)
            {
                // Initialise waveProvider
                IWaveProvider waveProvider = AudioClips[i].ToWaveProvider();

                // Check for resampling
                if (!waveProvider.WaveFormat.Equals(AudioEngine.WaveFormat))
                {
                    waveProvider = new MediaFoundationResampler(waveProvider, AudioEngine.WaveFormat);
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
            for (int i = 0; i < audioClips.Length; i++)
            {
                audioClips[i]?.Dispose();
                audioClips[i] = null!;
            }

            audioClips = null!;
            GC.SuppressFinalize(this);
        }
    }
}
