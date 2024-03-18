namespace CVAS.REPL
{
    /// <summary>
    /// Represents any <see cref="Exception"/> associated with the <see cref="REPL"/> class.
    /// </summary>
    internal abstract class REPLException : Exception
    {
        public REPLException(string message) : base(message) { }
    }
}
