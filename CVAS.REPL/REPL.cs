using CVAS.DataStructure;

namespace CVAS.REPL
{
    public class REPL
    {
        public static REPL Instance { get; } = new REPL();

        internal ICommand[] CommandInstances { get; } =
        {
            new HelpCommand(),
            new LoadCommand(),
            new PreviewCommand(),
            new SayCommand(),
            new RenderCommand(),
            new ExitCommand(),
        };

        public Library? CurrentLibrary { get; internal set; }

        public bool isRunning { get; internal set; } = false;

        private REPL() { } // Non-constructable

        public void Start()
        {
            isRunning = true;

            Console.WriteLine("----CVAS v0.5 REPL----"); // Write REPL header
            Console.WriteLine("Type 'help' for a list of commands.");
            Console.WriteLine();

            while (isRunning)
            {
                // Prompt user
                Console.Write(">> ");
                var command = Console.ReadLine();

                // Attempt to run command
                try
                {
                    RunFrom(command);
                }
                catch (CommandNotValidException)
                {
                    Console.WriteLine("Failed to execute: Command was not valid!"); // TODO: make this more detailed
                }
                catch (ArgumentNotValidException)
                {
                    Console.WriteLine("Failed to execute: Argument was not valid!"); // TODO: make this more detailed
                }
                catch (ContextNotValidException)
                {
                    Console.WriteLine("Failed to execute: Context was not valid! (Have you loaded a library yet?)");
                }
            }
        }

        internal void RunFrom(string str)
        {
            foreach (ICommand command in CommandInstances)
            {
                try
                {
                    command.RunFrom(str);
                    return; // Case: a valid command is found
                }
                catch (CommandNotValidException) { }
            }

            throw new CommandNotValidException(); // Case: no valid commands found
        }
    }
}
