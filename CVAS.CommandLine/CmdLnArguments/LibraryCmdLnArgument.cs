using CVAS.FileSystem;

namespace CVAS.CommandLine
{
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
            // Get path from args
            var ret = args[1..];
            var path = ReadStringFromAndTrim(ref ret);

            // Save path to CmdLnContext and return trimmed args
            CmdLnContext.Instance.LibraryPath = path;
            return ret;
        }
    }
}
