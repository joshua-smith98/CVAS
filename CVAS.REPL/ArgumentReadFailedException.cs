namespace CVAS.REPL
{
    /// <summary>
    /// Represents an error caused by a failure to read the value of an Argument.
    /// </summary>
    /// <param name="message"></param>
    internal class ArgumentReadFailedException(string message) : CommandRunFailedException(message);
}
