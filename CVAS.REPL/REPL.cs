using CVAS.Core;
using CVAS.TerminalNS;

namespace CVAS.REPL
{
    /// <summary>
    /// Contains context data and main methods for the REPL application. Non-constructable: access via <see cref="REPL.Instance"/>.
    /// </summary>
    public class REPL
    {
        /// <summary>
        /// Static instance of the REPL application.
        /// </summary>
        public static REPL Instance
        {
            get
            {
                instance ??= new REPL();
                return instance;
            }
        }
        private static REPL? instance;

        /// <summary>
        /// A list of all top-level commands that can be used in the REPL.
        /// </summary>
        internal Command[] CommandInstances { get; } =
        [
            new HelpCommand(),
            new LoadCommand(),
            new TestCommand(),
            new SayCommand(),
            new RenderCommand(),
            new StopCommand(),
            new ClearCommand(),
            new ExitCommand(),
        ];

        /// <summary>
        /// The currently loaded library for this REPL.
        /// </summary>
        public Library? CurrentLibrary { get; internal set; }
        /// <summary>
        /// The currently memorised sentence for this REPL.
        /// </summary>
        public Sentence? CurrentSentence { get; internal set; }

        public bool IsRunning { get; internal set; } = false;

        private REPL() { } // Non-constructable

        /// <summary>
        /// Begins the REPL loop. Set <see cref="IsRunning"/> = <see cref="false"/> to end.
        /// </summary>
        public void Start()
        {
            IsRunning = true;

            // Write REPL header
            Terminal.Instance.BeginMessage();
            Terminal.Instance.Message("----CVAS v0.6.0 REPL----");
            Terminal.Instance.Message("Type 'help' for a list of commands.");
            Terminal.Instance.EndMessage();

            // Main REPL loop
            while (IsRunning)
            {
                // Prompt user
                var command = Terminal.Instance.Prompt(">> ");

                // Attempt to run command, and handle exceptions.
                try
                {
                    RunFrom(command); // Since we're reading from the console, command will never be null
                }
                catch (REPLException e)
                {
                    // Don't print error message if user typed nothing
                    if (e is CommandReadFailedException && e.Message == "EMPTYSTR")
                        continue;
                    
                    Terminal.Instance.MessageSingle(e.Message, ConsoleColor.Red);
                }
            }
        }

        /// <summary>
        /// Attempts to run the commands in the given string. Throws <see cref="CommandReadFailedException"/> if str contains no valid commands.
        /// </summary>
        /// <param name="str"></param>
        /// <exception cref="CommandReadFailedException"></exception>
        internal void RunFrom(string str)
        {
            // Iterate over all commands and check validity
            foreach (Command command in CommandInstances)
            {
                try
                {
                    command.RunFrom(str);
                    return; // Case: a valid command is found
                }
                catch (CommandReadFailedException) { } // Case: the current command is not valid
            }

            if (str == string.Empty)
                throw new CommandReadFailedException("EMPTYSTR"); // Throw special exception if the user doesn't provide any data -> we won't display a message
            else
                throw new CommandReadFailedException($"Command '{str.Split().First()}' does not exist!"); // Case: no valid commands found
        }
    }
}
