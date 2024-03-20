namespace CVAS.CommandLine
{
    internal abstract class CmdLnArgument
    {
        public abstract string Str { get; }
        public abstract string? ShortStr { get; }
        public abstract string Description { get; }

        public string[] ImportFromAndTrim(string[] args)
        {
            throw new NotImplementedException();
        }

        private void VerifyStr(string[] args)
        {
            // Check to see if Str or ShortStr matches
            throw new NotImplementedException();
        }

        protected abstract string[] ImportFromImpl(string[] args);
    }
}
