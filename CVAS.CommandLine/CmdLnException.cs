namespace CVAS.CommandLine
{
    internal abstract class CmdLnException : Exception
    {
        public CmdLnException(string message) : base(message) { }
    }
}
