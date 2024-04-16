using CVAS.AudioEngine;

namespace CVAS.Core
{
    /// <summary>
    /// Represents a single inflection, including its type and relevant <see cref="WinAudioEngineNS.AudioClip"/>.
    /// </summary>
    public class Inflection(InflectionType inflectionType, IAudioClip audioClip)
    {
        public InflectionType InflectionType => inflectionType;
        public IAudioClip AudioClip => audioClip;
    }
}
