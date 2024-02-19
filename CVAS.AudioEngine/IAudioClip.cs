using NAudio.Wave;

namespace CVAS.AudioEngine
{
    /// <summary>
    /// Interface representing any playable piece of audio.
    /// </summary>
    public interface IAudioClip : IDisposable // Inherits IDisposable, because some subclasses need to be disposed
    {
        public WaveFormat WaveFormat { get; }

        public IWaveProvider toWaveProvider();
    }
}
