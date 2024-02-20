using CVAS.AudioEngine;

namespace CVAS.DataStructure
{
    public class SpokenPhrase : IPhrase
    {
        public string str { get; }
        public string[] words { get; }
        
        private IAudioClip _audioClip { get; }

        internal SpokenPhrase(string str, string[] words, IAudioClip audioClip)
        {
            this.str = str;
            this.words = words;
            _audioClip = audioClip;
        }

        public IAudioClip GetAudioClip() => _audioClip;
    }
}
