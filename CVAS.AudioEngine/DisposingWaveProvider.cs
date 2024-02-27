using NAudio.Wave;

namespace CVAS.AudioEngine
{
    /// <summary>
    /// A WaveProvider that takes a WaveStream, and automatically disposes of it when finished playing.
    /// </summary>
    internal class DisposingWaveProvider : IWaveProvider
    {
        public WaveFormat WaveFormat => waveStream.WaveFormat;

        private readonly WaveStream waveStream;
        private bool disposed = false;

        public DisposingWaveProvider(WaveStream waveStream)
        {
            this.waveStream = waveStream;
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            if (disposed) return 0; // In case this is called after EOF
            
            var bytesRead = waveStream.Read(buffer, offset, count);
            
            // Case: EOF
            if (bytesRead == 0)
            {
                waveStream.Dispose();
                disposed = true;
                GC.Collect(); // If we don't do this, the GC will allow gigabytes of memory usage to build up over time. This block won't be accessed too often, so it shouldn't affect performance.
            }

            return bytesRead;
        }
    }
}
