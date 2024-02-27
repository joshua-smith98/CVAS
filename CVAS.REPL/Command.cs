namespace CVAS.REPL
{
    public static class Command
    {
        public static readonly ICommand[] Commands =
        {

        };

        public static void RunFrom(ref string str)
        {
            foreach (ICommand command in Commands)
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
