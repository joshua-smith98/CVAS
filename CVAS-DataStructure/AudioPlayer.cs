using NAudio.Wave;

namespace CVAS.DataStructure
{
    /// <summary>
    /// Global object for playing <see cref="AudioFile"/>s and <see cref="Playlist"/>s. Access with <see cref="AudioPlayer.instance"/>.
    /// </summary>
    public class AudioPlayer : IDisposable
    {
        public static AudioPlayer instance = new AudioPlayer();
        public event EventHandler<StoppedEventArgs>? PlaybackStopped;
        public WaveFormat waveFormat => _outputDevice.OutputWaveFormat;

        private WaveOutEvent _outputDevice = new(); // Not implementing ability to change devices or outputs yet

        private AudioPlayer()
        {

        }

        /// <summary>
        /// Stops any current playback and plays the given <see cref="ISampleProvider"/>.
        /// </summary>
        /// <param name="sampleProvider"></param>
        public void Play(IAudioClip audioClip)
        {
            // Stop current outputDevice
            if (_outputDevice.PlaybackState is PlaybackState.Playing)
                _outputDevice.Stop();

            // Dispose of old outputDevice
            _outputDevice?.Dispose();

            // Construct new outputDevice and subscribe to events (incl. resetting all audioclips when stopped)
            _outputDevice = new();
            _outputDevice.PlaybackStopped += (object? sender, StoppedEventArgs e) =>
            {
                audioClip.Reset();
                PlaybackStopped?.Invoke(sender, e);
            };

            // Initialise new outputDevice and play
            _outputDevice.Init(audioClip.sampleProvider);
            _outputDevice.Play();
        }

        public void Stop()
        {
            _outputDevice.Stop();
        }

        public void Dispose()
        {
            _outputDevice.Dispose();
        }
    }
}
