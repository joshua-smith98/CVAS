﻿using CVAS.AudioEngine;

namespace CVAS.DataStructure
{
    /// <summary>
    /// A collection of <see cref="Phrase"/>s, including some default punctuation.
    /// </summary>
    public partial class Library
    {
        private List<Phrase> _phrases = DEFAULT_PHRASES.ToList(); // All Libraries include the special phrases by default
        public Phrase[] phrases => _phrases.ToArray(); //public get only

        public Library(Phrase[] phrases)
        {
            _phrases.AddRange(phrases);
        }

        public static Library LoadFromFolder(string path)
        {
            // Path validity checks
            if (!Directory.Exists(path)) throw new DirectoryNotFoundException();

            // Construct list of phrases to build library from
            List<Phrase> phrases = new List<Phrase>();

            // Assume we could have either a middle, end or both inflections.
            // First, run through all middle inflection files (no suffix) and attach any end inflections if they exist
            // If we add an end inflection to a phrase, remove it from the list of end inflection files

            string[] files = Directory.GetFiles(path);
            List<string> files_ends = files.Where(x => Path.GetFileNameWithoutExtension(x).EndsWith(".f")).ToList(); // List of files with end inflection
            string[] files_middles = files.Where(x => !files_ends.Contains(x)).ToArray(); // List of files with middle inflection (all that aren't an end)

            foreach (string file_middle in files_middles)
            {
                string str = Path.GetFileNameWithoutExtension(file_middle);
                IAudioClip audioClip_middle = new AudioFileStreaming(file_middle);

                // Check for ending inflection: construct new file path using directory, filename without extension, ".f" and extension
                string file_end = Path.Combine(
                    path, // directory name
                    Path.GetFileNameWithoutExtension(file_middle) + ".f" + // Filename + ending suffice
                    Path.GetExtension(file_middle)
                    );

                // Try to load ending inflection
                IAudioClip? audioClip_end = null;

                try // If it doesn't exist, or isn't an audio file, audioClip_end = null
                {
                    audioClip_end = new AudioFileStreaming(file_end);
                }
                catch { }

                // Construct new phrases and add to list
                if (audioClip_end is not null)
                {
                    // If we add an end inflection to a phrase, remove it from the list of end inflection files
                    phrases.Add(new Phrase(str, audioClip_end, audioClip_middle));
                    files_ends.Remove(file_end);
                }
                else phrases.Add(new Phrase(str, audioClip_middle, Inflection.Middle));
            }

            // Add all remaining end inflection files to a phrase
            foreach (string file_end in files_ends)
            {
                string str = Path.GetFileNameWithoutExtension(file_end);
                str = str.Substring(0, str.Length - 2);
                IAudioClip audioClip_end = new AudioFileStreaming(file_end);

                phrases.Add(new Phrase(str, audioClip_end));
            }

            // Construct and return library
            return new Library(phrases.ToArray());
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
            return _findSubPhrases(phrase.words);
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

            Phrase[] subPhrases = _findSubPhrases(words);

            SpokenPhrase[] spokenPhrases = new SpokenPhrase[subPhrases.Length];

            // Finds any phrases that come before a Period, and assigns the End inflection to them - otherwise, Middle.
            for (int i = 0; i < spokenPhrases.Length; i++)
            {
                // Case: This is not the last phrase, and the next phrase is a period.
                if (i < spokenPhrases.Length - 1 && subPhrases[i + 1].str == Phrase.SPECIAL_PHRASES["PERIOD"].ToString())
                {
                    spokenPhrases[i] = subPhrases[i].GetSpoken(Inflection.End);
                }
                else spokenPhrases[i] = subPhrases[i].GetSpoken(Inflection.Middle);
            }

            return new Sentence(str, words, spokenPhrases);
        }

        /// <summary>
        /// Finds the collection of Phrases in this Library that can be concatenated to match the given words.
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        private Phrase[] _findSubPhrases(string[] words)
        {
            List<string> tempWords = words.ToList();
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
