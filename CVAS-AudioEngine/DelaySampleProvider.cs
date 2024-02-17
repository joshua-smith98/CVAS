using NAudio.Wave;

namespace CVAS.AudioEngine
{
    /// <summary>
    /// A sample provider that provides a given number of milliseconds of silence.
    /// </summary>
    internal class DelaySampleProvider : ISampleProvider
    {
        public WaveFormat WaveFormat { get; }
        
        private long _numSamples;
        private long _sampleCounter = 0;

        public DelaySampleProvider(WaveFormat waveFormat, int milliseconds)
        {
            WaveFormat = waveFormat;
            _numSamples = (long)(milliseconds * (WaveFormat.SampleRate / 1000.0f) * WaveFormat.Channels); // Convert milliseconds into samples
        }

        public int Read(float[] buffer, int offset, int count)
        {
            long samplesRemaining = _numSamples - _sampleCounter;
            
            if (samplesRemaining >= count)
            {
                for (int i = offset; i < count; i++)
                    buffer[i] = 0.0f;
                _sampleCounter += count;
                return count;
            }
            else // if (samplesRemaining < count)
            {
                for (int i = offset; i < samplesRemaining; i++)
                    buffer[i] = 0.0f;
                _sampleCounter += samplesRemaining;
                return (int)samplesRemaining;
            }
        }
    }
}
