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

            while (isRunning)
            {
                // Prompt user
                Console.Write(">> ");
                var command = Console.ReadLine();

                // Attempt to run command

                RunFrom(command); // For testing purposes, we will leave this bare for now

                /*
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
                }*/
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
