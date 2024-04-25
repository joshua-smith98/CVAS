using CVAS.AudioEngine;
using CVAS.CommandLine;
using CVAS.REPL;
using CVAS.TerminalNS;

// Main entry point

using (Terminal.Instance) // Use terminal for duration of try/catch scope so that we can still print the fatal error message

#if !DEBUG // Only catch unhandled exceptions on release
    try
    {
#endif
        if (args.Length == 0)
        {
            using (IAudioEngine.Instance) // Use AudioEngine instance in the scope of the REPL
                REPL.Instance.Start(); // Start REPL when run with no args
        }
        else
        {
            // Silence Terminal, initialise command-line and read from args
            Terminal.Instance.IsSilent = true;
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