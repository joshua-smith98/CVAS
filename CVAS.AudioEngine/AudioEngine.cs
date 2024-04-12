using Un4seen.Bass;
using Un4seen.Bass.Misc;

namespace CVAS.AudioEngineNS
{
    public class AudioEngine : IDisposable
    {
        public static AudioEngine Instance
        {
            get
            {
                if (instance is null) throw new NullReferenceException();
                else return instance;
            }
        }
        private static AudioEngine? instance;

        private AudioEngine()
        {
            // Try to initialise BASS
            if (!Bass.BASS_Init(-1, 44100, 0, IntPtr.Zero))
            {
                var bassError = Bass.BASS_ErrorGetCode();
                switch (bassError)
                {
                    case BASSError.BASS_ERROR_DX:
                        throw new AudioEngineException("Failed to initialise BASS audio engine: DirectX is not installed.");
                    case BASSError.BASS_ERROR_ALREADY:
                        throw new AudioEngineException("Tried to initialise BASS twice!");
                    case BASSError.BASS_ERROR_MEM:
                        throw new AudioEngineException("BASS ran out of memory while initialising.");
                    default:
                        throw new AudioEngineException($"Failed to initialise BASS audio engine because of an unknown error. BASS error code: {bassError}");
                }
            }

            // TODO: Assign Freq and Chans depending on the current device's Freq and Chans

        }

        public static void Init()
        {
            instance = new AudioEngine();
        }

        public static void PlayOnce(AudioClip audioClip)
        {
            // Initialise BASS
            if (!Bass.BASS_Init(-1, 44100, 0, IntPtr.Zero))
            {
                // Handle errors
                var bassError = Bass.BASS_ErrorGetCode();
                switch (bassError)
                {
                    case BASSError.BASS_ERROR_DX:
                        throw new AudioEngineException("Failed to initialise BASS audio engine: DirectX is not installed.");
                    case BASSError.BASS_ERROR_ALREADY:
                        throw new AudioEngineException("Tried to initialise BASS twice!");
                    case BASSError.BASS_ERROR_MEM:
                        throw new AudioEngineException("BASS ran out of memory while initialising.");
                    default:
                        throw new AudioEngineException($"Failed to initialise BASS audio engine because of an unknown error. BASS error code: {bassError}");
                }
            }
            // Play audioClip stream
            var audioClipHandle = audioClip.GetStreamHandle();
            Bass.BASS_ChannelPlay(audioClipHandle, false); // Assume this works, all errors seem to be unlikely.

            // Hang until stream stops
            while(Bass.BASS_ChannelIsActive(audioClipHandle) is BASSActive.BASS_ACTIVE_PLAYING)
                Task.Delay(100); // Only check every 100ms

            // We don't need free the stream, as it is automatically done inside GetStreamHandle()

            // Free BASS
            Bass.BASS_Free();
        }

        public static bool IsAudioFile(string path)
        {
            // Check for bass init (we'll need it to load the file)
            var bassActive = Bass.BASS_IsStarted() != 0;
            if (!bassActive) Bass.BASS_Init(-1, 44100, 0, IntPtr.Zero);

            // Attempt to load audio file from path
            int streamHandle = Bass.BASS_StreamCreateFile(path, 0, 0, BASSFlag.BASS_DEFAULT);
            if (streamHandle == 0) return false; // Return false if failed

            // If it is, first free the stream and BASS, then return
            Bass.BASS_StreamFree(streamHandle);
            if (!bassActive) Bass.BASS_Free();

            return true;
        }

        internal static void FreeUponStop(int streamHandle)
        {
            // First check if it has started yet
            // If not, wait until it starts (so we don't free it immediately!)

            if (Bass.BASS_ChannelIsActive(streamHandle) is not BASSActive.BASS_ACTIVE_PLAYING)
            {
                while (Bass.BASS_ChannelIsActive(streamHandle) is not BASSActive.BASS_ACTIVE_PLAYING)
                    Task.Delay(100);
            }

            // Wait until stream is no longer playing
            while (Bass.BASS_ChannelIsActive(streamHandle) is BASSActive.BASS_ACTIVE_PLAYING)
                Task.Delay(100);

            // Free the stream
            if (!Bass.BASS_StreamFree(streamHandle))
            {
                // I just have this here in case there is a problem with the handle (e.g. giving this the wrong handle)
                throw new AudioEngineException($"An unknown error occurred while trying to free a stream: {Bass.BASS_ErrorGetCode()}");
            }
        }

        public void Play(AudioClip audioClip)
        {
            Bass.BASS_ChannelPlay(audioClip.GetStreamHandle(), false);
        }

        public static void Render(AudioClip audioClip, string path)
        {
            var audioClipStream = audioClip.GetStreamHandle();
            
            EncoderWAV encoderWAV = new EncoderWAV(audioClipStream)
            {
                InputFile = null,
                OutputFile = path,
            };
            encoderWAV.Start(null, IntPtr.Zero, false);
            Utils.DecodeAllData(audioClipStream, false);
            while(Bass.BASS_ChannelIsActive(audioClipStream) is BASSActive.BASS_ACTIVE_PLAYING) { } // TODO make this async to avoid hanging GUI in future
            encoderWAV.Stop();
            encoderWAV.Dispose();
        }

        public void StopAll()
        {
            Bass.BASS_Pause();
            Bass.BASS_Start();
        }

        public void Dispose()
        {
            Bass.BASS_Pause();
            Bass.BASS_Free();
        }
    }
}
