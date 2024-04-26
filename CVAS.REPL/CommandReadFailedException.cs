namespace CVAS.REPL
{
    /// <summary>
    /// Represents an error caused by a failure to validate <see cref="Command.Str"/>.
    /// </summary>
    internal class CommandReadFailedException(string message) : REPLException(message) { }
}
