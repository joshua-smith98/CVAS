using NAudio.Wave;

namespace CVAS.AudioEngineNS
{
    /// <summary>
    /// A playable piece of audio from a file, that is streamed directly from the disk.
    /// </summary>
    public class AudioFileStreaming(string path) : AudioFile
    {
        /// <summary>
        /// Path to the originating file.
        /// </summary>
        public override string Path => path; // We're assuming that the the file at 'path' is an audio file

        internal override IWaveProvider ToWaveProvider()
        {
            return new DisposingWaveProvider(new AudioFileReader(Path));
        }
    }
}
