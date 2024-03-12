namespace CVAS.TerminalInterface
{
    /// <summary>
    /// Generic exception for the <see cref="Terminal"/> static class.
    /// </summary>
    public class TerminalException : Exception
    {
        public TerminalException(string message) : base(message) { }
    }
}
