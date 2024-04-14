using Un4seen.Bass;

namespace CVAS.AudioEngineNS
{
    public class AudioFileStreaming(string path) : AudioFile
    {
        public override string Path => path;

        internal override int GetStreamHandle()
        {
            // Try to open file
            int streamHandle = Bass.BASS_StreamCreateFile(path, 0, 0, BASSFlag.BASS_STREAM_DECODE);
            
            // Handle errors -> maybe expand upon this in future?
            if (streamHandle == 0)
            {
                var bassError = Bass.BASS_ErrorGetCode();
                switch (bassError)
                {
                    case BASSError.BASS_ERROR_INIT:
                        throw new AudioEngineException("Attempted to open audio file before BASS was initialised!");
                    case BASSError.BASS_ERROR_FILEOPEN:
                        throw new FileNotFoundException();
                    case BASSError.BASS_ERROR_NOTAUDIO:
                        throw new AudioEngineException($"Attempted to open file that doesn't contain audio: {path}");
                    case BASSError.BASS_ERROR_MEM:
                        throw new AudioEngineException($"BASS ran out of memory while loading an audio file: {path}");
                    default:
                        throw new AudioEngineException($"An unknown error occurred while opening an audio file. BASS error code: {bassError}");
                }
            }

            // Return handle
            return streamHandle;
        }
    }
}
