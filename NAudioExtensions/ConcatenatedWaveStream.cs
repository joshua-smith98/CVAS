using NAudio.Wave;

namespace NAudioExtensions
{
    public class ConcatenatedWaveStream : WaveStream
    {
        public override WaveFormat WaveFormat { get; }

        public override long Length => throw new NotImplementedException();

        public override long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public WaveStream[] subStreams { get; }

        public ConcatenatedWaveStream(params WaveStream[] subStreams)
        {
            
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}
