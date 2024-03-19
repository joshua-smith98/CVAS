using NAudio.Wave;

namespace CVAS.AudioEngine
{
    /// <summary>
    /// A playable piece of audio from a file, that is streamed directly from the disk.
    /// </summary>
    public class AudioFileStreaming : IAudioFile
    {
        /// <summary>
        /// Path to the originating file.
        /// </summary>
        public string Path { get; }

        public AudioFileStreaming(string path)
        {
            Path = path; // We're assuming that the the file at 'path' is an audio file
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
