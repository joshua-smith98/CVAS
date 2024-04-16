namespace CVAS.AudioEngine
{
    public interface IAudioEngine : IDisposable
    {
        public static abstract IAudioEngine Instance { get; }
        public static abstract bool IsInitialised { get; }

        public static abstract void Init();
        public static abstract void PlayOnce(IAudioClip audioClip);
        public static abstract void Render(IAudioClip audioClip, string path);
        public static abstract bool IsAudioFile(string path);

        public void Play(IAudioClip audioClip);
        public void StopAll();
    }
}
