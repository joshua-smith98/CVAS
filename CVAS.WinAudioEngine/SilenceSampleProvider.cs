using NAudio.Wave;

namespace CVAS.WinAudioEngineNS
{
    /// <summary>
    /// A sample provider that provides a given number of milliseconds of silence. Only playable once.
    /// </summary>
    internal class SilenceSampleProvider : ISampleProvider
    {
        public WaveFormat WaveFormat { get; }
        
        private readonly long numSamples;
        private long sampleCounter = 0;

        public SilenceSampleProvider(WaveFormat waveFormat, int milliseconds)
        {
            WaveFormat = waveFormat;
            numSamples = (long)(milliseconds * (WaveFormat.SampleRate / 1000.0f) * WaveFormat.Channels); // Convert milliseconds into samples
        }

        public int Read(float[] buffer, int offset, int count)
        {
            // Calculate the number of samples remaining
            long samplesRemaining = numSamples - sampleCounter;
            
            // Case: intermediate reads
            if (samplesRemaining >= count)
            {
                for (int i = offset; i < count; i++)
                    buffer[i] = 0.0f;
                sampleCounter += count;
                return count;
            }
            // Case: last read (and any subsequent)
            else // if (samplesRemaining < count)
            {
                for (int i = offset; i < samplesRemaining; i++)
                    buffer[i] = 0.0f;
                sampleCounter += samplesRemaining;
                return (int)samplesRemaining;
            }
        }
    }
}
