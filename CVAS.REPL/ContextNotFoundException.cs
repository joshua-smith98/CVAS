namespace CVAS.REPL
{
    /// <summary>
    /// Thrown when a neccesary part of the REPL context was not found by a Command.
    /// </summary>
    internal class ContextNotFoundException(string message) : REPLException(message) { }
}
