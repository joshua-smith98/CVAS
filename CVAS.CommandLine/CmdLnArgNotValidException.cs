namespace CVAS.CommandLine
{
    /// <summary>
    /// An exception that is thrown when a <see cref="CmdLnArgument"/> fails to read from the given args.
    /// </summary>
    internal class CmdLnArgNotValidException(string message) : CmdLnException(message) { }
}
