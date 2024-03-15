using CVAS.AudioEngine;
using CVAS.Core;
using CVAS.TerminalNS;

namespace CVAS.REPL
{
    /// <summary>
    /// An <see cref="Command"/> that renders a given sentence to a file.
    /// </summary>
    internal class RenderCommand : Command
    {
        public override string Str => "render";

        public override string Description => "Renders the given sentence to a file at the given path, and commits that sentence to memory.";

        public override string[] Usage { get; } = { "render [path] [sentence]", "render [path]" };

        public override Command[] SubCommands { get; } = { };

        public override Argument[] Arguments { get; } =
        {
            new StringArgument("path", true), // Path to file
            new StringArgument("sentence", false), // Sentence or Path if sentence path failed to read
        };

        protected override void VerifyArgsAndRun()
        {
            // Validity check: CurrentLibrary must not be null
            if (REPL.Instance.CurrentLibrary is null) throw new ContextNotValidException("No library is currenty loaded.");

            // Case: Sentence not included in Args
            if (Arguments[1].Value is null)
            {
                // Validity check: CurrentSentence must not be null
                if (REPL.Instance.CurrentSentence is null) throw new ContextNotValidException("No sentence is current memorised, please provide one.");

                TryRenderSentence(REPL.Instance.CurrentSentence);
            }
            else
            {
                var sentence = REPL.Instance.CurrentLibrary.GetSentence((string)Arguments[1].Value!);;
                TryRenderSentence(sentence);

                // Memorise sentence
                REPL.Instance.CurrentSentence?.Dispose();
                REPL.Instance.CurrentSentence = sentence;
            }
        }

        private void TryRenderSentence(Sentence sentence)
        {
            var path = (string)Arguments[0].Value!;

            // Validity check: path directory must exist, or be empty
            var directoryName = Path.GetDirectoryName(path);
            if (directoryName != "" && !Directory.Exists(path)) throw new DirectoryNotFoundException();

            // Warn user about non-wav files
            if (!path.ToLower().EndsWith(".wav") && Terminal.GetUserApproval("Warning: specified path does not include '.wav' extension, this could cause the file to not be played properly.\nWould you like to automatically append '.wav' (y/n)?", ConsoleColor.Yellow))
            {
                // Append extension
                path += ".wav";
            }

            // Check for file exists and give option for overwriting
            if (File.Exists(path) && !Terminal.GetUserApproval("File already exists! Overwrite (y/n)?", ConsoleColor.DarkYellow))
            {
                // Cancel render on non-yes answer
                Terminal.MessageSingle("Render cancelled by user.", ConsoleColor.Red);
                return;
            }

            // Render file
            AudioEngine.AudioEngine.Instance.Render(sentence.GetAudioClip(), path);
            Terminal.MessageSingle($"Rendered to: {path}");
        }
    }
}
