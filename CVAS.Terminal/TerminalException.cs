namespace CVAS.TerminalNS
{
    /// <summary>
    /// Generic exception for the <see cref="Terminal"/> static class.
    /// </summary>
    public class TerminalException(string message) : Exception(message) { }
}
