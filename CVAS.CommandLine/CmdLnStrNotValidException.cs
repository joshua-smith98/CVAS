namespace CVAS.CommandLine
{
    internal class CmdLnStrNotValidException : CmdLnException
    {
        public CmdLnStrNotValidException(string message) : base(message) { }
    }
}
