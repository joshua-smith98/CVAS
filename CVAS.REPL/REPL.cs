using CVAS.Core;

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
        public static REPL Instance { get; } = new REPL();

        /// <summary>
        /// A list of all top-level commands that can be used in the REPL.
        /// </summary>
        internal ICommand[] CommandInstances { get; } =
        {
            new HelpCommand(),
            new LoadCommand(),
            new PreviewCommand(),
            new SayCommand(),
            new RenderCommand(),
            new ClearCommand(),
            new ExitCommand(),
        };

        public Library? CurrentLibrary { get; internal set; }

        public bool isRunning { get; internal set; } = false;

        private REPL() { } // Non-constructable

        /// <summary>
        /// Begins the REPL loop. Set <see cref="REPL.isRunning"/> = <see cref="false"/> to end.
        /// </summary>
        public void Start()
        {
            isRunning = true;

            // Write REPL header
            Console.WriteLine("----CVAS v0.5 REPL----");
            Console.WriteLine("Type 'help' for a list of commands.");
            Console.WriteLine();

            // Main REPL loop
            while (isRunning)
            {
                // Prompt user
                Console.Write(">> ");
                var command = Console.ReadLine();

                // Attempt to run command, and handle exceptions.
                try
                {
                    RunFrom(command); // Since we're reading from the console, command will never be null
                }
                catch (CommandNotValidException)
                {
                    Console.WriteLine("Failed to execute: Command was not valid!"); // TODO: make this more detailed
                    Console.WriteLine();
                }
                catch (ArgumentNotValidException)
                {
                    Console.WriteLine("Failed to execute: Argument was not valid!"); // TODO: make this more detailed
                    Console.WriteLine();
                }
                catch (ContextNotValidException)
                {
                    Console.WriteLine("Failed to execute: Context was not valid! (Have you loaded a library yet?)");
                    Console.WriteLine();
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
            foreach (ICommand command in CommandInstances)
            {
                try
                {
                    command.RunFrom(str);
                    return; // Case: a valid command is found
                }
                catch (CommandNotValidException) { } // Case: the current command is not valid
            }

            throw new CommandNotValidException(); // Case: no valid commands found
        }
    }
}
