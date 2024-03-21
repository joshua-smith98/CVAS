using CVAS.FileSystem;

namespace CVAS.CommandLine
{
    /// <summary>
    /// A <see cref="CmdLnArgument"/> the provides a library to the <see cref="CmdLnContext"/>.
    /// </summary>
    internal class LibraryCmdLnArgument : CmdLnArgument
    {
        public override string Str => "-library";

        public override string? ShortStr => "-l";

        public override string[] DescriptionLines { get; } =
        {
            "Provides a directory to load a library from.",
        };

        public override string Usage => "-library | -l [path to library]";

        protected override string[] ImportFromImpl(string[] args)
        {
            // Verify: args has more than just Str
            if (args.Length == 1) throw new CmdLnArgNotValidException($"Expected path to library after '{args[0]}', found nothing!");

            // Get path from args
            var ret = args[1..];
            var path = ReadStringFromAndTrim(ref ret);

            // Save path to CmdLnContext and return trimmed args
            CmdLnContext.Instance.LibraryPath = path;
            return ret;
        }
    }
}
