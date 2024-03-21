namespace CVAS.CommandLine
{
    /// <summary>
    /// Exception that covers all exceptions related to the CVAS.CommandLine project.
    /// </summary>
    internal class CmdLnException : Exception
    {
        public CmdLnException(string message) : base(message) { }
    }
}
