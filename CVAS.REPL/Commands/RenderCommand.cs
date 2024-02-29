namespace CVAS.REPL
{
    /// <summary>
    /// An <see cref="ICommand"/> that renders a given sentence to a file.
    /// </summary>
    internal class RenderCommand : ICommand
    {
        public string Str => "render";

        public string Description => "Renders the given sentence to a file, using the currently loaded library.";

        public string Usage => "render [sentence to render] [path to file]";

        public ICommand? SubCommand { get; }

        public IArgument[] Arguments { get; } =
        {
            new StringArgument(), // Sentence
            new StringArgument(), // Path to file
        };

        internal RenderCommand() { }

        public void RunFrom(string str)
        {
            // Validity check: str must begin with "load"
            if (!str.StartsWith(Str)) throw new CommandNotValidException();

            // Try to read arguments
            var temp_str = str.Substring(Str.Length).TrimStart();

            foreach (IArgument argument in Arguments)
            {
                argument.ReadFrom(ref temp_str); // If this fails, an ArgumentNotValidException will be thrown, then caught by the REPL class.
            }

            // Validity check: str must be empty after all arguments are read
            if (temp_str != "") throw new ArgumentNotValidException();

            // Validity check: CurrentLibrary must not be null
            if (REPL.Instance.CurrentLibrary is null) throw new ContextNotValidException();

            // JUST PRINTS DEBUG INFO FOR NOW
            var sentence_str = Arguments[0].Value as string;
            var path = Arguments[1].Value as string;

            Console.WriteLine($"DEBUG: Rendering \"{sentence_str}\" to path:\n\t{path}"); // Debug print - file rendering is not yet implemented!
        }
    }
}
