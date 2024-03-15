namespace CVAS.REPL
{
    /// <summary>
    /// Interface representing a REPL command which performs an action, optionally taking arguments as input.
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

        public abstract Command[] SubCommand { get; } // For potential future use - not yet implemented

        /// <summary>
        /// The list of argument instances which need to be given for this command to work.
        /// </summary>
        public abstract Argument[] Arguments { get; }

        /// <summary>
        /// Validates and attempts to perform this command's action, using the given string.
        /// </summary>
        /// <param name="str"></param>
        public abstract void RunFrom(string str);
    }
}
