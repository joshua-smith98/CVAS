using CVAS.AudioEngine;

namespace CVAS.DataStructure
{
    /// <summary>
    /// Represents a speakable phrase positioned within a sentence, with a single associated <see cref="DataStructure.Inflection"/>.
    /// </summary>
    public class SpokenPhrase : IPhrase
    {
        public string Str { get; }
        public string[] Words { get; }

        /// <summary>
        /// The <see cref="DataStructure.Inflection"/> associated with this SpokenPhrase.
        /// </summary>
        public Inflection Inflection { get; }

        private IAudioClip audioClip { get; }

        internal SpokenPhrase(string str, string[] words, IAudioClip audioClip, Inflection inflection)
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
    }
}
