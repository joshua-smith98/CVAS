namespace CVAS.REPL
{
    /// <summary>
    /// Represents an error caused by a command not existing, or being written incorrectly.
    /// </summary>
    internal class CommandNotValidException(string message) : REPLException(message) { }
}
