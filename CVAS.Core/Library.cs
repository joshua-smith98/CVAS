using CVAS.AudioEngine;

namespace CVAS.Core
{
    /// <summary>
    /// A collection of <see cref="Phrase"/>s, including some default punctuation.
    /// </summary>
    public partial class Library : IDisposable
    {
        public Phrase[] Phrases => phrases.ToArray(); //public get only
        private readonly List<Phrase> phrases = DefaultPhrases.ToList(); // All Libraries include the special phrases by default

        public Library(params Phrase[] phrases)
        {
            this.phrases.AddRange(phrases);
        }

        /// <summary>
        /// Finds the <see cref="Phrase"/> in this library that contains <paramref name="str"/>, or null.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>The specified <see cref="Phrase"/> or null if unsuccessful.</returns>
        public Phrase? FindPhrase(string str)
        {
            foreach(Phrase phr in phrases)
            {
                if (phr.Str == str) return phr;
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
            return FindSubPhrases(phrase.Words);
        }

        /// <summary>
        /// Takes a given string and attempts to find a speakable sentence using the phrases in this library.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public Sentence GetSentence(string str)
        {
            string[] words = Phrase.getWords(str); // TODO? Fix dependency on Phrase here

            // Copy paste from FindSubPhrases(), but with a big difference: we are calculating inflections here!

            Phrase[] subPhrases = FindSubPhrases(words);

            SpokenPhrase[] spokenPhrases = new SpokenPhrase[subPhrases.Length];

            // Finds any phrases that come before a Period, and assigns the End inflection to them - otherwise, Middle.
            for (int i = 0; i < spokenPhrases.Length; i++)
            {
                // Case: This is not the last phrase, and the next phrase is a period.
                if (i < spokenPhrases.Length - 1 && subPhrases[i + 1].Str == Phrase.SpecialPhrases["PERIOD"].ToString())
                {
                    spokenPhrases[i] = subPhrases[i].GetSpoken(InflectionType.End);
                }
                else spokenPhrases[i] = subPhrases[i].GetSpoken(InflectionType.Middle);
            }

            return new Sentence(str, words, spokenPhrases);
        }

        /// <summary>
        /// Finds the collection of Phrases in this Library that can be concatenated to match the given words.
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        private Phrase[] FindSubPhrases(string[] words)
        {
            List<string> tempWords = words.ToList();
            List<Phrase> subPhrases = new List<Phrase>();

            // Finds the longest beginning subphrase and adds to our list, then subtracts that subphrase from this phrase's words.
            // Repeats until there are no more words remaining, or no more subphrases can be found.
            while (tempWords.Count > 0)
            {
                Phrase? subPhrase = FindLargestSubphrase(tempWords.ToArray());
                if (subPhrase is null)
                {
                    // If we can't find a subphrase, just add an empty phrase to the list and continue
                    subPhrases.Add(new Phrase("NULL")); // TODO: make this into a static readonly Phrase.NULL ?
                    tempWords.RemoveRange(0, 1);
                    continue;
                }

                subPhrases.Add(subPhrase);
                tempWords.RemoveRange(0, subPhrase.Words.Length);
            }

            return subPhrases.ToArray();
        }

        /// <summary>
        /// Finds the longest phrase in this Library that matches the first n of the given words, or null.
        /// </summary>
        /// <param name="words"></param>
        /// <param name="inLibrary">The Library to search.</param>
        /// <returns>The longest <see cref="Phrase"/>, or <see cref="null"/>.</returns>
        private Phrase? FindLargestSubphrase(string[] words)
        {
            // Iterates through words to find the largest subphrase of those words, or null

            List<string> testPhrase = new List<string>(); // List of words that will be used to test for a possible subphrase
            Phrase? subPhrase = null;

            foreach (string word in words)
            {
                testPhrase.Add(word.ToLower());

                // Find matching phrase
                Phrase? tempSubPhrase = Phrases.ToList().Find(libPhrase => libPhrase.Words.Select(x => x.ToLower()).SequenceEqual(testPhrase));

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

        public void Dispose()
        {
            foreach (var phrase in phrases) phrase.Dispose();
        }
    }
}
