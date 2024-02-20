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

        /// <summary>
        /// Finds the collection of Phrases from a given <see cref="Library"/> that can be concatenated to create this Phrase.
        /// </summary>
        /// <param name="inLibrary">The Library to search for sub-phrases.</param>
        /// <returns></returns>
        public Phrase[] FindSubPhrases(Library inLibrary)
        {
            List<string> tempWords = words.ToList();
            List<Phrase> subPhrases = new List<Phrase>();

            // Finds the longest beginning subphrase and adds to our list, then subtracts that subphrase from this phrase's words.
            // Repeats until there are no more words remaining, or no more subphrases can be found.
            while (tempWords.Count > 0)
            {
                Phrase? subPhrase = _findLargestSubphrase(tempWords.ToArray(), inLibrary);
                if (subPhrase is null) break;

                subPhrases.Add(subPhrase);
                tempWords.RemoveRange(0, subPhrase.words.Length);
            }

            return subPhrases.ToArray();
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

        /// <summary>
        /// Finds the longest phrase in a given <see cref="Library"/> that matches some first of the given words, or null.
        /// </summary>
        /// <param name="words"></param>
        /// <param name="inLibrary">The Library to search.</param>
        /// <returns>The longest <see cref="Phrase"/>, or <see cref="null"/>.</returns>
        private static Phrase? _findLargestSubphrase(string[] words, Library inLibrary)
        {
            // Iterates through words to find the largest subphrase of those words, or null

            List<string> testPhrase = new List<string>(); // List of words that will be used to test for a possible subphrase
            Phrase? subPhrase = null;

            foreach (string word in words)
            {
                testPhrase.Add(word.ToLower());

                // Find matching phrase
                Phrase? tempSubPhrase = inLibrary.phrases.ToList().Find(libPhrase => libPhrase.words.Select(x => x.ToLower()).SequenceEqual(testPhrase));

                // Case: there is a match
                if (tempSubPhrase is not null)
                {
                    subPhrase = tempSubPhrase;
                }
                // Case: there is no match
                    // Do nothing
            }

            return subPhrase;
        }
    }
}
