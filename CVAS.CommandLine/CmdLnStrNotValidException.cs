namespace CVAS.CommandLine
{
    /// <summary>
    /// Exception that is thrown when a <see cref="CmdLnArgument"/>'s Str does not match the given value.
    /// </summary>
    internal class CmdLnStrNotValidException : CmdLnException
    {
        public CmdLnStrNotValidException(string message) : base(message) { }
    }
}
