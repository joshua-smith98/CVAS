// Conditional AudioEngine (NAudio for Windows and BASS for other OS)
#if Windows
using CVAS.WinAudioEngineNS;
#else
using CVAS.AudioEngineNS;
#endif

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
