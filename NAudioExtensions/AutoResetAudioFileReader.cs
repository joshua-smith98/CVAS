using NAudio.Wave;

namespace NAudioExtensions
{
    public class AutoResetAudioFileReader : IWaveProvider, IDisposable
    {
        private AudioFileReader _audioFileReader;
        private bool EOF = false; // Related to janky code in Read()
        
        public WaveFormat WaveFormat { get; }

        public AutoResetAudioFileReader(string path)
        {
            _audioFileReader = new AudioFileReader(path);
            WaveFormat = _audioFileReader.WaveFormat;
        }

        public void Dispose()
        {
            _audioFileReader.Dispose();
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            int bytesRead = _audioFileReader.Read(buffer, offset, count);

            // The reason this code is so janky is to ensure that ConcatenatingSampleProvider actually proceeds to the next ISampleProvider.
            // If you only return 0 once, for some reason it just keeps repeating the same file over and over again.
            // So I have to return 0 twice, which means YAY RANDOM BOOL TIME...
            // And all this because ConcatenatingSampleProvider can't play something twice.
            // NAudio pls fix

            if (bytesRead == 0)
            {
                if (EOF)
                {
                    _audioFileReader.Position = 0;
                    EOF = false;
                }
                else
                {
                    EOF = true;
                }
            }

            return bytesRead;
        }
    }
}
