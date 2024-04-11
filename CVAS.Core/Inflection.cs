using CVAS.AudioEngineNS;

namespace CVAS.Core
{
    /// <summary>
    /// Represents a single inflection, including its type and relevant <see cref="AudioEngineNS.AudioClip"/>.
    /// </summary>
    public class Inflection(InflectionType inflectionType, AudioClip audioClip) : IDisposable
    {
        public InflectionType InflectionType => inflectionType;
        public AudioClip AudioClip => audioClip;

        public void Dispose()
        {
            audioClip?.Dispose();
            audioClip = null!;
            GC.SuppressFinalize(this);
        }
    }
}
