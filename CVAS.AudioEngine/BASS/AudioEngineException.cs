namespace CVAS.AudioEngine.BASS
{
    /// <summary>
    /// Represents any exception thrown in relation to the audio engine.
    /// </summary>
    /// <param name="message"></param>
    public class AudioEngineException(string message) : Exception(message);
}
