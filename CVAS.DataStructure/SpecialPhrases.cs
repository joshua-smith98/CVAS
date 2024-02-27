using CVAS.AudioEngine;

namespace CVAS.DataStructure
{
    public partial class Phrase
    {
        /// <summary>
        /// The collection of all special phrases (usually punctuation).
        /// </summary>
        public static readonly Dictionary<string, char> SpecialPhrases = new()
        {
            {"VERTICAL_LINE", '|'}, // Used to place a boundary between words, e.g. if you wanted to split "TheTrainOnPlatformOne" into "TheTrainOnPlatform" + "One"
            {"COMMA", ','},
            {"PERIOD", '.'}
        };
    }

    public partial class Library
    {
        /// <summary>
        /// The collection of default phrases found in every <see cref="Library"/>, and their corresponding <see cref="IAudioClip"/>s (usually a <see cref="Silence"/>).
        /// </summary>
        public static readonly Phrase[] DefaultPhrases =
        {
            // Special phrases/punctuation - default to all Libraries
            new Phrase(Phrase.SpecialPhrases["VERTICAL_LINE"].ToString(), new Silence(0)), // '|' - delay of 0ms
            new Phrase(Phrase.SpecialPhrases["COMMA"].ToString(), new Silence(100)), // ','  - delay of 100ms
            new Phrase(Phrase.SpecialPhrases["PERIOD"].ToString(), new Silence(250)) // '.' - delay of 250ms
        };
    }
}
