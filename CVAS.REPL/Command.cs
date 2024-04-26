namespace CVAS.REPL
{
    /// <summary>
    /// Represents a REPL command which performs an action, optionally taking arguments as input.
    /// </summary>
    internal abstract class Command
    {
        /// <summary>
        /// The string used to call the command in the REPL
        /// </summary>
        public abstract string Str { get; }

        /// <summary>
        /// A description of the command's function. Used in the 'help' command.
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Examples of the command's usage. Used in the 'help' command.
        /// </summary>
        public abstract string[] Usage { get; }

        public abstract Command[] SubCommands { get; } // For potential future use - not yet implemented

        /// <summary>
        /// The collection of argument instances which need to be given for this command to work.
        /// </summary>
        public abstract Argument[] Arguments { get; }

        /// <summary>
        /// Validates and attempts to perform this command's action, using the given string.
        /// </summary>
        /// <param name="str"></param>
        public void RunFrom(string str)
        {
            VerifyStr(str);
            ReadArguments(str);
            VerifyArgsAndRun();
        }

        /// <summary>
        /// Verifies that the given string calls this command.
        /// </summary>
        /// <param name="str"></param>
        /// <exception cref="CommandReadFailedException"></exception>
        private void VerifyStr(string str)
        {
            // Validity check: str must begin with "Str"
            if (!str.StartsWith(Str)) throw new CommandReadFailedException("Command does not match. This message should never be printed - if it was, open an issue on Github!");
        }

        /// <summary>
        /// Reads the arguments from the given string into this Command's <see cref="Arguments"/>.
        /// </summary>
        /// <param name="str"></param>
        /// <exception cref="CommandRunFailedException"></exception>
        private void ReadArguments(string str)
        {
            // Try to read arguments
            var temp_str = str[Str.Length..].TrimStart();

            foreach (Argument argument in Arguments) temp_str = argument.ReadFrom(temp_str); // If this fails, an ArgumentNotValidException will be thrown, then caught by the REPL class.

            // Validity check: str must be empty after all arguments are read
            if (temp_str != "") throw new CommandRunFailedException($"Expected end of command, found: '{temp_str}'!");
        }

        /// <summary>
        /// Abstract method that performs the actions associated with the Command, and/or any additional validations.
        /// </summary>
        protected abstract void VerifyArgsAndRun();
    }
}
