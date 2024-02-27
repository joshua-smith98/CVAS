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
        public string Path { get; }
        public long Offset { get; } // See: AudioFileStreaming
        public long Length { get; }

        private readonly MemoryStream masterCache;

        public AudioFileCached(string path)
        {
            // Initialise properties
            Path = path;
            Offset = 0;

            // Gather metadata & copy wave data
            using (var tempAudioFileReader = new AudioFileReader(path))
            {
                WaveFormat = tempAudioFileReader.WaveFormat;
                Length = tempAudioFileReader.Length;

                masterCache = new MemoryStream();
                masterCache.SetLength(tempAudioFileReader.Length);
                tempAudioFileReader.CopyTo(masterCache);
                masterCache.Flush();
            }
        }

        public IWaveProvider ToWaveProvider()
        {
            // Copy audio data from master cache to new cache
            var newStream = new MemoryStream();
            newStream.SetLength(masterCache.Length);
            masterCache.Position = 0;
            masterCache.CopyTo(newStream);
            newStream.Flush();
            newStream.Position = 0;

            return new DisposingWaveProvider(new RawSourceWaveStream(newStream, WaveFormat));
        }

        public void Dispose()
        {
            masterCache.Dispose();
        }
    }
}
