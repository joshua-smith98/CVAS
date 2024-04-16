using NAudio.Wave;

namespace CVAS.AudioEngine.NAudio
{
    /// <summary>
    /// A playable piece of audio from a file, that is streamed directly from the disk.
    /// </summary>
    internal class AudioFileStreaming(string path) : AudioFile, IAudioFileStreaming
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
