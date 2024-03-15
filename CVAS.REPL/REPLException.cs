namespace CVAS.REPL
{
    internal abstract class REPLException : Exception
    {
        public REPLException(string message) : base(message) { }
    }
}
