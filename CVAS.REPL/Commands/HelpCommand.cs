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
            new CommandArgument(),
        };

        internal HelpCommand() { }

        public void RunFrom(string str)
        {
            // Validity check: str must begin with "load"
            if (!str.StartsWith(Str)) throw new CommandNotValidException();

            // Case: there is no given argument for [command]
            if (str.Length == Str.Length)
            {
                Console.WriteLine();

                // Print list of commands
                foreach (var command in REPL.Instance.CommandInstances)
                {
                    // Print str
                    Console.WriteLine($"{command.Str}:");

                    // Print description
                    Console.WriteLine("\tDescription:");
                    foreach (var line in command.Description.Split('\n'))
                    {
                        Console.WriteLine($"\t\t{line}");
                    }

                    // Print usage
                    Console.WriteLine("\tUsage:");
                    foreach (var line in command.Usage.Split('\n'))
                    {
                        Console.WriteLine($"\t\t{line}");
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
                if (temp_str != "") throw new ArgumentNotValidException();

                // Get command and print details
                var command = Arguments[0].Value as ICommand;

                Console.WriteLine();

                // Print str
                Console.WriteLine($"{command.Str}:"); // command should never be null at this point

                // Print description
                Console.WriteLine("\tDescription:");
                foreach (var line in command.Description.Split('\n'))
                {
                    Console.WriteLine($"\t\t{line}");
                }

                // Print usage
                Console.WriteLine("\tUsage:");
                foreach (var line in command.Usage.Split('\n'))
                {
                    Console.WriteLine($"\t\t{line}");
                }

                Console.WriteLine();
            }
        }
    }
}
