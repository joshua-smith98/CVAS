using System.Text;

namespace CVAS.CommandLine
{
    /// <summary>
    /// Abstract class representing a command-line argument.
    /// </summary>
    internal abstract class CmdLnArgument
    {
        public abstract string Str { get; }
        public abstract string? ShortStr { get; }
        public abstract string[] DescriptionLines { get; }
        public abstract string Usage { get; }

        /// <summary>
        /// Tries to import a value from the given arguments into <see cref="CmdLnContext.Instance"/>, returning the trimmed args.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <exception cref="CmdLnStrNotValidException"/>
        /// <exception cref="CmdLnArgNotValidException"/>
        public string[] ImportFromAndTrim(string[] args)
        {
            VerifyStr(args);
            return ImportFromImpl(args);
        }

        private void VerifyStr(string[] args)
        {
            // Check args[0] with Str and ShortStr
            if (args[0].ToLower() != Str && args[0].ToLower() != ShortStr) throw new CmdLnStrNotValidException("Given option does not match Str or ShortStr.");
        }

        /// <summary>
        /// Attempts to read a string from the given args, taking into account double quotes (").
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <exception cref="CmdLnArgNotValidException"></exception>
        /// <exception cref="Exception"></exception>
        protected string ReadStringFromAndTrim(ref string[] args)
        {
            // Read a string from args[], taking into account double quotes (")
            // Convert string[] back into a string
            var strArgs = string.Join(' ', args);

            // Variation on implementation of StringArgument.cs

            // Case: str begins with double quotes
            if (strArgs[0] == '"')
            {
                // Build string from str contents
                StringBuilder valueBuilder = new();

                for (int i = 1; i < strArgs.Length; i++)
                {
                    if (strArgs[i] == '"') // Case: end of value
                    {
                        // Trim args and return
                        var trimmedStrArgs = strArgs[(i + 1)..].TrimStart();
                        if (trimmedStrArgs == string.Empty) args = []; // Handle case where string is empty: otherwise Split() will produce array with one element!
                        else args = trimmedStrArgs.Split();

                        // Check that string we made contains something
                        if (valueBuilder.ToString().Trim() == string.Empty) throw new CmdLnArgNotValidException($"Value provided for argument '{Str}|{ShortStr}' was empty.");

                        return valueBuilder.ToString();
                    }

                    valueBuilder.Append(strArgs[i]);

                    if (i == strArgs.Length - 1) throw new CmdLnArgNotValidException($"Expected second set of double-quotes in argument: '{Str}|{ShortStr}'");
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

                // Check that string contains something
                if (ret.Trim() == string.Empty) throw new CmdLnArgNotValidException($"Value provided for argument '{Str}|{ShortStr}' was empty.");

                return ret;
            }
        }

        protected abstract string[] ImportFromImpl(string[] args);
    }
}
