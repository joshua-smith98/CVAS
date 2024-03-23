using CVAS.TerminalNS;

namespace CVAS.REPL
{
    /// <summary>
    /// A <see cref="Command"/> that either prints the description and usage for a given command, or lists all commands.
    /// </summary>
    internal class HelpCommand : Command
    {
        public override string Str => "help";

        public override string Description => "Gets the description and usage instructions for the given command, or lists all commands.";

        public override string[] Usage { get; } = ["help", "help [command]"];

        public override Command[] SubCommands { get; } = [];

        public override Argument[] Arguments { get; } =
        [
            new CommandArgument("command", false),
        ];

        protected override void VerifyArgsAndRun()
        {
            // Case: there is no given argument for [command]
            if (Arguments[0].Value is null)
            {
                // Print command list
                foreach(Command command in REPL.Instance.CommandInstances)
                {
                    PrintCommandDetails(command);
                }
            }
            else // Case: there is one or more argument given
            {
                // Get command and print details
                var command = (Command)Arguments[0].Value!;
                PrintCommandDetails(command);
            }
        }

        /// <summary>
        /// Prints the Str, Description and Usage details for the given command.
        /// </summary>
        /// <param name="command"></param>
        private static void PrintCommandDetails(Command command)
        {
            // Print Str and Description
            Terminal.BeginMessage();
            Terminal.Message($"{command.Str}:");
            Terminal.Message($"\t{command.Description}", ConsoleColor.Yellow);

            // Print use cases
            foreach (string useCase in command.Usage)
            {
                Terminal.Message($"\t>> {useCase}", ConsoleColor.DarkYellow);
            }
            Terminal.EndMessage();
        }
    }
}
