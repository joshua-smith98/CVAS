using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace CVAS.DataStructure
{
    public class Playlist : IAudioClip, IDisposable
    {
        public AudioFile[] audioFiles { get; }

        public WaveFormat waveFormat { get; } = new();
        internal ConcatenatingSampleProvider sampleProvider { get; }
        private MediaFoundationResampler[] _resamplers { get; }

        public Playlist(params IAudioClip[] audioClips)
        {
            // Init audioFiles & waveFormat
            this.audioFiles = audioFiles;
            this.waveFormat = waveFormat;

            // Initialise and create _resamplers
            this._resamplers = new MediaFoundationResampler[this.audioFiles.Length];

            for (int i = 0; i < audioFiles.Length; i++)
                _resamplers[i] = new MediaFoundationResampler(this.audioFiles[i].audioFileReader, this.waveFormat);

            // Init sampleProvider
            sampleProvider = new ConcatenatingSampleProvider(_resamplers.Select(x => x.ToSampleProvider()));
        }

        public void Dispose()
        {
            foreach (AudioFile audioFile in audioFiles) 
                audioFile.Dispose();
        }
    }
}
