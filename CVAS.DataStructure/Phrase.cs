using CVAS.AudioEngine;

namespace CVAS.DataStructure
{
    /// <summary>
    /// Represents a string, made up of a set of words and punctuation. Can also contain a linked <see cref="IAudioClip"/>.
    /// </summary>
    public partial class Phrase : IPhrase
    {
        public string str { get; }
        public string[] words { get; }

        private Dictionary<Inflection, IAudioClip> _audioClips = new();
        
        public Phrase (string str)
        {
            this.str = str;
            this.words = _getWords(str);
        }

        public Phrase(string str, IAudioClip audioClip, Inflection audioClipInflection = Inflection.End)
        {
            this.str = str;
            this.words = _getWords(str);
            _audioClips.Add(audioClipInflection, audioClip);
        }

        public Phrase(string str, IAudioClip audioClip_End, IAudioClip audioClip_Middle)
        {
            this.str = str;
            this.words = _getWords(str);
            _audioClips.Add(Inflection.Middle, audioClip_Middle);
            _audioClips.Add(Inflection.End, audioClip_End);
        }

        public IAudioClip GetAudioClip()
        {
            if (_audioClips.Keys.Contains(Inflection.End))
            {
                return _audioClips[Inflection.End];
            }
            else if (_audioClips.Keys.Contains(Inflection.Middle))
            {
                return _audioClips[Inflection.Middle];
            }
            else return new Silence(0);
        }

        public IAudioClip GetAudioClip(Inflection inflection)
        {
            if (_audioClips.Keys.Contains(inflection))
            {
                return _audioClips[inflection];
            }
            else return GetAudioClip();
        }

        public SpokenPhrase GetSpoken(Inflection inflection)
        {
            return new SpokenPhrase(str, words, GetAudioClip(inflection));
        }

        /// <summary>
        /// Splits a given string up into words and some special phrases (punctuation) using whitespace.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string[] _getWords(string str)
        {
            List<string> words = new List<string>();

            string word = "";
            foreach (char character in str)
            {
                // Case whitespace: add word to list and clear
                if (char.IsSeparator(character))
                {
                    if (word != "") words.Add(word); // Only add the word if there is a word to add (there might not be if there are two seperator chars in a row)
                    word = "";
                }
                // Case Special Phrases/Punctuation: add word & char to list and clear
                else if (SPECIAL_PHRASES.Values.Contains(character))
                {
                    if (word != "") words.Add(word); // Ditto
                    words.Add(character.ToString());
                    word = "";
                }
                // Case not whitespace or special phrase: add char to word
                else word += character;
            }

            // Case: last character was not seperator
            if (word != "") words.Add(word);

            return words.ToArray();
        }
    }
}
