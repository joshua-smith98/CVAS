using NAudio.Wave;
using NAudioExtensions;

namespace CVAS.DataStructure
{
    public class AudioFile : IAudioClip, IDisposable
    {
        public string path { get; }
        public WaveFormat waveFormat => _audioFileReader.WaveFormat;

        private AutoResetAudioFileReader _audioFileReader;
        public ISampleProvider sampleProvider => _audioFileReader.ToSampleProvider();

        public AudioFile(string path)
        {
            this.path = path;
            this._audioFileReader = new AutoResetAudioFileReader(path);
        }

        void IAudioClip.Reset()
        {
            _audioFileReader.Dispose();
            _audioFileReader = new(path);
        }

        public void Dispose()
        {
            _audioFileReader.Dispose();
        }
    }
}
