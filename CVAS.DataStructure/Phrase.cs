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
        /// The collection of <see cref="Inflection"/>s associated with this phrase, each containing an <see cref="IAudioClip"/>.
        /// </summary>
        public Inflection[] Inflections => inflections.ToArray();
        private InflectionCollection inflections = new();
        
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
        /// Constructs a new phrase with the given <see cref="Inflection"/>s.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="inflections">The collection of inflections to assign to this phrase.</param>
        public Phrase(string str, params Inflection[] inflections)
        {
            Str = str;
            Words = getWords(str);
            this.inflections.AddRange(inflections);
        }

        /// <summary>
        /// Gets the default <see cref="IAudioClip"/> for this phrase, or silence.
        /// </summary>
        /// <returns><see cref="InflectionType.End"/> if available, otherwise <see cref="InflectionType.Middle"/>. If there is no associated <see cref="IAudioClip"/>, this returns <see cref="Silence"/>.</returns>
        public IAudioClip GetAudioClip()
        {
            if (inflections.Select(x => x.InflectionType).Contains(InflectionType.End))
            {
                return inflections[InflectionType.End];
            }
            else if (inflections.Select(x => x.InflectionType).Contains(InflectionType.Middle))
            {
                return inflections[InflectionType.Middle];
            }
            else return new Silence(0);
        }

        /// <summary>
        /// Retrieves the <see cref="IAudioClip"/> with the specified <see cref="InflectionType"/>, or default.
        /// </summary>
        /// <param name="inflection"></param>
        /// <returns></returns>
        public IAudioClip GetAudioClip(InflectionType inflection)
        {
            if (inflections.Select(x => x.InflectionType).Contains(inflection))
            {
                return inflections[inflection];
            }
            else return GetAudioClip();
        }

        /// <summary>
        /// Gets the <see cref="SpokenPhrase"/> for this phrase, given the specified <see cref="InflectionType"/>.
        /// </summary>
        /// <param name="inflection"></param>
        /// <returns></returns>
        public SpokenPhrase GetSpoken(InflectionType inflection)
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
