namespace CVAS.CommandLine
{
    internal class CmdLnArgNotValidException : CmdLnException
    {
        public CmdLnArgNotValidException(string message) : base(message) { }
    }
}
