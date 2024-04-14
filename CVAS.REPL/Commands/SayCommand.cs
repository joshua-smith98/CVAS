using CVAS.AudioEngineNS;
using CVAS.TerminalNS;

namespace CVAS.REPL
{
    /// <summary>
    /// A <see cref="Command"/> that attempts to 'say' the given sentence.
    /// </summary>
    internal class SayCommand : Command
    {
        public override string Str => "say";

        public override string Description => "Says the given sentence, and commits it to memory.";

        public override string[] Usage { get; } = ["say [sentence]", "say"];

        public override Command[] SubCommands { get; } = [];

        public override Argument[] Arguments { get; } =
        [
            new StringArgument("sentence", false),
        ];

        protected override void VerifyArgsAndRun()
        {
            // Validity check: CurrentLibrary must not be null
            if (REPL.Instance.CurrentLibrary is null) throw new ContextNotValidException("No library is currently loaded.");

            // Case: sentence has not been given
            if (Arguments[0].Value is null)
            {
                // Validity check: CurrentSentence must not be null
                if (REPL.Instance.CurrentSentence is null) throw new ContextNotValidException($"No sentence is currently memorised, please provide one.");

                // Play memorised sentence
                AudioEngine.Instance.Play(REPL.Instance.CurrentSentence.GetAudioClip());
            }
            else
            {
                // Get sentence and play
                var sentence_str = (string)Arguments[0].Value!;
                var sentence = REPL.Instance.CurrentLibrary.GetSentence(sentence_str);

                AudioEngine.Instance.Play(sentence.GetAudioClip());

                // Memorise sentence
                REPL.Instance.CurrentSentence?.Dispose();
                REPL.Instance.CurrentSentence = sentence;
                Terminal.Instance.MessageSingle("Sentence was committed to memory.", ConsoleColor.Yellow);
            }
        }
    }
}
