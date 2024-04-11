using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace CVAS.WinAudioEngineNS
{
    /// <summary>
    /// An piece of playable audio made up of concatenated <see cref="AudioClip"/>s.
    /// </summary>
    public class Playlist(params AudioClip[] audioClips) : AudioClip
    {
        public AudioClip[] AudioClips => audioClips;

        internal override IWaveProvider ToWaveProvider()
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

        public override void Dispose()
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
