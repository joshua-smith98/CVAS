using CVAS.TerminalNS;

namespace CVAS.CommandLine
{
    internal class HelpCmdLnArgument : CmdLnArgument
    {
        public override string Str => "-help";

        public override string? ShortStr => "-h";

        public override string[] DescriptionLines { get; } =
        {
            "Prints command-line instructions and lists all options and their descriptions.",
        };

        public override string Usage => "-help | -h";

        protected override string[] ImportFromImpl(string[] args)
        {
            // Assign action to CmdLnContext
            CmdLnContext.Instance.Action = () =>
            {
                Terminal.BeginMessage();
                Terminal.Message("---CVAS Command-line Interface v0.5.0---");
                Terminal.Message("Usage:", ConsoleColor.Yellow);
                Terminal.Message("\tCVAS.exe -[option] (argument) -[option] (argument) ...", ConsoleColor.DarkYellow);
                Terminal.EndMessage();

                Terminal.MessageSingle("---Options---");
                foreach (CmdLnArgument argument in CmdLnContext.Instance.CmdLnArguments)
                {
                    Terminal.BeginMessage();
                    Terminal.Message($"{argument.Str} ({argument.ShortStr})", ConsoleColor.Yellow);
                    foreach (var line in argument.DescriptionLines)
                    {
                        Terminal.Message($"\t{line}", ConsoleColor.Yellow);
                    }
                    Terminal.Message($"\t{argument.Usage}", ConsoleColor.DarkYellow);
                    Terminal.Message();
                    Terminal.EndMessage();
                }

                Terminal.AwaitKey("Press any key to close the application...");
            };

            // Return trimmed args
            return new string[0]; // -help will force execution and prevent further arguments from being read
        }
    }
}
