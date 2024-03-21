namespace CVAS.CommandLine
{
    internal abstract class CmdLnArgument
    {
        public abstract string Str { get; }
        public abstract string? ShortStr { get; }
        public abstract string Description { get; }

        public string[] ImportFromAndTrim(string[] args)
        {
            VerifyStr(args);
            return ImportFromImpl(args);
        }

        private void VerifyStr(string[] args)
        {
            // Check that args[0] begins with a dash '-'
            if (args[0][0] != '-') throw new CmdLnStrNotValidException("Given option does not begin with '-'.");

            // Trim dash from args[0]
            var trimmedArg = args[0][1..].ToLower();

            // Check trimmed args[0] to Str and ShortStr
            if (trimmedArg != Str && trimmedArg != ShortStr) throw new CmdLnStrNotValidException("Given option does not match Str or ShortStr.")
        }

        protected abstract string[] ImportFromImpl(string[] args);
    }
}
