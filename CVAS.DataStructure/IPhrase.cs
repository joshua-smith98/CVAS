using CVAS.AudioEngine;

namespace CVAS.DataStructure
{
    /// <summary>
    /// Interface representing a speakable phrase consisting of one or more words.
    /// </summary>
    public interface IPhrase
    {
        public string Str { get; }
        public string[] Words { get; }

        /// <summary>
        /// Gets the default <see cref="IAudioClip"/> associated with this phrase.
        /// </summary>
        /// <returns></returns>
        public IAudioClip GetAudioClip();
    }
}
