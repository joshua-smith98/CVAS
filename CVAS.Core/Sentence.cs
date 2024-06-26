﻿using CVAS.AudioEngine;

namespace CVAS.Core
{
    /// <summary>
    /// Represents a speakable sentence, made up of a sequence of <see cref="SpokenPhrase"/>s.
    /// </summary>
    public class Sentence
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
        /// Gets a playable <see cref="AudioClip"/> for this sentence.
        /// </summary>
        /// <returns>Note: always returns <see cref="Playlist"/> as <see cref="AudioClip"/>.</returns>
        public IAudioClip GetAudioClip()
        {
            return IPlaylist.New(SpokenPhrases.Select(x => x.GetAudioClip()).ToArray());
        }
    }
}
