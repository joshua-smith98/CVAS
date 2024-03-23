using CVAS.TerminalNS;

namespace CVAS.CommandLine
{
    /// <summary>
    /// A <see cref="CmdLnArgument"/> that prints help information.
    /// </summary>
    internal class HelpCmdLnArgument : CmdLnArgument
    {
        public override string Str => "-help";

        public override string? ShortStr => "-h";

        public override string[] DescriptionLines { get; } =
        [
            "Prints command-line instructions and lists all options and their descriptions.",
        ];

        public override string Usage => "-help | -h";

        protected override string[] ImportFromImpl(string[] args)
        {
            // Assign action to CmdLnContext
            CmdLnContext.Instance.Action = () =>
            {
                // Header
                Terminal.IsSilent = false;
                Terminal.BeginMessage();
                Terminal.Message("---CVAS Command-line Interface v0.5.0---");
                Terminal.Message("Usage:", ConsoleColor.Yellow);
                Terminal.Message("\t[cvas.exe] -[option] (argument) -[option] (argument) ...", ConsoleColor.DarkYellow);
                Terminal.EndMessage();

                // Print list of options
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
                Terminal.IsSilent = true;
            };

            // Return trimmed args
            return []; // -help will force execution and prevent further arguments from being read
        }
    }
}
