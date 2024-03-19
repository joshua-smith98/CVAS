// Main entry point

using CVAS.AudioEngine;
using CVAS.REPL;

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
    // Initialise commandline
}