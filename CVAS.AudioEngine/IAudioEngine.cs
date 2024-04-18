using System.Runtime.InteropServices;

namespace CVAS.AudioEngine
{
    /// <summary>
    /// Specifies the structure of an <see cref="IAudioEngine"/> implementation, where <see cref="TAudioClip"/> is the local implementation of <see cref="IAudioClip"/>.
    /// </summary>
    /// <typeparam name="TAudioClip"></typeparam>
    internal interface IAudioEngine<TAudioClip> : IAudioEngine where TAudioClip : IAudioClip
    {
        internal static abstract new IAudioEngine Instance { get; }
        internal static abstract new bool IsInitialised { get; }

        internal static abstract new void Init();
        internal static abstract new bool IsAudioFile(string path);
        internal static abstract void PlayOnce(TAudioClip audioClip);
        internal static abstract void Render(TAudioClip audioClip, string path);

        public void Play(TAudioClip audioClip);
        public new void StopAll();
    }
    
    /// <summary>
    /// Handles all audio playback, mixing and rendering. Non-constructable - to use, call <see cref="Init()"/> and then access with <see cref="Instance"/>.
    /// <br/>Automatically selects the implementation required for each OS (NAudio for Windows, BASS for all others).
    /// </summary>
    public interface IAudioEngine : IDisposable
    {

        /// <summary>
        /// Gets the currently active <see cref="IAudioEngine"/>.
        /// Call <see cref="Init()"/> before accessing. Throws <see cref="NullReferenceException"/> if <see cref="IAudioEngine"/> has not been initialised.
        /// </summary>
        /// <exception cref="NullReferenceException"/>
        public static IAudioEngine Instance
            => RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? NAudio.AudioEngine.Instance
            : BASS.AudioEngine.Instance;

        /// <summary>
        /// Gets <see cref="true"/> if the audio engine has been initialised, otherwise <see cref="false"/>.
        /// </summary>
        public static bool IsInitialised
            => RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? NAudio.AudioEngine.IsInitialised
            : BASS.AudioEngine.IsInitialised;

        /// <summary>
        /// Initialises <see cref="Instance"/>. Throws an <see cref="AudioEngineException"/> if called more than once.
        /// </summary>
        /// <exception cref="AudioEngineException"/>
        public static void Init()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                NAudio.AudioEngine.Init();
            else
                BASS.AudioEngine.Init();
        }

        /// <summary>
        /// Plays the given <see cref="IAudioClip"/> once, For use when <see cref="Instance"/> has not been initialised.
        /// </summary>
        /// <param name="audioClip"></param>
        public static void PlayOnce(IAudioClip audioClip)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                NAudio.AudioEngine.PlayOnce((NAudio.AudioClip)audioClip);
            else
                BASS.AudioEngine.PlayOnce((BASS.AudioClip)audioClip);
        }

        /// <summary>
        /// Renders an <see cref="IAudioClip"/> to a new file at the given path. Assumes the directory exists, and overwrites any existing file.
        /// </summary>
        /// <param name="audioClip"></param>
        /// <param name="path"></param>
        /// <exception cref="AudioEngineException"/>
        public static void Render(IAudioClip audioClip, string path)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                NAudio.AudioEngine.Render((NAudio.AudioClip)audioClip, path);
            else
                BASS.AudioEngine.Render((BASS.AudioClip)audioClip, path);
        }

        /// <summary>
        /// Returns true if the file at the given path is a valid audio file, false otherwise.
        /// Also returns false if the path is invalid or the file doesn't exist.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsAudioFile(string path)
            => RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? NAudio.AudioEngine.IsAudioFile(path)
            : BASS.AudioEngine.IsAudioFile(path);

        /// <summary>
        /// Plays the given <see cref="IAudioClip"/>.
        /// </summary>
        /// <param name="audioClip"></param>
        public void Play(IAudioClip audioClip)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                ((IAudioEngine<NAudio.AudioClip>)this).Play((NAudio.AudioClip)audioClip);
            else
                ((IAudioEngine<BASS.AudioClip>)this).Play((BASS.AudioClip)audioClip);
        }

        /// <summary>
        /// Clears all playback.
        /// </summary>
        public void StopAll()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                ((IAudioEngine<NAudio.AudioClip>)this).StopAll();
            else
                ((IAudioEngine<BASS.AudioClip>)this).StopAll();
        }
    }
}
