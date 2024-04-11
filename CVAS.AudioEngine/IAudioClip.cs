using NAudio.Wave;

namespace CVAS.AudioEngineNS
{
    /// <summary>
    /// Interface representing any playable piece of audio.
    /// </summary>
    public interface IAudioClip : IDisposable // Inherits IDisposable, because some subclasses need to be disposed
    {
        internal IWaveProvider ToWaveProvider();
    }
}
