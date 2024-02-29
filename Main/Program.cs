// Main entry point

using CVAS.REPL;

if (args.Length == 0)
{
    // Initialise XAML or WinForms ?
}
else if (args[0] == "-c")
{
    // Initialise REPL
    REPL.Instance.Start();
}
else // if args has some thing(s), but not -c
{
    // Initialise commandline
}