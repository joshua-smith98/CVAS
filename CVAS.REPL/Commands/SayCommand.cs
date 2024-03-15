namespace CVAS.REPL
{
    /// <summary>
    /// An <see cref="Command"/> that attempts to 'say' the given sentence.
    /// </summary>
    internal class SayCommand : Command
    {
        public override string Str => "say";

        public override string Description => "Says the given sentence, using the currently loaded library.";

        public override string[] Usage { get; } = { "say [sentence]" };

        public override Command[] SubCommands { get; } = { };

        public override Argument[] Arguments { get; } =
        {
            new StringArgument("sentence", false),
        };

        protected override void VerifyArgsAndRun()
        {
            // Validity check: CurrentLibrary must not be null
            if (REPL.Instance.CurrentLibrary is null) throw new ContextNotValidException("No library is currently loaded.");

            // Case: sentence has not been given
            if (Arguments[0].Value is null)
            {
                // Validity check: CurrentSentence must not be null
                if (REPL.Instance.CurrentSentence is null) throw new ContextNotValidException($"No sentence is currently memorised, please provide one.");

                AudioEngine.AudioEngine.Instance.Play(REPL.Instance.CurrentSentence.GetAudioClip());
            }

            // Get sentence and print phrases & inflections.
            var sentence_str = (string)Arguments[0].Value!;
            var sentence = REPL.Instance.CurrentLibrary.GetSentence(sentence_str);

            AudioEngine.AudioEngine.Instance.Play(sentence.GetAudioClip());
        }
    }
}
