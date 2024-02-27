namespace CVAS.REPL
{
    public interface ICommand
    {
        public string Str { get; }
        public string Description { get; }
        public string Usage { get; }

        public ICommand? SubCommand { get; } // For potential future use - not yet implemented

        public List<IArgument> Arguments { get; }

        public void RunFrom(ref string str);
    }
}
