using CVAS.AudioEngineNS;
using CVAS.Core;
using CVAS.TerminalNS;

namespace CVAS.REPL
{
    /// <summary>
    /// A <see cref="Command"/> that prints a list of a given sentence's subphrases.
    /// </summary>
    internal class TestCommand : Command
    {
        public override string Str => "test";

        public override string Description => "Displays the given sentence and any phrases that couldn't be found. Also commits that sentence to memory.";

        public override string[] Usage { get; } = ["test [sentence]", "test"];

        public override Command[] SubCommands { get; } = [];

        public override Argument[] Arguments { get; } =
        [
            new StringArgument("sentence", false),
        ];

        protected override void VerifyArgsAndRun()
        {
            // Validity check: CurrentLibrary must not be null
            if (REPL.Instance.CurrentLibrary is null) throw new ContextNotValidException($"No library is currently loaded.");

            // Case: sentence has not been given
            if (Arguments[0].Value is null)
            {
                // Validity check: CurrentSentence must not be null
                if (REPL.Instance.CurrentSentence is null) throw new ContextNotValidException($"No sentence is currently memorised, please provide one.");

                // Print memorised sentence
                PrintSentencePreview(REPL.Instance.CurrentSentence);
            }
            else
            {
                // Get sentence and print phrases & inflections.
                var sentence_str = (string)Arguments[0].Value!;
                var sentence = REPL.Instance.CurrentLibrary.GetSentence(sentence_str);

                PrintSentencePreview(sentence);

                // Memorise Sentence
                REPL.Instance.CurrentSentence?.Dispose();
                REPL.Instance.CurrentSentence = sentence;
                Terminal.MessageSingle("Sentence was committed to memory.", ConsoleColor.Yellow);
            }
        }

        /// <summary>
        /// Prints a formatted preview of the given sentence to the console.
        /// </summary>
        /// <param name="sentence"></param>
        private static void PrintSentencePreview(Sentence sentence)
        {
            Terminal.BeginMessage();

            // Print phrases
            foreach (var phrase in sentence.SpokenPhrases.Where(x => !x.IsSpecialPhrase()))
            {
                // Case: current Phrase is empty (i.e. it couldn't be found)
                if (phrase.IsEmptyPhrase())
                {
                    Terminal.Message($"[{phrase.Str}] : {phrase.Inflection}", ConsoleColor.Red); // Print empty phrases in red
                    continue;
                }

                // Case: current Phrase is not empty
                Terminal.Message($"[{phrase.Str}] : {phrase.Inflection}");

                // Print a new line after phrases that end a sentence.
                if (phrase != sentence.SpokenPhrases.Where(x => !x.IsSpecialPhrase()).Last() && phrase.Inflection is InflectionType.End)
                    Terminal.Message();
            }

            Terminal.EndMessage();
        }
    }
}
