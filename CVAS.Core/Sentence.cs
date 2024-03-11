﻿using CVAS.AudioEngine;

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
        public SpokenPhrase[] spokenPhrases { get; }

        internal Sentence(string str, string[] words, SpokenPhrase[] spokenPhrases)
        {
            Str = str;
            Words = words;
            this.spokenPhrases = spokenPhrases;
        }

        /// <summary>
        /// Gets a playable <see cref="IAudioClip"/> for this sentence.
        /// </summary>
        /// <returns>Note: always returns <see cref="Playlist"/> as <see cref="IAudioClip"/>.</returns>
        public IAudioClip GetAudioClip()
        {
            return new Playlist(spokenPhrases.Select(x => x.GetAudioClip()).ToArray());
        }
    }
}