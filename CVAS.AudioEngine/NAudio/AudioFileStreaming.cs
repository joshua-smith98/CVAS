using NAudio.Wave;

namespace CVAS.AudioEngine.NAudio
{
    /// <summary>
    /// NAudio implementation of <see cref="IAudioFileStreaming"/>.
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
