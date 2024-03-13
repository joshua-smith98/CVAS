using System.Data;

namespace CVAS.TerminalNS
{
    /// <summary>
    /// Simplified interface for printing information to the console.
    /// </summary>
    public static class Terminal
    {
        private static TerminalBlockStatus Status = TerminalBlockStatus.NoBlockActive;

        static Terminal()
        {
            Console.CursorVisible = false;
        }

        /// <summary>
        /// Prompts the user for a string, using ">> " as the prompt.
        /// </summary>
        /// <returns></returns>
        public static string Prompt()
        {
            return Prompt(">> ");
        }

        /// <summary>
        /// Prompts the user for a string with a custom prompt.
        /// </summary>
        /// <param name="prompt"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public static string Prompt(string prompt)
        {
            Console.Write(prompt);
            Console.CursorVisible = true;
            string ret = Console.ReadLine()!; // Console.ReadLine() will never be null
            Console.CursorVisible = false;
            Console.WriteLine();
            return ret;
        }

        /// <summary>
        /// Waits for the user to press any key before continuing.
        /// </summary>
        public static char AwaitKey()
        {
            Console.CursorVisible = true;
            var keyinfo = Console.ReadKey();
            Console.CursorVisible = false;
            Console.CursorLeft--;
            Console.Write(" ");
            Console.WriteLine();
            Console.WriteLine();

            return keyinfo.KeyChar;
        }

        /// <summary>
        /// Prints the given message, and then waits for the user to press a key before continuing.
        /// </summary>
        /// <param name="message"></param>
        public static char AwaitKey(string message)
        {
            Console.Write(message);
            return AwaitKey();
        }

        /// <summary>
        /// Prints the given message with the given ConsoleColor, and then waits for the user to press a key before continuing.
        /// </summary>
        /// <param name="message"></param>
        public static char AwaitKey(string message, ConsoleColor colour)
        {
            WriteWithColour(message, colour);
            return AwaitKey();
        }

        /// <summary>
        /// Prompts the user for a y/n response to the given prompt.
        /// </summary>
        /// <param name="prompt"></param>
        /// <returns><see cref="true"/> if user answers 'y', or <see cref="false"/> otherwise.</returns>
        public static bool GetUserApproval(string prompt)
        {
            var response = AwaitKey(prompt);
            return char.ToLower(response) == 'y';
        }

        /// <summary>
        /// Prompts the user for a y/n response to the given prompt. Prints the prompt in the given colour.
        /// </summary>
        /// <param name="prompt"></param>
        /// <returns><see cref="true"/> if user answers 'y', or <see cref="false"/> otherwise.</returns>
        public static bool GetUserApproval(string prompt, ConsoleColor colour)
        {
            var response = AwaitKey(prompt, colour);
            return char.ToLower(response) == 'y';
        }

        /// <summary>
        /// Begins a message block (text surrounded by vertical whitespace). Throws a <see cref="TerminalException"/> if another block is already active.
        /// </summary>
        /// <exception cref="TerminalException"></exception>
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

        /// <summary>
        /// Prints an empty line to the console. Throws <see cref="TerminalException"/> if a message block is not currently active.
        /// </summary>
        public static void Message()
        {
            Message("");
        }

        /// <summary>
        /// Prints the given message to the console. Throws <see cref="TerminalException"/> if a message block is not currently active.
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="TerminalException"></exception>
        public static void Message(string message)
        {
            // Check Status
            if (Status is not TerminalBlockStatus.MessageBlockActive)
                throw new TerminalException("Tried to post a message outside a message block!");

            Console.WriteLine(message);
        }

        /// <summary>
        /// Prints the given message to the console in the given <see cref="ConsoleColor"/>. Throws <see cref="TerminalException"/> if a message block is not currently active.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="colour"></param>
        /// <exception cref="TerminalException"></exception>
        public static void Message(string message, ConsoleColor colour)
        {
            // Check Status
            if (Status is not TerminalBlockStatus.MessageBlockActive)
                throw new TerminalException("Tried to post a message outside a message block!");

            WriteLineWithColour(message, colour);
        }

        /// <summary>
        /// Ends a message block. Throws <see cref="TerminalException"/> if a message block is not currently active.
        /// </summary>
        /// <exception cref="TerminalException"></exception>
        public static void EndMessage()
        {
            // Check status
            if (Status is not TerminalBlockStatus.MessageBlockActive)
                throw new TerminalException("Tried to close a message block, when no message block was active!");

            Status = TerminalBlockStatus.NoBlockActive;
            Console.WriteLine();
        }

