namespace CVAS.REPL
{
    internal class RenderCommand : ICommand
    {
        public string Str => "render";

        public string Description => "Renders the given sentence to a file, using the currently loaded library.";

        public string Usage => "render \"[sentence to render]\" \"[path to file]\"";

        public ICommand? SubCommand { get; }

        public IArgument[] Arguments { get; } =
        {
            new StringArgument(), // Sentence
            new StringArgument(), // Path to file
        };

        internal RenderCommand() { }

        public void RunFrom(ref string str)
        {
            throw new NotImplementedException(); // File rendering is yet to be implemented!
        }
    }
}
