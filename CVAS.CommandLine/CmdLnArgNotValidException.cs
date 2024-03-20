namespace CVAS.CommandLine
{
    internal class CmdLnArgNotValidException : Exception
    {
        public CmdLnArgNotValidException(string message) : base(message) { }
    }
}
