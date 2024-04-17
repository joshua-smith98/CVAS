using System.Runtime.InteropServices;

namespace CVAS.AudioEngine
{
    internal interface IAudioEngine<T> : IAudioEngine where T : IAudioClip
    {
        internal static abstract new IAudioEngine Instance { get; }
        internal static abstract new bool IsInitialised { get; }

        internal static abstract new void Init();
        internal static abstract new bool IsAudioFile(string path);
        internal static abstract void PlayOnce(T audioClip);
        internal static abstract void Render(T audioClip, string path);

        public void Play(T audioClip);
        public new void StopAll();
    }
    
    public interface IAudioEngine : IDisposable
    {

        public static IAudioEngine Instance
            => RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? NAudio.AudioEngine.Instance
            : BASS.AudioEngine.Instance;

        public static bool IsInitialised
            => RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? NAudio.AudioEngine.IsInitialised
            : BASS.AudioEngine.IsInitialised;

        public static void Init()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                NAudio.AudioEngine.Init();
            else
                BASS.AudioEngine.Init();
        }

        public static void PlayOnce(IAudioClip audioClip)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                NAudio.AudioEngine.PlayOnce((NAudio.AudioClip)audioClip);
            else
                BASS.AudioEngine.PlayOnce((BASS.AudioClip)audioClip);
        }

        public static void Render(IAudioClip audioClip, string path)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                NAudio.AudioEngine.Render((NAudio.AudioClip)audioClip, path);
            else
                BASS.AudioEngine.Render((BASS.AudioClip)audioClip, path);
        }

        public static bool IsAudioFile(string path)
            => RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? NAudio.AudioEngine.IsAudioFile(path)
            : BASS.AudioEngine.IsAudioFile(path);

        public void Play(IAudioClip audioClip)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                ((IAudioEngine<NAudio.AudioClip>)this).Play((NAudio.AudioClip)audioClip);
            else
                ((IAudioEngine<BASS.AudioClip>)this).Play((BASS.AudioClip)audioClip);
        }

        public void StopAll()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                ((IAudioEngine<NAudio.AudioClip>)this).StopAll();
            else
                ((IAudioEngine<BASS.AudioClip>)this).StopAll();
        }
    }
}
