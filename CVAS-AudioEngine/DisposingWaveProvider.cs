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
            if (_disposed) return 0;
            
            var bytesRead = _waveStream.Read(buffer, offset, count);
            
            if (bytesRead == 0)
            {
                _waveStream.Dispose();
                _disposed = true;
                return 0;
            }

            return bytesRead;
        }
    }
}
