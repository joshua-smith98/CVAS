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
        public string path { get; }
        public long offset { get; } // These two properties are for future plans to access WaveStreams embedded somewhere in a file block (will need a refactor))
        public long length { get; }

        public AudioFileStreaming(string path)
        {
            this.path = path;
            this.offset = 0;

            using (var tempAudioFileReader = new AudioFileReader(path)) // Briefly opening file to get some metadata
            {
                this.WaveFormat = tempAudioFileReader.WaveFormat;
                this.length = tempAudioFileReader.Length;
            }
        }

        public IWaveProvider toWaveProvider()
        {
            return new DisposingWaveProvider(new AudioFileReader(path));
        }

        public void Dispose()
        {
            // Nothing to dispose of.
        }
    }
}
