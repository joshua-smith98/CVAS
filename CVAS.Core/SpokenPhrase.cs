using CVAS.AudioEngineNS;

namespace CVAS.Core
{
    /// <summary>
    /// Represents a speakable phrase positioned within a sentence, with a single associated <see cref="Core.InflectionType"/>.
    /// </summary>
    public class SpokenPhrase : IPhrase
    {
        public string Str { get; }
        public string[] Words { get; }

        /// <summary>
        /// The <see cref="Core.InflectionType"/> associated with this SpokenPhrase.
        /// </summary>
        public InflectionType Inflection { get; }

        private IAudioClip audioClip;

        internal SpokenPhrase(string str, string[] words, IAudioClip audioClip, InflectionType inflection)
        {
            Str = str;
            Words = words;
            Inflection = inflection;
            this.audioClip = audioClip;
        }

        /// <summary>
        /// Gets the associated <see cref="IAudioClip"/> for this SpokenPhrase.
        /// </summary>
        /// <returns></returns>
        public IAudioClip GetAudioClip() => audioClip;

        /// <summary>
        /// Determines whether this <see cref="SpokenPhrase"/> is one of the special phrases used for punctuation.
        /// </summary>
        /// <returns></returns>
        public bool IsSpecialPhrase()
        {
            return Phrase.SpecialPhrases.Values.Any(x => x.ToString() == Str);
        }

        /// <summary>
        /// Determines whether this <see cref="SpokenPhrase"/> is empty.
        /// </summary>
        /// <returns></returns>
        public bool IsEmptyPhrase()
        {
            return Str == "NULL";
        }

        public void Dispose()
        {
            audioClip?.Dispose();
            audioClip = null!;
            GC.SuppressFinalize(this);
        }
    }
}
