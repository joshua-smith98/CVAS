namespace CVAS.CommandLine
{
    internal class CmdLnContextNotValidException : CmdLnException
    {
        public CmdLnContextNotValidException(string message) : base(message) { }
    }
}
