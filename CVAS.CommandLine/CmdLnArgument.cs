using System.Text;

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
            if (trimmedArg != Str && trimmedArg != ShortStr) throw new CmdLnStrNotValidException("Given option does not match Str or ShortStr.");
        }

        protected static string ReadStringFrom(ref string[] args)
        {
            // Read a string from args[], taking into account double quotes (")
            // Convert string[] back into a string
            var strArgs = string.Join(' ', args);

            // Variation on implementation of StringArgument.cs

            // Case: str begins with double quotes
            if (strArgs[0] == '"')
            {
                // Build string from str contents
                StringBuilder valueBuilder = new StringBuilder();

                for (int i = 1; i < strArgs.Length; i++)
                {
                    if (strArgs[i] == '"') // Case: end of value
                    {
                        // Trim args and return
                        args = strArgs[(i + 1)..].Split();
                        return valueBuilder.ToString();
                    }

                    valueBuilder.Append(strArgs[i]);

                    if (i == strArgs.Length - 1) throw new CmdLnArgNotValidException($"Expected second set of double-quotes in argument!"); // TODO: make this message a bit more helpful
                }

                // We should never reach this place
                throw new Exception("Something has gone horribly wrong...");
            }
            else // Case: str doesn't begin with quotes
            {
                // We don't need to build a string here, as we already have args split into words
                // We'll just return the first element in args, and then trim args
                var ret = args[0];
                args = args[1..];
                return ret;
            }
        }

        protected abstract string[] ImportFromImpl(string[] args);
    }
}
