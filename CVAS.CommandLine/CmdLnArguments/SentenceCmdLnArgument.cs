namespace CVAS.CommandLine
{
    internal class SentenceCmdLnArgument : CmdLnArgument
    {
        public override string Str => "-sentence";

        public override string? ShortStr => "-s";

        public override string Description => "Provides a sentence to speak using the currently loaded library.";

        protected override string[] ImportFromImpl(string[] args)
        {
            // Get sentence
            var ret = args[1..];
            var sentenceStr = ReadStringFromAndTrim(ref ret);

            // Verify: Context.Library must exist!
            if (CmdLnContext.Instance.Library is null) throw new CmdLnContextNotValidException("Cannot provide a sentence with no library loaded!");

            // Save sentence and return trimmed args
            CmdLnContext.Instance.Sentence = CmdLnContext.Instance.Library.GetSentence(sentenceStr);
            return ret;
        }
    }
}
