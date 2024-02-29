using CVAS.DataStructure;

namespace CVAS.REPL
{
    public class REPL
    {
        public static REPL Instance { get; } = new REPL();

        internal ICommand[] CommandInstances { get; } =
        {

        };

        public Library? CurrentLibrary { get; internal set; }

        private REPL() { } // Non-constructable

        public void Start()
        {
            // REPL logic
        }

        internal void RunFrom(ref string str)
        {
            foreach (ICommand command in CommandInstances)
            {
                try
                {
                    command.RunFrom(ref str);
                    return; // Case: a valid command is found
                }
                catch (CommandNotValidException) { }
            }

            throw new CommandNotValidException(); // Case: no valid commands found
        }
    }
}
