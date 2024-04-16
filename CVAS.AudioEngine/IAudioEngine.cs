using System.Runtime.InteropServices;

namespace CVAS.AudioEngine
{
    public interface IAudioEngine : IDisposable
    {
        internal static abstract IAudioEngine InstanceImpl { get; }
        internal static abstract bool IsInitialisedImpl { get; }

        internal static abstract void InitImpl();
        internal static abstract void PlayOnceImpl(IAudioClip audioClip);
        internal static abstract void RenderImpl(IAudioClip audioClip, string path);
        internal static abstract bool IsAudioFileImpl(string path);

        public void Play(IAudioClip audioClip);
        public void StopAll();

        public static IAudioEngine Instance
            => RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? NAudio.AudioEngine.InstanceImpl
            : BASS.AudioEngine.InstanceImpl;

        public static bool IsInitialised
            => RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? NAudio.AudioEngine.IsInitialisedImpl
            : BASS.AudioEngine.IsInitialisedImpl;

        public static void Init()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                NAudio.AudioEngine.InitImpl();
            else
                BASS.AudioEngine.InitImpl();
        }

        public static void PlayOnce(IAudioClip audioClip)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                NAudio.AudioEngine.PlayOnceImpl(audioClip);
            else
                BASS.AudioEngine.PlayOnceImpl(audioClip);
        }

        public static void Render(IAudioClip audioClip, string path)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                NAudio.AudioEngine.RenderImpl(audioClip, path);
            else
                BASS.AudioEngine.RenderImpl(audioClip, path);
        }

        public static bool IsAudioFile(string path)
            => RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? NAudio.AudioEngine.IsAudioFileImpl(path)
            : BASS.AudioEngine.IsAudioFileImpl(path);
    }
}
