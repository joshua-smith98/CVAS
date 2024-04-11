using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace CVAS.AudioEngineNS
{
    /// <summary>
    /// Governs all audio mixing, rendering and playback. Non-constructable - use the static <see cref="AudioEngine.Instance"/> to access.
    /// </summary>
    public class AudioEngine : IDisposable
    {
        public static AudioEngine Instance
        {
            get
            {
                if (instance is null) throw new NullReferenceException();
                else return instance;
            }
        }
        private static AudioEngine? instance;
        
        internal static readonly WaveFormat WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2); // TODO: Implement changing WaveFormat

        private WaveOutEvent waveOutEvent;

        private readonly MixingSampleProvider sampleProvider;

        private AudioEngine()
        {
            // Initialise properties
            sampleProvider = new MixingSampleProvider(WaveFormat);
            sampleProvider.ReadFully = true;
            waveOutEvent = new WaveOutEvent();
            waveOutEvent.Init(sampleProvider);
            waveOutEvent.Play();
        }

        public static void Init()
        {
            // Check if already initialised
            if (instance is not null) throw new Exception("AudioEngine cannot be initialised twice!");
            instance = new AudioEngine();
        }

        public static void PlayOnce(IAudioClip audioClip)
        {
            // Initialise new WaveOutEvent and play
            WaveOutEvent waveOutEvent = new();
            waveOutEvent.Init(audioClip.ToWaveProvider());
            waveOutEvent.Play();

            // Hang until playback is complete
            while (waveOutEvent.PlaybackState is PlaybackState.Playing);

            waveOutEvent.Dispose();
        }

        /// <summary>
        /// Plays the given <see cref="IAudioClip"/>, with automatic resampling.
        /// </summary>
        /// <param name="audioClip"></param>
        public void Play(IAudioClip audioClip)
        {
            MediaFoundationResampler resampler = new(audioClip.ToWaveProvider(), WaveFormat);
            sampleProvider.AddMixerInput(resampler);
        }

        /// <summary>
        /// Renders an <see cref="IAudioClip"/> to a new file at the given path. Assumes the directory exists, and overwrites any existing file.
        /// </summary>
        /// <param name="audioClip"></param>
        /// <param name="path"></param>
        public static void Render(IAudioClip audioClip, string path)
        {
            // Write wave file
            WaveFileWriter.CreateWaveFile16(path, audioClip.ToWaveProvider().ToSampleProvider());
        }

        /// <summary>
        /// Stops all audio playback.
        /// </summary>
        public void StopAll()
        {
            sampleProvider.RemoveAllMixerInputs();
        }

        public void Dispose()
        {
            waveOutEvent?.Stop();
            waveOutEvent?.Dispose();
            waveOutEvent = null!;
            GC.SuppressFinalize(this);
        }
    }
}