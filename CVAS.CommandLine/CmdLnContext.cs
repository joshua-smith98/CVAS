using CVAS.Core;
using CVAS.FileSystem;
using CVAS.TerminalNS;

namespace CVAS.CommandLine
{
    /// <summary>
    /// Represents the contextual items used for CVAS's command-line functionality.
    /// </summary>
    public class CmdLnContext
    {
        /// <summary>
        /// The current instance of <see cref="CmdLnContext"/>. Throws a <see cref="NullReferenceException"/> if <see cref="CmdLnContext.Init"> has not been called.
        /// </summary>
        public static CmdLnContext Instance
        {
            get
            {
                instance ??= new CmdLnContext();
                return instance;
            }
        }
        private static CmdLnContext? instance;

        public string? LibraryPath { get; internal set; }
        public string? SentenceStr { get; internal set; }
        public string? OutputPath { get; internal set; }

        public Library? Library { get; internal set; }
        public Sentence? Sentence { get; internal set; }
        public Action? Action { get; internal set; }

        /// <summary>
        /// Array of all <see cref="CmdLnArgument"/>s.
        /// </summary>
        internal CmdLnArgument[] CmdLnArguments =
        [
            new HelpCmdLnArgument(),
            new PlayCmdLnArgument(),
            new RenderCmdLnArgument(),
            new LibraryCmdLnArgument(),
            new SentenceCmdLnArgument(),
            new OutputCmdLnArgument(),
        ];

        private CmdLnContext() { } // Non-constructable

        /// <summary>
        /// Reads the given arguments and tries to run the attached action. Handles all <see cref="CmdLnException"/>s.
        /// </summary>
        /// <param name="args"></param>
        public void ReadFromAndRun(string[] args)
        {
            try
            {
                ReadFrom(args);
                Run();
            }
            catch (CmdLnException e)
            {
                Terminal.Instance.IsSilent = false;
                Terminal.Instance.MessageSingle(e.Message, ConsoleColor.Red);
                Terminal.Instance.IsSilent = true;
            }
        }

        /// <summary>
        /// Tries to read this context from the given arguments.
        /// </summary>
        /// <param name="args"></param>
        /// <exception cref="CmdLnStrNotValidException"/>
        /// <exception cref="CmdLnArgNotValidException"/>
        private void ReadFrom(string[] args)
        {
            // Try to read the data into this context from args[]
            
            // Iterate through args and try arguments
            while(args.Length != 0)
            {
                bool cmdFound = false;

                foreach (CmdLnArgument cmdLnArgument in CmdLnArguments)
                {
                    try
                    {
                        args = cmdLnArgument.ImportFromAndTrim(args);
                        cmdFound = true;
                        break;
                    }
                    catch(CmdLnArgNotValidException) // This command matches, but an error occured while reading the argument value
                    {
                        cmdFound = true;
                        throw;
                    }
                    catch(CmdLnStrNotValidException) { } // This command doesn't match, just continue
                }

                if (!cmdFound) throw new CmdLnStrNotValidException($"'{args[0]}' is not a valid option. Use '-help' or '-h' to get a list of valid options.");
            }
        }

        /// <summary>
        /// Tries to run this context with the contained properties. Throws the appropriate <see cref="CmdLnException"/> upon failure.
        /// </summary>
        /// <exception cref="CmdLnContextNotValidException"></exception>
        /// <exception cref="CmdLnArgNotValidException"></exception>
        private void Run()
        {
            // Check that we have an Action to run
            if (Action is null) throw new CmdLnContextNotValidException("Couldn't find anything to do!");

            // Try to load library
            if (LibraryPath is not null)
            {
                try
                {
                    Library = LibraryFolder.LoadFrom(LibraryPath).Construct();
                }
                catch (DirectoryNotFoundException)
                {
                    throw new CmdLnArgNotValidException($"Couldn't find directory: {LibraryPath}");
                }
            }

            // Try to get sentence
            if (SentenceStr is not null)
            {
                Sentence = Library!.GetSentence(SentenceStr);
            }

            // Try to run the given action
            try
            {
                Action.Invoke();
            }
            catch (CmdLnException)
            { 
                throw;
            }
        }
    }
}