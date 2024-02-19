using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace CVAS.AudioEngine
{
    /// <summary>
    /// An piece of playable audio made up of concatenated <see cref="IAudioClip"/>s.
    /// </summary>
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
            IWaveProvider[] waveProviders = new IWaveProvider[audioClips.Length]; // Collection of WaveProviders to concatenate

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

            // Initialise ConcatenatingSampleProvider & return
            sampleProvider = new ConcatenatingSampleProvider(waveProviders.Select(x => x.ToSampleProvider()));
            return sampleProvider.ToWaveProvider();
        }

        public void Dispose()
        {
            foreach (IAudioClip audioClip in audioClips)
            {
                audioClip.Dispose();
            }
        }
    }
}
