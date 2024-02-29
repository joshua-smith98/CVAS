using CVAS.DataStructure;

namespace CVAS.REPL
{
    public class REPL
    {
        public static REPL Instance { get; } = new REPL();

        internal ICommand[] CommandInstances { get; } =
        {
            new ExitCommand(),
            new HelpCommand(),
            new LoadCommand(),
            new PreviewCommand(),
            new RenderCommand(),
            new SpeakCommand(),
        };

        public Library? CurrentLibrary { get; internal set; }

        public bool isRunning { get; internal set; } = false;

        private REPL() { } // Non-constructable

        public void Start()
        {
            // REPL logic
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
