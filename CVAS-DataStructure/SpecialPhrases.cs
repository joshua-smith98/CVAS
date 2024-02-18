﻿using CVAS.AudioEngine;

namespace CVAS.DataStructure
{
    public partial class Phrase
    {
        /// <summary>
        /// The collection of all special phrases (usually punctuation).
        /// </summary>
        public static readonly Dictionary<string, char> SPECIAL_PHRASES = new()
        {
            {"VERTICAL_LINE", '|'}, // Used to place a boundary between words, e.g. if you wanted to split "TheTrainOnPlatformOne" into "TheTrainOnPlatform" + "One"
            {"COMMA", ','},
            {"PERIOD", '.'}
        };
    }

    public partial class Library
    {
        /// <summary>
        /// The collection of default phrases found in every <see cref="Library"/>, and their corresponding <see cref="IAudioClip"/>s (usually a <see cref="Delay"/>).
        /// </summary>
        public static readonly Phrase[] DEFAULT_PHRASES =
        {
            // Special phrases/punctuation - default to all Libraries
            new Phrase(Phrase.SPECIAL_PHRASES["VERTICAL_LINE"].ToString(), new Delay(0)), // '|' - delay of 0ms
            new Phrase(Phrase.SPECIAL_PHRASES["COMMA"].ToString(), new Delay(100)), // ','  - delay of 100ms
            new Phrase(Phrase.SPECIAL_PHRASES["PERIOD"].ToString(), new Delay(250)) // '.' - delay of 250ms
        };
    }
}