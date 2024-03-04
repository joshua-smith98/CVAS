using CVAS.AudioEngine;

namespace CVAS.DataStructure
{
    /// <summary>
    /// Represents a single inflection, including its type and relevant <see cref="IAudioClip"/>.
    /// </summary>
    public class Inflection
    {
        public InflectionType InflectionType { get; }
        public IAudioClip AudioClip { get; }

        public Inflection(InflectionType inflectionType, IAudioClip audioClip)
        {
            InflectionType = inflectionType;
            AudioClip = audioClip;
        }
    }
}
