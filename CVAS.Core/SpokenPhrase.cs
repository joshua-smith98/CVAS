using CVAS.AudioEngine;

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

        private IAudioClip audioClip { get; }

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

        public void Dispose()
        {
            audioClip.Dispose();
        }
    }
}
