using NAudio.Wave;

namespace CVAS.AudioEngine
{
    /// <summary>
    /// A playable piece of audio from a file, that has been cached in memory.
    /// </summary>
    public class AudioFileCached : IAudioFile
    {
        public WaveFormat WaveFormat { get; }

        /// <summary>
        /// Path to the originating file.
        /// </summary>
        public string path { get; }
        public long offset { get; } // See: AudioFileStreaming
        public long length { get; }

        private MemoryStream _masterCache;

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

                _masterCache = new MemoryStream();
                _masterCache.SetLength(tempAudioFileReader.Length);
                tempAudioFileReader.CopyTo(_masterCache);
                _masterCache.Flush();
            }
        }

        public IWaveProvider toWaveProvider()
        {
            // Copy audio data from master cache to new cache
            var newStream = new MemoryStream();
            newStream.SetLength(_masterCache.Length);
            _masterCache.Position = 0;
            _masterCache.CopyTo(newStream);
            newStream.Flush();
            newStream.Position = 0;

            return new DisposingWaveProvider(new RawSourceWaveStream(newStream, WaveFormat));
        }

        public void Dispose()
        {
            _masterCache.Dispose();
        }
    }
}
