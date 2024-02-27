namespace CVAS.REPL
{
    public interface IArgument
    {
        public object? Value { get; }
        public Type ValueType { get; }

        public void ReadFrom(ref string str);
    }
}