namespace CVAS.CommandLine
{
    /// <summary>
    /// A <see cref="CmdLnArgument"/> that provides an output path for rendering to the <see cref="CmdLnContext"/>.
    /// </summary>
    internal class OutputCmdLnArgument : CmdLnArgument
    {
        public override string Str => "-output";

        public override string? ShortStr => "-o";

        public override string[] DescriptionLines { get; } =
        [
            "Specifies an output file for the '-render' option.",
        ];

        public override string Usage => "-output | -o [output path]";

        protected override string[] ImportFromImpl(string[] args)
        {
            // Verify: args has more than just Str
            if (args.Length == 1) throw new CmdLnArgNotValidException($"Expected output path after '{args[0]}', found nothing!");

            // Get path
            var ret = args[1..];
            var path = ReadStringFromAndTrim(ref ret);

            // Verify: directory exists
            var directoryName = Path.GetDirectoryName(path);
            if (directoryName != "" && !Directory.Exists(path)) throw new CmdLnArgNotValidException($"Directory not found: {directoryName}");

            // Save path to CmdLnContext and return trimmed args
            CmdLnContext.Instance.OutputPath = path;
            return ret;
        }
    }
}
