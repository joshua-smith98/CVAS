using NAudio.Wave;

namespace CVAS.AudioEngine
{
    public class AudioFile : IAudioClip
    {
        public WaveFormat WaveFormat { get; }

        public string path { get; }
        public long offset { get; } // These two properties are for future plans to access WaveStreams embedded somewhere in a file block (will need a refactor))
        public long length { get; }

        public AudioFile(string path)
        {
            this.path = path;
            using (var tempAudioFileReader = new AudioFileReader(path))
                this.WaveFormat = tempAudioFileReader.WaveFormat;
        }

        public IWaveProvider toWaveProvider()
        {
            return new DisposingWaveProvider(new AudioFileReader(path));
        }
    }
}
