using CVAS.AudioEngine;

namespace CVAS.DataStructure
{
    public class Phrase
    {
        public string str { get; }
        public string[] words { get; }
        public AudioFile? linkedAudio { get; }
        
        public Phrase (string str)
        {
            this.str = str;
            this.words = _getWords(str);
        }

        public Phrase(string str, AudioFile linkedAudio)
        {
            this.str = str;
            this.words = _getWords(str);
            this.linkedAudio = linkedAudio;
        }

        public Phrase[] FindSubPhrases(Library inLibrary)
        {
            List<string> tempWords = words.ToList();
            List<Phrase> subPhrases = new List<Phrase>();

            while (tempWords.Count > 0)
            {
                Phrase? subPhrase = _findLargestSubphrase(tempWords.ToArray(), inLibrary);
                if (subPhrase is null) break;

                subPhrases.Add(subPhrase);
                tempWords.RemoveRange(0, subPhrase.words.Length);
            }

            return subPhrases.ToArray();
        }

        private static string[] _getWords(string str)
        {
            List<string> words = new List<string>();

            string word = "";
            foreach (char character in str)
            {
                // Case whitespace: add word to list and clear
                if (char.IsSeparator(character))
                {
                    words.Add(word);
                    word = "";
                }
                // Case not whitespace: add char to word
                else word += character;
            }

            // Case: last character was not seperator
            if (word != "") words.Add(word);

            return words.ToArray();
        }

        private static Phrase? _findLargestSubphrase(string[] words, Library inLibrary)
        {
            // Iterates through words to find the largest subphrase of those words, or null

            List<string> testPhrase = new List<string>(); // List of words that will be used to test for a possible subphrase
            Phrase? subPhrase = null;

            foreach (string word in words)
            {
                testPhrase.Add(word.ToLower());
                foreach (string phr in testPhrase)
                {
                    Console.Write($"[{phr}] ");
                }
                Console.WriteLine();

                // Find matching phrase
                Phrase? tempSubPhrase = inLibrary.phrases.ToList().Find(libPhrase => libPhrase.words.Select(x => x.ToLower()).SequenceEqual(testPhrase));

                // Case: there is a match
                if (tempSubPhrase is not null)
                {
                    Console.WriteLine(" " + tempSubPhrase.str);
                    subPhrase = tempSubPhrase;
                }
                // Case: there is no match
                    // Do nothing
            }

            return subPhrase;
        }
    }
}
