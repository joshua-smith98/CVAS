namespace CVAS.CommandLine
{
    internal class SentenceCmdLnArgument : CmdLnArgument
    {
        public override string Str => "-sentence";

        public override string? ShortStr => "-s";

        public override string[] DescriptionLines { get; } =
        {
            "Provides a sentence to speak using the currently loaded library.",
        };

        public override string Usage => "-sentence | -s [sentence]";

        protected override string[] ImportFromImpl(string[] args)
        {
            // Verify: args has more than just Str
            if (args.Length == 1) throw new CmdLnArgNotValidException($"Expected sentence after '{args[0]}', found nothing!");

            // Get sentence
            var ret = args[1..];
            var sentenceStr = ReadStringFromAndTrim(ref ret);

            // Save sentenceStr to CmdLnContext and return trimmed args
            CmdLnContext.Instance.SentenceStr = sentenceStr;
            return ret;
        }
    }
}
