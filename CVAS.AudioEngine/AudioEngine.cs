using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace CVAS.AudioEngine
{
    /// <summary>
    /// Governs all audio mixing, rendering and playback. Non-constructable - use the static <see cref="AudioEngine.Instance"/> to access.
    /// </summary>
    public class AudioEngine : IDisposable
    {
        public static AudioEngine Instance { get; } = new AudioEngine();
        
        public WaveFormat WaveFormat => sampleProvider.WaveFormat; // Not sure if I'll need this but good to have it anyway

        private readonly WaveOutEvent waveOutEvent;

        private readonly MixingSampleProvider sampleProvider;


        private AudioEngine()
        {
            // Initialise properties
            sampleProvider = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2)); // TODO: Implement changing WaveFormat
            sampleProvider.ReadFully = true;
            waveOutEvent = new WaveOutEvent();
            waveOutEvent.Init(sampleProvider);
            waveOutEvent.Play();
        }

        /// <summary>
        /// Plays the given <see cref="IAudioClip"/>, with automatic resampling.
        /// </summary>
        /// <param name="audioClip"></param>
        public void Play(IAudioClip audioClip)
        {
            MediaFoundationResampler resampler = new MediaFoundationResampler(audioClip.ToWaveProvider(), WaveFormat);
            sampleProvider.AddMixerInput(resampler);
        }

        /// <summary>
        /// Renders an <see cref="IAudioClip"/> to a new file at the given path. Assumes the directory exists, and overwrites any existing file.
        /// </summary>
        /// <param name="audioClip"></param>
        /// <param name="path"></param>
        public void Render(IAudioClip audioClip, string path)
        {
            // Write wave file
            WaveFileWriter.CreateWaveFile16(path, audioClip.ToWaveProvider().ToSampleProvider());
        }

        public void Dispose()
        {
            waveOutEvent.Stop();
            waveOutEvent.Dispose();
        }
    }
}