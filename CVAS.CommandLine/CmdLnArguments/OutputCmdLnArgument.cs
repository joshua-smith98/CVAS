namespace CVAS.CommandLine
{
    internal class OutputCmdLnArgument : CmdLnArgument
    {
        public override string Str => "-output";

        public override string? ShortStr => "-o";

        public override string[] DescriptionLines { get; } =
        {
            "Specifies an output file for the '-render' option.",
        };

        public override string Usage => "-output | -o [output path]";

        protected override string[] ImportFromImpl(string[] args)
        {
            // Get path
            var ret = args[1..];
            var path = ReadStringFromAndTrim(ref ret);

            // Verify: directory exists
            if (!Directory.Exists(path)) throw new CmdLnArgNotValidException($"Couldn't find directory: {Path.GetDirectoryName(path)}");

            // Save path to CmdLnContext and return trimmed args
            CmdLnContext.Instance.OutputPath = path;
            return ret;
        }
    }
}
