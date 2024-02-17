using CVAS.AudioEngine;

namespace CVAS.DataStructure
{
    public partial class Phrase
    {
        public static readonly Dictionary<string, char> SPECIAL_PHRASES = new()
        {
            {"VERTICAL_LINE", '|'},
            {"COMMA", ','},
            {"PERIOD", '.'}
        };
    }

    public partial class Library
    {
        public static readonly Phrase[] DEFAULT_PHRASES =
        {
            // Special phrases/punctuation - default to all Libraries
            new Phrase(Phrase.SPECIAL_PHRASES["VERTICAL_LINE"].ToString(), new Delay(0)), // '|' - delay of 0ms
            new Phrase(Phrase.SPECIAL_PHRASES["COMMA"].ToString(), new Delay(100)), // ','  - delay of 100ms
            new Phrase(Phrase.SPECIAL_PHRASES["PERIOD"].ToString(), new Delay(250)) // '.' - delay of 250ms
        };
    }
}
