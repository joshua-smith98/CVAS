using CVAS.AudioEngine;
using CVAS.Core;
using CVAS.TerminalNS;

namespace CVAS.REPL
{
    /// <summary>
    /// An <see cref="Command"/> that prints a list of a given sentence's subphrases.
    /// </summary>
    internal class PreviewCommand : Command
    {
        public override string Str => "preview";

        public override string Description => "Displays the phrases that would be used to speak a given sentence, and commits that sentence to memory.";

        public override string[] Usage { get; } = { "preview [sentence]", "preview" };

        public override Command[] SubCommands { get; } = { };

        public override Argument[] Arguments { get; } =
        {
            new StringArgument("sentence", false),
        };

        protected override void VerifyArgsAndRun()
        {
            // Validity check: CurrentLibrary must not be null
            if (REPL.Instance.CurrentLibrary is null) throw new ContextNotValidException($"No library is currently loaded.");

            // Case: sentence has not been given
            if (Arguments[0].Value is null)
            {
                // Validity check: CurrentSentence must not be null
                if (REPL.Instance.CurrentSentence is null) throw new ContextNotValidException($"No sentence is currently memorised, please provide one.");

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
            }
        }

        private void PrintSentencePreview(Sentence sentence)
        {
            Terminal.BeginMessage();

            foreach (var phrase in sentence.spokenPhrases.Where(x => !x.IsSpecialPhrase()))
            {
                // Print NULL phrases (the ones that couldn't be found) in red
                if (phrase.IsEmptyPhrase())
                {
                    Terminal.Message($"[{phrase.Str}] : {phrase.Inflection}", ConsoleColor.Red);
                    continue;
                }

                Terminal.Message($"[{phrase.Str}] : {phrase.Inflection}");

                // New line at the end of every sentence.
                if (phrase != sentence.spokenPhrases.Where(x => !x.IsSpecialPhrase()).Last() && phrase.Inflection is InflectionType.End) Terminal.Message();
            }

            Terminal.EndMessage();
        }
    }
}
