using CVAS.AudioEngine;

namespace CVAS.REPL
{
    /// <summary>
    /// An <see cref="ICommand"/> that renders a given sentence to a file.
    /// </summary>
    internal class RenderCommand : ICommand
    {
        public string Str => "render";

        public string Description => "Renders the given sentence to a file at the given path, using the currently loaded library.";

        public string[] Usage { get; } = { "render [sentence] [path]" };

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

            var sentence = REPL.Instance.CurrentLibrary.GetSentence((string)Arguments[0].Value);
            var path = (string)Arguments[1].Value;

            // Validity check: path directory must exist, or be empty
            var directoryName = Path.GetDirectoryName(path);
            if (directoryName != "" && !Directory.Exists(path)) throw new DirectoryNotFoundException();

            // Check for file exists and give option for overwriting
            if (File.Exists(path))
            {
                // Prompt user
                Console.WriteLine();
                Console.WriteLine("File already exists! Overwrite (y/n)?");
                Console.Write(">>");
                var response = Console.ReadLine();
                if (response.ToLower() != "y")
                {
                    // Cancel render on non-yes answer
                    Console.WriteLine();
                    Console.WriteLine("Render cancelled.");
                    Console.WriteLine();
                    return;
                }
            }

            // Render file
            AudioEngine.AudioEngine.Instance.Render(sentence.GetAudioClip(), path);
            Console.WriteLine();
            Console.WriteLine($"Rendered to: {path}");
            Console.WriteLine();
        }
    }
}
