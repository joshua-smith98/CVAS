namespace CVAS.CommandLine
{
    /// <summary>
    /// An exception that is thrown when <see cref="CmdLnContext"/> fails to run because of a context error.
    /// </summary>
    internal class CmdLnContextNotValidException(string message) : CmdLnException(message) { }
}
