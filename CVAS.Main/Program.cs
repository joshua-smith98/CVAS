using CVAS.WinAudioEngineNS;
using CVAS.CommandLine;
using CVAS.REPL;
using CVAS.TerminalNS;

// Main entry point
#if !DEBUG
try
{
#endif
    if (args.Length == 0) // No args | default app
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
        // Silence Terminal, initialise command-line and read from args
        Terminal.IsSilent = true;
        CmdLnContext.Init();
        CmdLnContext.Instance.ReadFromAndRun(args);
    }
#if !DEBUG
}
catch(Exception e)
{
    // Print unknown error message for misc exceptions
    Terminal.IsSilent = false; // just in case

    Terminal.ForceBeginMessage();
    Terminal.Message($"A fatal error has occurred, of type: {e.GetType()}", ConsoleColor.Red);
    Terminal.Message();
    Terminal.Message($"\"{e.Message}\"", ConsoleColor.Red);
    Terminal.Message($"{e.StackTrace}", ConsoleColor.Red);
    Terminal.Message();
    Terminal.Message("Please take a screenshot of this, and create an issue on github.", ConsoleColor.Red);
    Terminal.EndMessage();

    Terminal.AwaitKey("Press any key to close the application...");
}
#endif