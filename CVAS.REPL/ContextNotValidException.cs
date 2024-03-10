namespace CVAS.REPL
{
    /// <summary>
    /// Represents an error with the current REPL context.
    /// </summary>
    internal class ContextNotValidException : Exception
    {
        public ContextNotValidException(string message) : base(message) { }
    }
}
