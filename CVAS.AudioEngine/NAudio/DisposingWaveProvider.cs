using NAudio.Wave;

namespace CVAS.AudioEngine.NAudio
{
    /// <summary>
    /// A WaveProvider that takes a WaveStream, and automatically disposes of it when finished playing.
    /// </summary>
    internal class DisposingWaveProvider(WaveStream waveStream) : IWaveProvider
    {
        public WaveFormat WaveFormat => waveStream.WaveFormat;

        private WaveStream waveStream = waveStream;
        private bool disposed = false;

        public int Read(byte[] buffer, int offset, int count)
        {
            if (disposed) return 0; // In case this is called after EOF
            
            var bytesRead = waveStream.Read(buffer, offset, count);
            
            // Case: EOF
            if (bytesRead == 0) Dispose();

            return bytesRead;
        }

        ~DisposingWaveProvider() // In case EOF is never reached for some reason
        {
            Dispose();
        }

        public void Dispose()
        {
            waveStream?.Dispose();
            waveStream = null!;
            disposed = true;
            GC.SuppressFinalize(this);
            GC.Collect(); // If we don't do this, the GC will allow gigabytes of memory usage to build up over time. This block won't be accessed too often, so it shouldn't affect performance.
        }
    }
}
