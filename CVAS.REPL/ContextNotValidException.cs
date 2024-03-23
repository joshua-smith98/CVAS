namespace CVAS.REPL
{
    /// <summary>
    /// Represents an error with the current REPL context.
    /// </summary>
    internal class ContextNotValidException(string message) : REPLException(message) { }
}
