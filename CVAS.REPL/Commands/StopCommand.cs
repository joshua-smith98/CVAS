namespace CVAS.REPL
{
    /// <summary>
    /// An <see cref="Command"/> that stops all audio playback.
    /// </summary>
    internal class StopCommand : Command
    {
        public string Str => "stop";

        public string Description => "Stops all audio playback.";

        public string[] Usage { get; } = { "stop" };

        public Command? SubCommand { get; }

        public Argument[] Arguments { get; } = { };

        internal StopCommand() { }

        public void RunFrom(string str)
        {
            // Validity check: str must begin with "load"
            if (!str.StartsWith(Str)) throw new CommandNotValidException("Command does not match. This message should never be printed - if it was, open an issue on Github!");

            // Try to read arguments
            var temp_str = str[Str.Length..].TrimStart();

            foreach (Argument argument in Arguments)
            {
                argument.ReadFrom(ref temp_str); // If this fails, an ArgumentNotValidException will be thrown, then caught by the REPL class.
            }

            // Validity check: str must be empty after all arguments are read
            if (temp_str != "") throw new ArgumentNotValidException($"Expected end of command, found: '{temp_str}'!");

            AudioEngine.AudioEngine.Instance.StopAll();
        }
    }
}
