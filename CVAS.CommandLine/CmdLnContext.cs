using CVAS.Core;

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

        internal static void ReadStringFrom(string[] args)
        {
            // Read a string from args[], taking into account double quotes (")
            throw new NotImplementedException();
        }
    }
}