using CVAS.AudioEngine;

namespace CVAS.DataStructure
{
    public interface IPhrase
    {
        public string str { get; }
        public string[] words { get; }

        public IAudioClip GetAudioClip();
    }
}
