using CVAS.AudioEngineNS;

namespace CVAS.Core
{
    /// <summary>
    /// Represents a single inflection, including its type and relevant <see cref="IAudioClip"/>.
    /// </summary>
    public class Inflection(InflectionType inflectionType, IAudioClip audioClip) : IDisposable
    {
        public InflectionType InflectionType => inflectionType;
        public IAudioClip AudioClip => audioClip;

        public void Dispose()
        {
            audioClip?.Dispose();
            audioClip = null!;
            GC.SuppressFinalize(this);
        }
    }
}
