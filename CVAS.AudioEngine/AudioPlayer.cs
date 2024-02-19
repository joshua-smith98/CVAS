using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace CVAS.AudioEngine
{
    /// <summary>
    /// Governs all audio mixing and playback. Non-constructable - use the static <see cref="AudioPlayer.instance"/> to access.
    /// </summary>
    public class AudioPlayer : IDisposable
    {
        public static AudioPlayer instance { get; } = new AudioPlayer();
        
        public WaveFormat WaveFormat => _sampleProvider.WaveFormat; // Not sure if I'll need this but good to have it anyway

        private WaveOutEvent _waveOutEvent;

        private MixingSampleProvider _sampleProvider;


        private AudioPlayer()
        {
            // Initialise properties
            _sampleProvider = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2)); // TODO: Implement changing WaveFormat
            _sampleProvider.ReadFully = true;
            _waveOutEvent = new WaveOutEvent();
            _waveOutEvent.Init(_sampleProvider);
            _waveOutEvent.Play();
        }

        /// <summary>
        /// Plays the given <see cref="IAudioClip"/>, with automatic resampling.
        /// </summary>
        /// <param name="audioClip"></param>
        public void Play(IAudioClip audioClip)
        {
            MediaFoundationResampler resampler = new MediaFoundationResampler(audioClip.toWaveProvider(), WaveFormat);
            _sampleProvider.AddMixerInput(resampler);
        }

        public void Dispose()
        {
            _waveOutEvent.Stop();
            _waveOutEvent.Dispose();
        }
    }
}