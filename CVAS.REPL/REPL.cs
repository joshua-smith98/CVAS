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
                if (instance is null) throw new NullReferenceException();
                else return instance;
            }
        }
        private static REPL? instance;

        /// <summary>
        /// A list of all top-level commands that can be used in the REPL.
        /// </summary>
        internal Command[] CommandInstances { get; } =
        {
            new HelpCommand(),
            new LoadCommand(),
            new TestCommand(),
            new SayCommand(),
            new RenderCommand(),
            new StopCommand(),
            new ClearCommand(),
            new ExitCommand(),
        };

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

        public static void Init()
        {
            // Check if already initialised
            if (instance is not null) throw new Exception("REPL cannot be initialised twice!");
            instance = new REPL();
        }

        /// <summary>
        /// Begins the REPL loop. Set <see cref="IsRunning"/> = <see cref="false"/> to end.
        /// </summary>
        public void Start()
        {
            IsRunning = true;

            // Write REPL header
            Terminal.BeginMessage();
            Terminal.Message("----CVAS v0.5.0 REPL----");
            Terminal.Message("Type 'help' for a list of commands.");
            Terminal.EndMessage();

            // Main REPL loop
            while (IsRunning)
            {
                // Prompt user
                var command = Terminal.Prompt(">> ");

                // Attempt to run command, and handle exceptions.
                try
                {
                    RunFrom(command); // Since we're reading from the console, command will never be null
                }
                catch (CommandNotValidException e) // TODO: this can be simplified with REPLException
                {
                    Terminal.MessageSingle(e.Message, ConsoleColor.Red);
                }
                catch (ArgumentNotValidException e)
                {
                    Terminal.MessageSingle(e.Message, ConsoleColor.Red);
                }
                catch (ContextNotValidException e)
                {
                    Terminal.MessageSingle(e.Message, ConsoleColor.Red);
                }
            }
        }

        /// <summary>
        /// Attempts to run the commands in the given string. Throws <see cref="CommandNotValidException"/> if str contains no valid commands.
        /// </summary>
        /// <param name="str"></param>
        /// <exception cref="CommandNotValidException"></exception>
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
                catch (CommandNotValidException) { } // Case: the current command is not valid
            }

            throw new CommandNotValidException($"Command '{str.Split().First()}' does not exist!"); // Case: no valid commands found
        }
    }
}
