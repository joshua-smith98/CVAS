using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace CVAS.AudioEngine.NAudio
{
    /// <summary>
    /// NAudio implementation of <see cref="IAudioEngine"/>.
    /// </summary>
    internal class AudioEngine : IAudioEngine<AudioClip>
    {
        public static IAudioEngine Instance
        {
            get
            {
                instance ??= new AudioEngine();
                return instance;
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

        /// <summary>
        /// Plays a single <see cref="AudioClip"/> once. For use when <see cref="Instance"/> hasn't been initialised.
        /// </summary>
        /// <param name="audioClip"></param>
        public static void PlayOnce(AudioClip audioClip)
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
        /// Returns true if the file at the given path is an audio file, false otherwise.
        /// Also returns false if the path is invalid or the file doesn't exist.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsAudioFile(string path)
        {
            try
            {
                // Return true if we can load the audio file
                using (new AudioFileReader(path)) { } ;
                return true;
            }
            catch { return false; } // Otherwise return false
        }

        /// <summary>
        /// Plays the given <see cref="AudioClip"/>, with automatic resampling.
        /// </summary>
        /// <param name="audioClip"></param>
        public void Play(AudioClip audioClip)
        {
            MediaFoundationResampler resampler = new(audioClip.ToWaveProvider(), WaveFormat);
            sampleProvider.AddMixerInput(resampler);
        }

        /// <summary>
        /// Renders an <see cref="AudioClip"/> to a new file at the given path. Assumes the directory exists, and overwrites any existing file.
        /// </summary>
        /// <param name="audioClip"></param>
        /// <param name="path"></param>
        public static void Render(AudioClip audioClip, string path)
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