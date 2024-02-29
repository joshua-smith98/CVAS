using CVAS.AudioEngine;

namespace CVAS.REPL
{
    internal class PreviewCommand : ICommand
    {
        public string Str => "preview";

        public string Description => "Displays the phrases that would be used to speak a given sentence, using the currently loaded library.";

        public string Usage => "preview \"[sentence to preview]\"";

        public ICommand? SubCommand { get; }

        public IArgument[] Arguments { get; } =
        {
            new StringArgument(),
        };

        internal PreviewCommand() { }

        public void RunFrom(string str)
        {
            // Validity check: str must begin with "load"
            if (!str.StartsWith(Str)) throw new CommandNotValidException();

            // Try to read arguments
            var temp_str = str.Substring(Str.Length).TrimStart();

            foreach (IArgument argument in Arguments) argument.ReadFrom(ref temp_str); // If this fails, an ArgumentNotValidException will be thrown, then caught by the REPL class.

            // Validity check: str must be empty after all arguments are read
            if (temp_str != "") throw new CommandNotValidException();

            // Run Command
            var sentence_str = Arguments[0].Value as string;
            var sentence = REPL.Instance.CurrentLibrary.GetSentence(sentence_str); // TODO: Handle CurrentLibrary being null

            foreach (var phrase in sentence.spokenPhrases)
            {
                Console.WriteLine($"[{phrase.Str}] : {phrase.Inflection.ToString()}");
            }
        }
    }
}
