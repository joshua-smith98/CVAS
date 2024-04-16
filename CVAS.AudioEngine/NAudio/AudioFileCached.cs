using NAudio.Wave;

namespace CVAS.AudioEngine.NAudio
{
    /// <summary>
    /// A playable piece of audio from a file, that has been cached in memory.
    /// </summary>
    [Obsolete("No longer used in CVAS.AudioEngineWin, and unavailable in CVAS.AudioEngine. Use AudioFileStreaming instead.")]
    public class AudioFileCached : AudioFile, IDisposable // Only AudioClip that inherits from IDisposeable
    {
        internal WaveFormat WaveFormat { get; } // We need this here in order to load the wavestream from memory

        /// <summary>
        /// Path to the originating file.
        /// </summary>
        public override string Path { get; }

        private MemoryStream masterCache;

        public AudioFileCached(string path)
        {
            // Initialise properties
            Path = path;

            // Gather metadata & copy wave data
            using (var tempAudioFileReader = new AudioFileReader(path))
            {
                WaveFormat = tempAudioFileReader.WaveFormat;

                masterCache = new MemoryStream();
                masterCache.SetLength(tempAudioFileReader.Length);
                tempAudioFileReader.CopyTo(masterCache);
                masterCache.Flush();
            }
        }

        internal override IWaveProvider ToWaveProvider()
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
            masterCache?.Dispose();
            masterCache = null!;
            GC.SuppressFinalize(this);
            GC.Collect();
        }
    }
}
