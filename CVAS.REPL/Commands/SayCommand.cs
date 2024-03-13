namespace CVAS.REPL
{
    /// <summary>
    /// An <see cref="ICommand"/> that attempts to 'say' the given sentence.
    /// </summary>
    internal class SayCommand : ICommand
    {
        public string Str => "say";

        public string Description => "Says the given sentence, using the currently loaded library.";

        public string[] Usage { get; } = { "say [sentence]" };

        public ICommand? SubCommand { get; }

        public IArgument[] Arguments { get; } =
        {
            new StringArgument("sentence"),
        };

        internal SayCommand() { }

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
            if (REPL.Instance.CurrentLibrary is null) throw new ContextNotValidException("No library is currently loaded.");

            // Run Command
            var sentence_str = (string)Arguments[0].Value!;
            var sentence = REPL.Instance.CurrentLibrary.GetSentence(sentence_str);

            AudioEngine.AudioEngine.Instance.Play(sentence.GetAudioClip());
        }
    }
}
