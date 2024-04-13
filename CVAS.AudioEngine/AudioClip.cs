namespace CVAS.AudioEngineNS
{
    public abstract class AudioClip : IDisposable
    {
        internal abstract int GetStreamHandle();

        public virtual void Dispose() => GC.SuppressFinalize(this);
    }
}
