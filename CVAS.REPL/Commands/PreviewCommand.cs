using CVAS.AudioEngine;
using CVAS.TerminalInterface;

namespace CVAS.REPL
{
    /// <summary>
    /// An <see cref="ICommand"/> that prints a list of a given sentence's subphrases.
    /// </summary>
    internal class PreviewCommand : ICommand
    {
        public string Str => "preview";

        public string Description => "Displays the phrases that would be used to speak a given sentence, using the currently loaded library.";

        public string[] Usage { get; } = { "preview [sentence]" };

        public ICommand? SubCommand { get; }

        public IArgument[] Arguments { get; } =
        {
            new StringArgument("sentence"),
        };

        internal PreviewCommand() { }

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
            if (REPL.Instance.CurrentLibrary is null) throw new ContextNotValidException($"No library is currently loaded.");

            // Get sentence and print phrases & inflections.
            var sentence_str = Arguments[0].Value as string;
            var sentence = REPL.Instance.CurrentLibrary.GetSentence(sentence_str);

            Terminal.BeginMessage();

            foreach (var phrase in sentence.spokenPhrases)
            {
                Terminal.Message($"[{phrase.Str}] : {phrase.Inflection}");
            }

            Terminal.EndMessage();
        }
    }
}
