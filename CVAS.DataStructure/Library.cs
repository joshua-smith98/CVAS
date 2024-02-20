﻿namespace CVAS.DataStructure
{
    /// <summary>
    /// A collection of <see cref="Phrase"/>s, including some default punctuation.
    /// </summary>
    public partial class Library
    {
        private List<Phrase> _phrases = DEFAULT_PHRASES.ToList();
        public Phrase[] phrases => _phrases.ToArray(); //public get only

        public Library(Phrase[] phrases)
        {
            _phrases.AddRange(phrases);
        }

        /// <summary>
        /// Finds the <see cref="Phrase"/> in this library that contains <paramref name="str"/>, or null.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>The specified <see cref="Phrase"/> or null if unsuccessful.</returns>
        public Phrase? FindPhrase(string str)
        {
            foreach(Phrase phr in _phrases)
            {
                if (phr.str == str) return phr;
            }

            return null;
        }

        /// <summary>
        /// Finds the collection of Phrases in this Library that can be concatenated to create the given <see cref="Phrase"/>.
        /// </summary>
        /// <param name="phrase"></param>
        /// <returns></returns>
        public Phrase[] FindSubPhrases(Phrase phrase)
        {
            List<string> tempWords = phrase.words.ToList();
            List<Phrase> subPhrases = new List<Phrase>();

            // Finds the longest beginning subphrase and adds to our list, then subtracts that subphrase from this phrase's words.
            // Repeats until there are no more words remaining, or no more subphrases can be found.
            while (tempWords.Count > 0)
            {
                Phrase? subPhrase = _findLargestSubphrase(tempWords.ToArray());
                if (subPhrase is null) break;

                subPhrases.Add(subPhrase);
                tempWords.RemoveRange(0, subPhrase.words.Length);
            }

            return subPhrases.ToArray();
        }

        /// <summary>
        /// Finds the longest phrase in this Library that matches the first n of the given words, or null.
        /// </summary>
        /// <param name="words"></param>
        /// <param name="inLibrary">The Library to search.</param>
        /// <returns>The longest <see cref="Phrase"/>, or <see cref="null"/>.</returns>
        private Phrase? _findLargestSubphrase(string[] words)
        {
            // Iterates through words to find the largest subphrase of those words, or null

            List<string> testPhrase = new List<string>(); // List of words that will be used to test for a possible subphrase
            Phrase? subPhrase = null;

            foreach (string word in words)
            {
                testPhrase.Add(word.ToLower());

                // Find matching phrase
                Phrase? tempSubPhrase = phrases.ToList().Find(libPhrase => libPhrase.words.Select(x => x.ToLower()).SequenceEqual(testPhrase));

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
