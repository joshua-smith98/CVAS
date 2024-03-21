using CVAS.Core;
using System.Text;
using System.Xml.Linq;

namespace CVAS.CommandLine
{
    public class CmdLnContext
    {
        public static CmdLnContext Instance
        {
            get
            {
                if (instance is null) throw new NullReferenceException();
                return instance;
            }
        }
        private static CmdLnContext? instance;
        
        public Library? Library { get; internal set; }
        public Sentence? Sentence { get; internal set; }
        public Action? Action { get; internal set; }

        internal CmdLnArgument[] CmdLnArguments =
        {

        };

        private CmdLnContext() { } // Non-constructable

        public static void Init()
        {
            // Check if already initialised
            if (instance is not null) throw new Exception("CmdLnContext cannot be initialised twice!");
            instance = new CmdLnContext();
        }

        public void ReadFrom(string[] args)
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
                        throw e;
                    }
                    catch(CmdLnStrNotValidException) { } // This command doesn't match, just continue
                }

                if (!cmdFound) throw new CmdLnStrNotValidException($"No option found for: {args[0]}");
            }
        }

        public void Run()
        {
            // Check that we have an Action to run
            if (Action is null) throw new CmdLnContextNotValidException("No action provided!");

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