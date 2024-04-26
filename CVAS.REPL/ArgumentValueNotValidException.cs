namespace CVAS.REPL
{
    /// <summary>
    /// Represents an error caused by problems in reading the value of an argument.
    /// </summary>
    /// <param name="message"></param>
    internal class ArgumentValueNotValidException(string message) : ArgumentNotValidException(message);
}
