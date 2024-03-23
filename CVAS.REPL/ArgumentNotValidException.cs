namespace CVAS.REPL
{
    /// <summary>
    /// Represents an error caused by an Argument being written incorrectly, or otherwise being unable to be read.
    /// </summary>
    internal class ArgumentNotValidException(string message) : REPLException(message) { }
}