        /// <summary>
        /// Prints a single message to the console, automatically opening and closing the required message block.
        /// Throws <see cref="TerminalException"/> if another block type is currently active.
        /// </summary>
        /// <param name="message"></param>
        public static void MessageSingle(string message)
        {
            BeginMessage();
            Message(message);
            EndMessage();
        }

        /// <summary>
        /// Prints a single message to the console in the given colour, automatically opening and closing the required message block.
        /// Throws <see cref="TerminalException"/> if another block type is currently active.
        /// </summary>
        /// <param name="message"></param>
        public static void MessageSingle(string message, ConsoleColor colour)
        {
            BeginMessage();
            Message(message, colour);
            EndMessage();
        }

        /// <summary>
        /// Begins a report (changing text on a single line - e.g. a progress report). Throws a <see cref="TerminalException"/> if another block is already active.
        /// </summary>
        /// <exception cref="TerminalException"></exception>
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

        /// <summary>
        /// Begins a report (changing text on a single line - e.g. a progress report) with the given header. Throws a <see cref="TerminalException"/> if another block is already active.
        /// </summary>
        /// <param name="reportHeader"></param>
        public static void BeginReport(string reportHeader)
        {
            BeginReport();
            Console.WriteLine(reportHeader);
        }

        /// <summary>
        /// Begins a report (changing text on a single line - e.g. a progress report) with the given header, in the given colour. Throws a <see cref="TerminalException"/> if another block is already active.
        /// </summary>
        /// <param name="reportHeader"></param>
        public static void BeginReport(string reportHeader, ConsoleColor colour)
        {
            BeginReport();
            WriteLineWithColour(reportHeader, colour);
        }

        /// <summary>
        /// Clears the current line and prints a report message to the console. Throws <see cref="TerminalException"/> if a report block is not currently active.
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="TerminalException"></exception>
        public static void Report(string message)
        {
            if (Status is not TerminalBlockStatus.ReportBlockActive)
                throw new TerminalException("Tried to post a report outside a report block!");

            Console.CursorLeft = 0;
            Console.Write(string.Concat(Enumerable.Repeat(" ", Console.WindowWidth)));
            Console.CursorLeft = 0;
            Console.Write(message);
        }

        /// <summary>
        /// Clears the current line and prints a report message to the console in the given colour. Throws <see cref="TerminalException"/> if a report block is not currently active.
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="TerminalException"></exception>
        public static void Report(string message, ConsoleColor colour)
        {
            if (Status is not TerminalBlockStatus.ReportBlockActive)
                throw new TerminalException("Tried to post a report outside a report block!");

            Console.CursorLeft = 0;
            Console.Write(string.Concat(Enumerable.Repeat(" ", Console.WindowWidth)));
            Console.CursorLeft = 0;
            WriteWithColour(message, colour);
        }

        /// <summary>
        /// Clears the current line and prints final message, before ending a report. Throws <see cref="TerminalException"/> if a report block is not currently active.
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="TerminalException"></exception>
        public static void EndReport(string message)
        {
            // Check status
            if (Status is not TerminalBlockStatus.ReportBlockActive)
                throw new TerminalException("Tried to close a report block, when no report block was active!");

            Report(message);
            Status = TerminalBlockStatus.NoBlockActive;
            Console.WriteLine();
            Console.WriteLine();
        }

        /// <summary>
        /// Makes a final report in the given colour, before ending the block. Throws <see cref="TerminalException"/> if a report block is not currently active.
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="TerminalException"></exception>
        public static void EndReport(string message, ConsoleColor colour)
        {
            // Check status
            if (Status is not TerminalBlockStatus.ReportBlockActive)
                throw new TerminalException("Tried to close a report block, when no report block was active!");

            Report(message, colour);
            Status = TerminalBlockStatus.NoBlockActive;
            Console.WriteLine();
            Console.WriteLine();
        }

        private static void WriteLineWithColour(string str, ConsoleColor colour)
        {
            var defaultColour = Console.ForegroundColor;
            Console.ForegroundColor = colour;
            Console.WriteLine(str);
            Console.ForegroundColor = defaultColour;
        }

        private static void WriteWithColour(string str, ConsoleColor colour)
        {
            var defaultColour = Console.ForegroundColor;
            Console.ForegroundColor = colour;
            Console.Write(str);
            Console.ForegroundColor = defaultColour;
        }
    }
}