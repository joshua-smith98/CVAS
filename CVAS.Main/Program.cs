// Main entry point

using CVAS.AudioEngine;
using CVAS.CommandLine;
using CVAS.REPL;
using CVAS.TerminalNS;

try
{
    if (args.Length == 0)
    {
        // Since the REPL is the only usable interface, we will set it as the default for now
        AudioEngine.Init();
        REPL.Init();
        REPL.Instance.Start();

        // Initialise XAML or WinForms ?
    }
    else if (args[0] == "-c")
    {
        // Initialise audio engine
        AudioEngine.Init();

        // Initialise and start REPL;
        REPL.Init();
        REPL.Instance.Start();
    }
    else // if args has some thing(s), but not -c
    {
        Terminal.IsSilent = true;
        CmdLnContext.Init();
        CmdLnContext.Instance.ReadFromAndRun(args);
    }
}
catch(Exception e)
{
    Terminal.IsSilent = false;

    Terminal.BeginMessage();
    Terminal.Message("An unknown error has occurred!", ConsoleColor.Red);
    Terminal.Message();
    Terminal.Message($"Type: {e.GetType()}", ConsoleColor.Yellow);
    Terminal.Message();
    if (e.Message == string.Empty) Terminal.Message("Message: No message was given.", ConsoleColor.Yellow);
    else Terminal.Message($"Message: {e.Message}", ConsoleColor.Yellow);
    Terminal.Message();
    Terminal.Message($"Stack trace:", ConsoleColor.Yellow);
    Terminal.Message($"{e.StackTrace}", ConsoleColor.Yellow);
    Terminal.Message();
    Terminal.Message("Please take a screenshot of this, and create an issue on github.", ConsoleColor.Red);
    Terminal.Message();
    Terminal.Message("The application will now close.", ConsoleColor.Red);
    Terminal.EndMessage();
}