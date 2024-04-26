namespace CVAS.REPL
{
    /// <summary>
    /// Represents an error caused by a failure to perform the actions associated with a <see cref="Command"/>.
    /// </summary>
    internal class CommandRunFailedException(string message) : REPLException(message) { }
}
