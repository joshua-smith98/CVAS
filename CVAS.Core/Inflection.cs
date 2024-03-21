using CVAS.AudioEngineNS;

namespace CVAS.Core
{
    /// <summary>
    /// Represents a single inflection, including its type and relevant <see cref="IAudioClip"/>.
    /// </summary>
    public class Inflection : IDisposable
    {
        public InflectionType InflectionType { get; }
        public IAudioClip AudioClip { get; private set; }

        public Inflection(InflectionType inflectionType, IAudioClip audioClip)
        {
            InflectionType = inflectionType;
            AudioClip = audioClip;
        }

        ~Inflection()
        {
            Dispose();
        }

        public void Dispose()
        {
            AudioClip?.Dispose();
            AudioClip = null!;
            GC.SuppressFinalize(this);
        }
    }
}
