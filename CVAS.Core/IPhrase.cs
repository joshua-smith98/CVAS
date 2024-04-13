using CVAS.AudioEngineNS;

namespace CVAS.Core
{
    /// <summary>
    /// Interface representing a speakable phrase consisting of one or more words.
    /// </summary>
    public interface IPhrase : IDisposable
    {
        public string Str { get; }
        public string[] Words { get; }

        /// <summary>
        /// Gets the default <see cref="AudioClip"/> associated with this phrase.
        /// </summary>
        /// <returns></returns>
        public AudioClip GetAudioClip();
    }
}
