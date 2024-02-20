using CVAS.AudioEngine;

namespace CVAS.DataStructure
{
    public class Sentence : IPhrase
    {
        public string str { get; }
        public string[] words { get; }

        private SpokenPhrase[] _spokenPhrases;

        internal Sentence(string str, string[] words, SpokenPhrase[] spokenPhrases)
        {
            this.str = str;
            this.words = words;
            this._spokenPhrases = spokenPhrases;
        }

        public IAudioClip GetAudioClip()
        {
            return new Playlist(_spokenPhrases.Select(x => x.GetAudioClip()).ToArray());
        }
    }
}
