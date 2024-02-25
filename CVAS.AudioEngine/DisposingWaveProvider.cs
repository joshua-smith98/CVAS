using NAudio.Wave;

namespace CVAS.AudioEngine
{
    /// <summary>
    /// A WaveProvider that takes a WaveStream, and automatically disposes of it when finished playing.
    /// </summary>
    internal class DisposingWaveProvider : IWaveProvider
    {
        public WaveFormat WaveFormat => _waveStream.WaveFormat;

        private WaveStream _waveStream;
        private bool _disposed = false;

        public DisposingWaveProvider(WaveStream waveStream)
        {
            _waveStream = waveStream;
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            if (_disposed) return 0; // In case this is called after EOF
            
            var bytesRead = _waveStream.Read(buffer, offset, count);
            
            // Case: EOF
            if (bytesRead == 0)
            {
                _waveStream.Dispose();
                _disposed = true;
                GC.Collect(); // If we don't do this, the GC will allow gigabytes of memory usage to build up over time. This block won't be accessed often, so it should be fine.
            }

            return bytesRead;
        }
    }
}
