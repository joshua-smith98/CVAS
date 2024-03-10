namespace CVAS.REPL
{
    /// <summary>
    /// An <see cref="ICommand"/> that either prints the description and usage for a given command, or lists all commands.
    /// </summary>
    internal class HelpCommand : ICommand
    {
        public string Str => "help";

        public string Description => "Gets the description and usage instructions for the given command, or lists all commands.";

        public string[] Usage { get; } = { "help", "help [command]" };

        public ICommand? SubCommand { get; }

        public IArgument[] Arguments { get; } =
        {
            new CommandArgument("command"),
        };

        internal HelpCommand() { }

        public void RunFrom(string str)
        {
            // Validity check: str must begin with "load"
            if (!str.StartsWith(Str)) throw new CommandNotValidException("Command does not match. This message should never be printed - if it was, open an issue on Github!");

            // Case: there is no given argument for [command]
            if (str.Length == Str.Length)
            {
                // Print command list
                Console.WriteLine();
                foreach(ICommand command in REPL.Instance.CommandInstances)
                {
                    Console.WriteLine($"{command.Str}:");
                    Console.WriteLine($"\t{command.Description}");

                    // Print use cases
                    foreach(string useCase in command.Usage)
                    {
                        Console.WriteLine($"\t{useCase}");
                    }
                    Console.WriteLine();
                }
            }
            else // Case: there is one or more argument given
            {
                // Try to read arguments
                var temp_str = str.Substring(Str.Length).TrimStart();

                foreach (IArgument argument in Arguments)
                {
                    argument.ReadFrom(ref temp_str); // If this fails, an ArgumentNotValidException will be thrown, then caught by the REPL class.
                }
                // Validity check: str must be empty after all arguments are read
                if (temp_str != "") throw new ArgumentNotValidException($"Expected end of command, found: '{temp_str}'!");

                // Get command and print details
                var command = Arguments[0].Value as ICommand;

                Console.WriteLine();
                Console.WriteLine($"{command.Str}:");
                Console.WriteLine($"\t{command.Description}");

                // Print use cases
                foreach (string useCase in command.Usage)
                {
                    Console.WriteLine($"\t{useCase}");
                }
                Console.WriteLine();
            }
        }
    }
}
