using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace CVAS.AudioEngine
{
    public class Playlist : IAudioClip
    {
        public WaveFormat WaveFormat { get; }

        public AudioFile[] audioFiles { get; }

        public Playlist(WaveFormat waveFormat, params AudioFile[] audioFiles)
        {
            WaveFormat = waveFormat;
            this.audioFiles = audioFiles;
        }

        public IWaveProvider toWaveProvider()
        {
            ConcatenatingSampleProvider sampleProvider;
            IWaveProvider[] waveProviders = new IWaveProvider[audioFiles.Length];

            for (int i = 0; i < audioFiles.Length; i++)
            {
                // Open audio file & convert to DisposingWaveProvider
                IWaveProvider waveProvider = new DisposingWaveProvider(new AudioFileReader(audioFiles[i].path));

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
