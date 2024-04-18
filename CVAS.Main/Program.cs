using CVAS.AudioEngine;
using CVAS.CommandLine;
using CVAS.REPL;
using CVAS.TerminalNS;

// Main entry point
// Always use Terminal
using var t = Terminal.Init();

// Only catch unhandled exceptions on release
#if !DEBUG
try
{
#endif
    if (args.Length == 0) // No args | default app
    {
        // Since the REPL is the only usable interface, we will set it as the default for now
        IAudioEngine.Init();
        REPL.Init();
        REPL.Instance.Start();

        // Initialise XAML or WinForms ?
    }
    else if (args[0] == "-c")
    {
        // Initialise audio engine
        IAudioEngine.Init();

        // Initialise and start REPL;
        REPL.Init();
        REPL.Instance.Start();
    }
    else // if args has some thing(s), but not -c
    {
        // Silence Terminal, initialise command-line and read from args
        Terminal.Instance.IsSilent = true;
        CmdLnContext.Init();
        CmdLnContext.Instance.ReadFromAndRun(args);
    }
#if !DEBUG
}
catch(Exception e)
{
    // Print unknown error message for misc exceptions
    Terminal.Instance.IsSilent = false; // just in case

    Terminal.Instance.ForceBeginMessage();
    Terminal.Instance.Message($"A fatal error has occurred, of type: {e.GetType()}", ConsoleColor.Red);
    Terminal.Instance.Message();
    Terminal.Instance.Message($"\"{e.Message}\"", ConsoleColor.Red);
    Terminal.Instance.Message($"{e.StackTrace}", ConsoleColor.Red);
    Terminal.Instance.Message();
    Terminal.Instance.Message("Please take a screenshot of this, and create an issue on github.", ConsoleColor.Red);
    Terminal.Instance.EndMessage();

    Terminal.Instance.AwaitKey("Press any key to close the application...");
}
#endif