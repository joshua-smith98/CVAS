﻿// Conditional AudioEngine (NAudio for Windows and BASS for other OS)
#if Windows
using CVAS.WinAudioEngineNS;
#else
using CVAS.AudioEngineNS;
#endif

namespace CVAS.Core
{
    /// <summary>
    /// Represents a string, made up of a set of words and punctuation. Can also contain a linked <see cref="AudioClip"/>.
    /// </summary>
    public partial class Phrase
    {
        public string Str { get; }
        public string[] Words { get; }

        /// <summary>
        /// The collection of <see cref="Inflection"/>s associated with this phrase, each containing an <see cref="AudioClip"/>.
        /// </summary>
        public Inflection[] Inflections => inflections.ToArray();
        private InflectionCollection inflections = [];
        
        /// <summary>
        /// Constructs a new phrase with no associated audio.
        /// </summary>
        /// <param name="str"></param>
        public Phrase (string str)
        {
            Str = str;
            Words = GetWords(str);
        }

        /// <summary>
        /// Constructs a new phrase with a single associated <see cref="AudioClip"/>, with <see cref="InflectionType.End"/>.
        /// </summary>
        /// <param name="str"></param>
        public Phrase(string str, AudioClip audioClip)
        {
            Str = str;
            Words = GetWords(str);
            inflections.Add(new Inflection(InflectionType.End, audioClip));
        }

        /// <summary>
        /// Constructs a new phrase with the given <see cref="Inflection"/>s.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="inflections">The collection of inflections to assign to this phrase.</param>
        public Phrase(string str, params Inflection[] inflections)
        {
            Str = str;
            Words = GetWords(str);
            this.inflections.AddRange(inflections);
        }

        /// <summary>
        /// Determines whether this <see cref="Phrase"/> is empty.
        /// </summary>
        /// <returns></returns>
        public bool IsEmptyPhrase() => Inflections.Length == 0;

        /// <summary>
        /// Gets the default <see cref="AudioClip"/> for this phrase, or silence.
        /// </summary>
        /// <returns><see cref="InflectionType.End"/> if available, otherwise <see cref="InflectionType.Middle"/>. If there is no associated <see cref="AudioClip"/>, this returns <see cref="Silence"/>.</returns>
        public AudioClip GetAudioClip()
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
        /// Retrieves the <see cref="AudioClip"/> with the specified <see cref="InflectionType"/>, or default.
        /// </summary>
        /// <param name="inflection"></param>
        /// <returns></returns>
        public AudioClip GetAudioClip(InflectionType inflection)
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
            return !IsEmptyPhrase()
                ? new SpokenPhrase(Str, Words, GetAudioClip(inflection), inflection) // Case: Inflections exist
                : new SpokenPhrase(Str, Words, GetAudioClip(), InflectionType.Null); // Case: No inflections (means this phrase couldn't be found)
        }

        /// <summary>
        /// Splits a given string up into words and some special phrases (punctuation) using whitespace.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        internal static string[] GetWords(string str)
        {
            List<string> words = [];

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
                else if (SpecialPhrases.ContainsValue(character))
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
