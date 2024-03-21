﻿using CVAS.Core;
using CVAS.FileSystem;
using CVAS.TerminalNS;
using System.IO;

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
                if (instance is null) throw new NullReferenceException();
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
        {
            new HelpCmdLnArgument(),
            new PlayCmdLnArgument(),
            new RenderCmdLnArgument(),
            new LibraryCmdLnArgument(),
            new SentenceCmdLnArgument(),
            new OutputCmdLnArgument(),
        };

        private CmdLnContext() { } // Non-constructable

        /// <summary>
        /// Initialises <see cref="CmdLnContext.Instance"/>. Throws a <see cref="CmdLnException"/> if called more than once.
        /// </summary>
        /// <exception cref="CmdLnException"></exception>
        public static void Init()
        {
            // Check if already initialised
            if (instance is not null) throw new CmdLnException("CmdLnContext cannot be initialised twice!");
            instance = new CmdLnContext();
        }

        public void ReadFromAndRun(string[] args)
        {
            try
            {
                ReadFrom(args);
                Run();
            }
            catch (CmdLnException e)
            {
                Terminal.MessageSingle(e.Message, ConsoleColor.Red);
            }
        }

        /// <summary>
        /// Tries to read this context from the given arguments.
        /// </summary>
        /// <param name="args"></param>
        /// <exception cref="CmdLnStrNotValidException"></exception>
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
                    catch(CmdLnArgNotValidException e) // This command matches, but an error occured while reading the argument value
                    {
                        cmdFound = true;
                        throw e;
                    }
                    catch(CmdLnStrNotValidException) { } // This command doesn't match, just continue
                }

                if (!cmdFound) throw new CmdLnStrNotValidException($"No option found for: {args[0]}");
            }
        }

        /// <summary>
        /// Tries to run this context with the contained properties. Throws a <see cref="CmdLnContextNotValidException"/> upon failure.
        /// </summary>
        /// <exception cref="CmdLnContextNotValidException"></exception>
        private void Run()
        {
            // Check that we have an Action to run
            if (Action is null) throw new CmdLnContextNotValidException("No action provided!");

            // Try to load library
            if (LibraryPath is not null)
            {
                try
                {
                    Library = LibraryFolder.LoadFrom(LibraryPath).Construct();
                }
                catch (DirectoryNotFoundException)
                {
                    throw new CmdLnArgNotValidException($"Failed to load library. Couldn't find directory at: {LibraryPath}");
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
            catch (CmdLnException e)
            { 
                throw e;
            }
        }
    }
}