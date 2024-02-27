using NAudio.Wave;

namespace CVAS.AudioEngine
{
    /// <summary>
    /// A playable piece of audio from a file, that is streamed directly from the disk.
    /// </summary>
    public class AudioFileStreaming : IAudioFile
    {
        public WaveFormat WaveFormat { get; }

        /// <summary>
        /// Path to the originating file.
        /// </summary>
        public string Path { get; }
        public long Offset { get; } // These two properties are for future plans to access WaveStreams embedded somewhere in a file block (will need a refactor))
        public long Length { get; }

        public AudioFileStreaming(string path)
        {
            Path = path;
            Offset = 0;

            using (var tempAudioFileReader = new AudioFileReader(path)) // Briefly opening file to get some metadata
            {
                WaveFormat = tempAudioFileReader.WaveFormat;
                Length = tempAudioFileReader.Length;
            }
        }

        public IWaveProvider ToWaveProvider()
        {
            return new DisposingWaveProvider(new AudioFileReader(Path));
        }

        public void Dispose()
        {
            // Nothing to dispose of.
        }
    }
}
