using CVAS.AudioEngine;

namespace CVAS.DataStructure
{
    public class Sentence : IPhrase
    {
        public string str { get; }
        public string[] words { get; }

        public SpokenPhrase[] spokenPhrases { get; }

        internal Sentence(string str, string[] words, SpokenPhrase[] spokenPhrases)
        {
            this.str = str;
            this.words = words;
            this.spokenPhrases = spokenPhrases;
        }

        public IAudioClip GetAudioClip()
        {
            return new Playlist(spokenPhrases.Select(x => x.GetAudioClip()).ToArray());
        }
    }
}
