using CVAS.AudioEngine;
using CVAS.TerminalInterface;

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
            new StringArgument("sentence"), // Sentence
            new StringArgument("path"), // Path to file
        };

        internal RenderCommand() { }

        public void RunFrom(string str)
        {
            // Validity check: str must begin with "load"
            if (!str.StartsWith(Str)) throw new CommandNotValidException("Command does not match. This message should never be printed - if it was, open an issue on Github!");

            // Try to read arguments
            var temp_str = str.Substring(Str.Length).TrimStart();

            foreach (IArgument argument in Arguments)
            {
                argument.ReadFrom(ref temp_str); // If this fails, an ArgumentNotValidException will be thrown, then caught by the REPL class.
            }

            // Validity check: str must be empty after all arguments are read
            if (temp_str != "") throw new ArgumentNotValidException($"Expected end of command, found: '{temp_str}'!");

            // Validity check: CurrentLibrary must not be null
            if (REPL.Instance.CurrentLibrary is null) throw new ContextNotValidException("No library is currenty loaded.");

            var sentence = REPL.Instance.CurrentLibrary.GetSentence((string)Arguments[0].Value);
            var path = (string)Arguments[1].Value;

            // Validity check: path directory must exist, or be empty
            var directoryName = Path.GetDirectoryName(path);
            if (directoryName != "" && !Directory.Exists(path)) throw new DirectoryNotFoundException();

            // Check for file exists and give option for overwriting
            if (File.Exists(path))
            {
                // Prompt user
                Terminal.BeginMessage();
                Terminal.Message("File already exists! Overwrite (y/n)?", ConsoleColor.Yellow);
                var response = Terminal.Prompt(">> ");
                Terminal.EndMessage();
                if (response.ToLower() != "y")
                {
                    // Cancel render on non-yes answer
                    Terminal.BeginMessage();
                    Terminal.Message("Render cancelled by user.");
                    Terminal.EndMessage();
                    return;
                }
            }

            // Render file
            AudioEngine.AudioEngine.Instance.Render(sentence.GetAudioClip(), path);
            Terminal.BeginMessage();
            Terminal.Message($"Rendered to: {path}");
            Terminal.EndMessage();
        }
    }
}
