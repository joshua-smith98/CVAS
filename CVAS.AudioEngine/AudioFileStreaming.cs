using NAudio.Wave;

namespace CVAS.AudioEngine
{
    /// <summary>
    /// A playable piece of audio originating from an audio file.
    /// </summary>
    public class AudioFileStreaming : IAudioClip
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
            using (var tempAudioFileReader = new AudioFileReader(path)) // Briefly opening file to get the WaveFormat
                this.WaveFormat = tempAudioFileReader.WaveFormat;
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
