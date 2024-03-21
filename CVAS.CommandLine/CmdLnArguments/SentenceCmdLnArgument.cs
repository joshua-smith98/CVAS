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

            // Save sentenceStr to CmdLnContext and return trimmed args
            CmdLnContext.Instance.SentenceStr = sentenceStr;
            return ret;
        }
    }
}
