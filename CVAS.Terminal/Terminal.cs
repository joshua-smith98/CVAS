using System.Data;

namespace CVAS.Terminal
{
    public static class Terminal
    {
        private static TerminalBlockStatus Status = TerminalBlockStatus.NoBlockActive;

        public static string Prompt()
        {
            return Prompt(">> ");
        }

        public static string Prompt(string prompt)
        {
            Console.Write(prompt);
            string? ret = Console.ReadLine();
            Console.WriteLine();
            if (ret is null) throw new NullReferenceException(); // Should never be null, but just in case...
            return ret;
        }

        public static void AwaitKey()
        {
            Console.ReadKey();
            Console.CursorLeft = Console.CursorLeft - 1;
            Console.Write(" ");
            Console.WriteLine();
        }

        public static void AwaitKey(string message)
        {
            Console.Write(message);
            AwaitKey();
        }

        public static void BeginMessage()
        {
            // Check Status
            if (Status is not TerminalBlockStatus.NoBlockActive)
            {
                if (Status is TerminalBlockStatus.MessageBlockActive)
                    throw new TerminalException("Tried to open a message block twice in a row!");
                    
                throw new TerminalException("Tried to open a message block when a another block type is currently active!");
            }

            Status = TerminalBlockStatus.MessageBlockActive;
        }

        public static void Message(string message)
        {
            // Check Status
            if (Status is not TerminalBlockStatus.MessageBlockActive)
                throw new TerminalException("Tried to post a message outside a message block!");

            Console.WriteLine(message);
        }

        public static void Message(string message, ConsoleColor colour)
        {
            // Check Status
            if (Status is not TerminalBlockStatus.MessageBlockActive)
                throw new TerminalException("Tried to post a message outside a message block!");

            var defaultColour = Console.ForegroundColor;
            Console.ForegroundColor = colour;
            Console.WriteLine(message);
            Console.ForegroundColor = defaultColour;
        }

        public static void EndMessage()
        {
            // Check status
            if (Status is not TerminalBlockStatus.MessageBlockActive)
                throw new TerminalException("Tried to close a message block, when no message block was active!");

            Status = TerminalBlockStatus.NoBlockActive;
            Console.WriteLine();
        }

        public static void BeginReport()
        {
            // Check Status
            if (Status is not TerminalBlockStatus.NoBlockActive)
            {
                if (Status is TerminalBlockStatus.ReportBlockActive)
                    throw new TerminalException("Tried to open a report block twice in a row!");

                throw new TerminalException("Tried to open a report block when a another block type is currently active!");
            }

            Status = TerminalBlockStatus.ReportBlockActive;
        }

        public static void BeginReport(string reportHeader)
        {
            BeginReport();
            Console.WriteLine(reportHeader);
        }

        public static void Report(string message)
        {
            if (Status is not TerminalBlockStatus.ReportBlockActive)
                throw new TerminalException("Tried to post a report outside a report block!");

            Console.CursorLeft = 0;
            Console.Write(Enumerable.Repeat(" ", Console.WindowWidth));
            Console.CursorLeft = 0;
            Console.Write(message);
        }

        public static void EndReport(string message)
        {
            // Check status
            if (Status is not TerminalBlockStatus.ReportBlockActive)
                throw new TerminalException("Tried to close a report block, when no report block was active!");

            Status = TerminalBlockStatus.NoBlockActive;
            Console.WriteLine();
        }
    }
}