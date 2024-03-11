using System.Data;

namespace CVAS.Terminal
{
    public static class Terminal
    {
        private static TerminalBlockStatus Status = TerminalBlockStatus.NoBlockActive;

        public static string Prompt()
        {
            throw new NotImplementedException();
        }

        public static string Prompt(string prompt)
        {
            throw new NotImplementedException();
        }

        public static void AwaitKey()
        {
            throw new NotImplementedException();
        }

        public static void AwaitKey(string message)
        {
            throw new NotImplementedException();
        }

        public static void BeginMessage()
        {
            throw new NotImplementedException();
        }

        public static void Message(string message)
        {
            throw new NotImplementedException();
        }

        public static void Message(string message, ConsoleColor colour)
        {
            throw new NotImplementedException();
        }

        public static void EndMessage()
        {
            throw new NotImplementedException();
        }

        public static void BeginReport()
        {
            throw new NotImplementedException();
        }

        public static void BeginReport(string reportHeader)
        {
            throw new NotImplementedException();
        }

        public static void Report(string message)
        {
            throw new NotImplementedException();
        }

        public static void EndReport(string message)
        {
            throw new NotImplementedException();
        }
    }
}