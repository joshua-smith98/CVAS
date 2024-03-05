using CVAS.AudioEngine;
using CVAS.FileFormats;

namespace CVAS.DataStructure
{
    /// <summary>
    /// A collection of <see cref="Phrase"/>s, including some default punctuation.
    /// </summary>
    public partial class Library
    {
        public Phrase[] Phrases => phrases.ToArray(); //public get only
        private readonly List<Phrase> phrases = DefaultPhrases.ToList(); // All Libraries include the special phrases by default

        public Library(params Phrase[] phrases)
        {
            this.phrases.AddRange(phrases);
        }

        /// <summary>
        /// Constructs and loads a Library from the given directory. Assumes the directory contains audio files with correctly formatted file names.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public static Library LoadFromFolder(string path)
        {
            // Path validity checks
            if (!Directory.Exists(path)) throw new DirectoryNotFoundException();

            // Try to load from cache file
            string cachePath = Path.Combine(path, "cache.lbc");
            try
            {
                return LibraryCacheFile.LoadFrom(cachePath).Construct();
            }
            // Case: Some known error with the cache file -> we will load manually and automatically rebuild
            // Those commented out won't be once the library caching feature has finished testing
            catch (FileNotFoundException) { }
            //catch (InvalidFileHeaderException) { }
            //catch (InvalidFileFormatException) { }
            catch (InvalidFileHashException) { }

            // Construct list of phrases to build library from
            List<Phrase> phrases = new List<Phrase>();

            // Assume we could have either a middle, end or both inflections.
            // First, iterate through all middle inflection files (no suffix), create phrases and attach any end inflections if they exist
            // If we attach an end reflection, remove it from the list of end inflection files
            // Finally, add all remaining end inflection files to their own phrase and construct library.

            // Get files
            string[] files = Directory.GetFiles(path);
            List<string> files_ends = files.Where(x => Path.GetFileNameWithoutExtension(x).EndsWith(".f")).ToList(); // List of files with end inflection
            string[] files_middles = files.Where(x => !files_ends.Contains(x)).ToArray(); // List of files with middle inflection (all that aren't an end)

            // Iterate through all middle inflection files
            foreach (string file_middle in files_middles)
            {
                // Audio file validity check
                IAudioClip audioClip_middle;
                try
                {
                    audioClip_middle = new AudioFileStreaming(file_middle);
                }
                catch { continue; }

                Console.WriteLine(file_middle);

                // Phrase.str is file name without extension
                string str = Path.GetFileNameWithoutExtension(file_middle);

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
                    phrases.Add(new Phrase(str, new Inflection(InflectionType.End, audioClip_end), new Inflection(InflectionType.Middle, audioClip_middle)));
                    files_ends.Remove(file_end); // If we add an end inflection to a phrase, remove it from the list of end inflection files
                    Console.WriteLine(file_end);
                }
                else phrases.Add(new Phrase(str, new Inflection(InflectionType.Middle, audioClip_middle)));
            }

            // Add all remaining end inflection files to their own phrase
            foreach (string file_end in files_ends)
            {
                // Audio file validity check
                IAudioClip audioClip_end;

                try
                {
                    audioClip_end = new AudioFileStreaming(file_end);
                }
                catch { continue; }

                string str = Path.GetFileNameWithoutExtension(file_end);
                str = str.Substring(0, str.Length - 2);

                phrases.Add(new Phrase(str, new Inflection(InflectionType.End, audioClip_end)));
                Console.WriteLine(file_end);
            }

            // Construct library
            Library ret = new Library(phrases.ToArray());

            // Build cache
            LibraryCacheFile.Deconstruct(ret).SaveTo(cachePath);

            return ret;
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
    }
}
