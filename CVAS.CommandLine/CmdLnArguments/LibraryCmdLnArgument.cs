using CVAS.FileSystem;

namespace CVAS.CommandLine
{
    internal class LibraryCmdLnArgument : CmdLnArgument
    {
        public override string Str => "-library";

        public override string? ShortStr => "-l";

        public override string Description => "Loads the library at the given directory.";

        protected override string[] ImportFromImpl(string[] args)
        {
            // Get path from args
            var ret = args[1..];
            var path = ReadStringFromAndTrim(ref ret);

            // Try to load library
            try
            {
                CmdLnContext.Instance.Library = LibraryFolder.LoadFrom(path).Construct();
            }
            catch (DirectoryNotFoundException)
            {
                throw new CmdLnArgNotValidException($"Failed to load library. Couldn't find directory at: {path}");
            }

            // return trimmed args
            return ret;
        }
    }
}
