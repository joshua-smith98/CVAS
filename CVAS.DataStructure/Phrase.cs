using CVAS.AudioEngine;

namespace CVAS.DataStructure
{
    /// <summary>
    /// Represents a string, made up of a set of words and punctuation. Can also contain a linked <see cref="IAudioClip"/>.
    /// </summary>
    public partial class Phrase : IPhrase
    {
        public string Str { get; }
        public string[] Words { get; }

        /// <summary>
        /// The collection of <see cref="IAudioClip"/>s associated with this phrase, mapped to their inflections.
        /// </summary>
        public Dictionary<Inflection, IAudioClip> AudioClips { get; } = new();
        
        /// <summary>
        /// Constructs a new phrase with no associated audio.
        /// </summary>
        /// <param name="str"></param>
        public Phrase (string str)
        {
            Str = str;
            Words = getWords(str);
        }

        /// <summary>
        /// Constructs a new phrase with a single associated <see cref="IAudioClip"/>.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="audioClip"></param>
        /// <param name="audioClipInflection">The inflection of the associated audio clip (default is <see cref="Inflection.End"/>)</param>
        public Phrase(string str, IAudioClip audioClip, Inflection audioClipInflection = Inflection.End)
        {
            Str = str;
            Words = getWords(str);
            AudioClips.Add(audioClipInflection, audioClip);
        }

        /// <summary>
        /// Constructs a new phrase with two associated <see cref="IAudioClip"/>s, each mapped to their associated inflections.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="audioClip_End"></param>
        /// <param name="audioClip_Middle"></param>
        public Phrase(string str, IAudioClip audioClip_End, IAudioClip audioClip_Middle)
        {
            Str = str;
            Words = getWords(str);
            AudioClips.Add(Inflection.Middle, audioClip_Middle);
            AudioClips.Add(Inflection.End, audioClip_End);
        }

        /// <summary>
        /// Gets the default <see cref="IAudioClip"/> for this phrase, or silence.
        /// </summary>
        /// <returns><see cref="Inflection.End"/> if available, otherwise <see cref="Inflection.Middle"/>. If there is no associated <see cref="IAudioClip"/>, this returns <see cref="Silence"/>.</returns>
        public IAudioClip GetAudioClip()
        {
            if (AudioClips.Keys.Contains(Inflection.End))
            {
                return AudioClips[Inflection.End];
            }
            else if (AudioClips.Keys.Contains(Inflection.Middle))
            {
                return AudioClips[Inflection.Middle];
            }
            else return new Silence(0);
        }

        /// <summary>
        /// Retrieves the <see cref="IAudioClip"/> with the specified <see cref="Inflection"/>, or default.
        /// </summary>
        /// <param name="inflection"></param>
        /// <returns></returns>
        public IAudioClip GetAudioClip(Inflection inflection)
        {
            if (AudioClips.Keys.Contains(inflection))
            {
                return AudioClips[inflection];
            }
            else return GetAudioClip();
        }

        /// <summary>
        /// Gets the <see cref="SpokenPhrase"/> for this phrase, given the specified <see cref="Inflection"/>.
        /// </summary>
        /// <param name="inflection"></param>
        /// <returns></returns>
        public SpokenPhrase GetSpoken(Inflection inflection)
        {
            return new SpokenPhrase(Str, Words, GetAudioClip(inflection), inflection);
        }

        /// <summary>
        /// Splits a given string up into words and some special phrases (punctuation) using whitespace.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        internal static string[] getWords(string str)
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
                else if (SpecialPhrases.Values.Contains(character))
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
