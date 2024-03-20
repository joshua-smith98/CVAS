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
            throw new NotImplementedException();
        }

        public void Run()
        {
            // Try to run the given Action
            throw new NotImplementedException();
        }

        internal static string ReadStringFrom(string[] args)
        {
            // Read a string from args[], taking into account double quotes (")
            // Convert string[] back into a string
            var strArgs = string.Join(' ', args);

            // Essentially a copy/paste from REPL StringArgument
            StringBuilder valueBuilder = new StringBuilder();

            // Case: str begins with double quotes
            if (strArgs[0] == '"')
            {
                // Build string from str contents
                for (int i = 1; i < strArgs.Length; i++)
                {
                    if (strArgs[i] == '"') break; // Case: end of value

                    valueBuilder.Append(strArgs[i]);

                    if (i == strArgs.Length - 1) throw new CmdLnArgNotValidException($"Expected second set of double-quotes in argument!"); // TODO: make this message a bit more helpful
                }

                return valueBuilder.ToString();
            }
            else // Case: str doesn't begin with quotes
            {
                // Build string from str contents
                for (int i = 0; i < strArgs.Length; i++)
                {
                    if (char.IsSeparator(strArgs[i])) break; // Case: end of value

                    valueBuilder.Append(strArgs[i]);
                }

                return valueBuilder.ToString();
            }
        }
    }
}