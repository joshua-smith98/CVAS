using CVAS.AudioEngine;

namespace CVAS.DataStructure
{
    /// <summary>
    /// Represents a speakable phrase positioned within a sentence, with a single associated <see cref="Inflection"/>.
    /// </summary>
    public class SpokenPhrase : IPhrase
    {
        public string str { get; }
        public string[] words { get; }

        /// <summary>
        /// The <see cref="Inflection"/> associated with this SpokenPhrase.
        /// </summary>
        public Inflection inflection { get; }

        private IAudioClip _audioClip { get; }

        internal SpokenPhrase(string str, string[] words, IAudioClip audioClip, Inflection inflection)
        {
            this.str = str;
            this.words = words;
            this.inflection = inflection;
            _audioClip = audioClip;
        }

        /// <summary>
        /// Gets the associated <see cref="IAudioClip"/> for this SpokenPhrase.
        /// </summary>
        /// <returns></returns>
        public IAudioClip GetAudioClip() => _audioClip;
    }
}
