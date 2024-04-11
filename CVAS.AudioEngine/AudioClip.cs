using NAudio.Wave;

namespace CVAS.AudioEngineNS
{
    /// <summary>
    /// Abstract class representing any playable piece of audio.
    /// </summary>
    public abstract class AudioClip : IDisposable // Inherits IDisposable, because some subclasses need to be disposed
    {
        internal abstract IWaveProvider ToWaveProvider();

        public virtual void Dispose() => GC.SuppressFinalize(this); // AudioClips are sometimes disposable, but not always.
    }
}
