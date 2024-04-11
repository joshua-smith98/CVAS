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

        internal int Freq;

        internal int BitsPerSample;

        private AudioEngine()
        {
            // Initialise BASS and properties
        }

        public static void Init()
        {
            // Create Instance
        }

        public static void PlayOnce(AudioClip audioClip)
        {
            // Initialise BASS
            // Play audioClip stream
            // Hang until stream stops
            // Free audioClip stream
            // Free BASS
        }

        public static bool IsAudioFile(string path)
        {
            // Attempt to load audio file from path
            // Return true on success, false on failure
        }

        internal static void FreeUponStop(int streamHandle)
        {
            // Wait until the given stream has stopped playing, and then free it
        }

        public void Play(AudioClip audioClip)
        {
            // Play audioClip stream
        }

        public void Render(AudioClip audioClip, string path)
        {
            // Render the audioClip stream to the file at the given path
        }

        public void StopAll()
        {
            // Call Bass.BASS_Pause()
        }

        public void Dispose()
        {
            // Calls Bass.BASS_Pause()
            // Frees Bass
        }
    }
}
