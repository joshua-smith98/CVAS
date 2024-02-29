namespace CVAS.REPL
{
    /// <summary>
    /// Interface representing a REPL command which performs an action, optionally taking arguments as input.
    /// </summary>
    internal interface ICommand
    {
        /// <summary>
        /// The string used to call the command in the REPL
        /// </summary>
        public string Str { get; }

        /// <summary>
        /// A description of the command's function. Used in the 'help' command.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// An example of the command's usage. Used in the 'help' command.
        /// </summary>
        public string Usage { get; }

        public ICommand? SubCommand { get; } // For potential future use - not yet implemented

        /// <summary>
        /// The list of argument instances which need to be given for this command to work.
        /// </summary>
        public IArgument[] Arguments { get; }

        /// <summary>
        /// Validates and attempts to perform this command's action, using the given string.
        /// </summary>
        /// <param name="str"></param>
        public void RunFrom(string str);
    }
}
