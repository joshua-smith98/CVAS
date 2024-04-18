using System.IO;
using Un4seen.Bass;

namespace CVAS.AudioEngine.BASS
{
    /// <summary>
    /// BASS implementation of <see cref="ISilence"/>.
    /// </summary>
    internal class Silence(int milliseconds) : AudioClip, ISilence
    {
        public int Milliseconds => milliseconds;

        internal override int GetStreamHandle()
        {
            // Get length in bytes
            var byteLength = (int)(milliseconds / 1000f * 44100 * 2); // 2 is the number of bytes per sample (16bit audio)

            // Create sample
            int sampleHandle = Bass.BASS_SampleCreate(byteLength, 44100, 1, 2, BASSFlag.BASS_DEFAULT); // Apparently '1' is not a valid value for max on linux!
            if (sampleHandle == 0)
            {
                // Handle errors
                var bassError = Bass.BASS_ErrorGetCode();
                switch (bassError)
                {
                    case BASSError.BASS_ERROR_INIT:
                        throw new AudioEngineException("Attempted to open audio file before BASS was initialised!");
                    case BASSError.BASS_ERROR_MEM:
                        throw new AudioEngineException("BASS ran out of memory while generating silence.");
                    default:
                        throw new AudioEngineException($"An unknown error occurred while generating silence. BASS error code: {bassError}");
                }
            }

            // Generate sample bytes
            if (!Bass.BASS_SampleSetData(sampleHandle, new short[byteLength]))
            {
                // Handle unknown error
                var bassError = Bass.BASS_ErrorGetCode();
                throw new AudioEngineException($"An unknown error occurred while generating silence. BASS error code: {bassError}");
            }

            // Get stream handle
            int streamHandle = Bass.BASS_SampleGetChannel(sampleHandle, BASSFlag.BASS_SAMCHAN_STREAM | BASSFlag.BASS_STREAM_DECODE);
            if (streamHandle == 0)
            {
                // No errors should be thrown here if the code is semantically correct, so we'll only use the default "unknown error" message.
                var bassError = Bass.BASS_ErrorGetCode();
                throw new AudioEngineException($"An unknown error occurred while generating silence. BASS error code: {bassError}");
            }

            // Return handle
            return streamHandle;
        }
    }
}
