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
                Terminal.Instance.IsSilent = false;
                Terminal.Instance.BeginMessage();
                Terminal.Instance.Message("---CVAS Command-line Interface v0.5.0---");
                Terminal.Instance.Message("Usage:", ConsoleColor.Yellow);
                Terminal.Instance.Message("\t[cvas.exe] -[option] (argument) -[option] (argument) ...", ConsoleColor.DarkYellow);
                Terminal.Instance.EndMessage();

                // Print list of options
                Terminal.Instance.MessageSingle("---Options---");
                foreach (CmdLnArgument argument in CmdLnContext.Instance.CmdLnArguments)
                {
                    Terminal.Instance.BeginMessage();
                    Terminal.Instance.Message($"{argument.Str} ({argument.ShortStr})", ConsoleColor.Yellow);
                    foreach (var line in argument.DescriptionLines)
                    {
                        Terminal.Instance.Message($"\t{line}", ConsoleColor.Yellow);
                    }
                    Terminal.Instance.Message($"\t{argument.Usage}", ConsoleColor.DarkYellow);
                    Terminal.Instance.Message();
                    Terminal.Instance.EndMessage();
                }
                Terminal.Instance.IsSilent = true;
            };

            // Return trimmed args
            return []; // -help will force execution and prevent further arguments from being read
        }
    }
}
