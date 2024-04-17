using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace CVAS.AudioEngine.NAudio
{
    /// <summary>
    /// NAudio implementation of <see cref="IPlaylist"/>.
    /// </summary>
    internal class Playlist(params AudioClip[] audioClips) : AudioClip, IPlaylist
    {
        public IAudioClip[] AudioClips => audioClips;

        internal override IWaveProvider ToWaveProvider()
        {
            ConcatenatingSampleProvider sampleProvider;
            IWaveProvider[] waveProviders = new IWaveProvider[AudioClips.Length]; // Collection of WaveProviders to concatenate

            for (int i = 0; i < AudioClips.Length; i++)
            {
                // Initialise waveProvider
                IWaveProvider waveProvider = audioClips[i].ToWaveProvider();

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
    }
}
