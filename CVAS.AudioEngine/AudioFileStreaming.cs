using NAudio.Wave;

namespace CVAS.AudioEngineNS
{
    /// <summary>
    /// A playable piece of audio from a file, that is streamed directly from the disk.
    /// </summary>
    public class AudioFileStreaming(string path) : IAudioFile
    {
        /// <summary>
        /// Path to the originating file.
        /// </summary>
        public string Path => path; // We're assuming that the the file at 'path' is an audio file

        internal IWaveProvider ToWaveProvider()
        {
            return new DisposingWaveProvider(new AudioFileReader(Path));
        }

        public void Dispose()
        {
            // Nothing to dispose of
            GC.SuppressFinalize(this);
        }
    }
}
