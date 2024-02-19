using NAudio.Wave;

namespace CVAS.AudioEngine
{
    public class AudioFileCached : IAudioFile
    {
        public WaveFormat WaveFormat { get; }

        public string path { get; }
        public long offset { get; }
        public long length { get; }

        private RawSourceWaveStream _masterCache;

        public AudioFileCached(string path)
        {
            // Initialise properties
            this.path = path;
            offset = 0;

            // Gather metadata & copy wave data
            using (var tempAudioFileReader = new AudioFileReader(path))
            {
                WaveFormat = tempAudioFileReader.WaveFormat;
                length = tempAudioFileReader.Length;

                _masterCache = new RawSourceWaveStream(tempAudioFileReader, WaveFormat);
            }
        }

        public IWaveProvider toWaveProvider()
        {
            var newStream = new DisposingWaveProvider(new RawSourceWaveStream(_masterCache, WaveFormat));
            
            _masterCache.Position = 0; // Just in case...

            return newStream;
        }

        public void Dispose()
        {
            _masterCache.Dispose();
        }
    }
}
