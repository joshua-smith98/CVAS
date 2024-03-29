﻿using CVAS.AudioEngineNS;

namespace CVAS.Core
{
    /// <summary>
    /// Represents a speakable sentence, made up of a sequence of <see cref="SpokenPhrase"/>s.
    /// </summary>
    public class Sentence : IPhrase
    {
        public string Str { get; }
        public string[] Words { get; }

        /// <summary>
        /// The sequence of <see cref="SpokenPhrase"/>s that defines this sentence.
        /// </summary>
        public SpokenPhrase[] SpokenPhrases { get; private set; }

        internal Sentence(string str, string[] words, SpokenPhrase[] spokenPhrases)
        {
            Str = str;
            Words = words;
            SpokenPhrases = spokenPhrases;
        }

        /// <summary>
        /// Gets a playable <see cref="IAudioClip"/> for this sentence.
        /// </summary>
        /// <returns>Note: always returns <see cref="Playlist"/> as <see cref="IAudioClip"/>.</returns>
        public IAudioClip GetAudioClip()
        {
            return new Playlist(SpokenPhrases.Select(x => x.GetAudioClip()).ToArray());
        }

        public void Dispose()
        {
            for (int i = 0; i < SpokenPhrases.Length; i++)
            {
                SpokenPhrases[i]?.Dispose();
                SpokenPhrases[i] = null!;
            }

            SpokenPhrases = null!;
            GC.SuppressFinalize(this);
        }
    }
}
