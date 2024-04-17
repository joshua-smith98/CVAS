using LinuxConsoleReadLineFix;

namespace CVAS.TerminalNS
{
    /// <summary>
    /// Simplified interface for printing information to the console.
    /// </summary>
    public class Terminal : IDisposable
    {
        
        public static Terminal Instance
        {
            get
            {
                if (instance is null) throw new NullReferenceException();
                else return instance;
            }
        }
        private static Terminal? instance;
        
        /// <summary>
        /// Setting this to true will disable all terminal messages and reports.
        /// </summary>
        public bool IsSilent { get; set; } = false;

        private TerminalBlockStatus Status = TerminalBlockStatus.NoBlockActive;

        private Terminal()
        {
            Console.CursorVisible = false;
        }

        public void Dispose()
        {
            Console.CursorVisible = true; // Make terminal cursor visible again when disposed (seems to affect linux but not windows)
            GC.SuppressFinalize(this);
        }
        
        public static Terminal Init()
        {
            if (instance is not null) throw new TerminalException("Terminal cannot be initialised twice!");
            else
            {
                var ret = new Terminal();
                instance = ret;
                return ret;
            }
        }

        /// <summary>
        /// Prompts the user for a string, using ">> " as the prompt.
        /// </summary>
        /// <returns></returns>
        public string Prompt()
        {
            return Prompt(">> ");
        }

        /// <summary>
        /// Prompts the user for a string with a custom prompt.
        /// </summary>
        /// <param name="prompt"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public string Prompt(string prompt)
        {
            Console.Write(prompt);
            Console.CursorVisible = true;
            string ret = LinuxTerminalFix.ReadLine()!;
            Console.CursorVisible = false;
            Console.WriteLine();
            return ret;
        }

        /// <summary>
        /// Waits for the user to press any key before continuing.
        /// </summary>
        public char AwaitKey()
        {
            // Console.CursorVisible = true;
            var keyinfo = Console.ReadKey();
            // Console.CursorVisible = false;
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
        public char AwaitKey(string message)
        {
            Console.Write(message);
            return AwaitKey();
        }

        /// <summary>
        /// Prints the given message with the given ConsoleColor, and then waits for the user to press a key before continuing.
        /// </summary>
        /// <param name="message"></param>
        public char AwaitKey(string message, ConsoleColor colour)
        {
            WriteWithColour(message, colour);
            return AwaitKey();
        }

        /// <summary>
        /// Prompts the user for a y/n response to the given prompt.
        /// </summary>
        /// <param name="prompt"></param>
        /// <returns><see cref="true"/> if user answers 'y', or <see cref="false"/> otherwise.</returns>
        public bool GetUserApproval(string prompt)
        {
            var response = AwaitKey(prompt);
            return char.ToLower(response) == 'y';
        }

        /// <summary>
        /// Prompts the user for a y/n response to the given prompt. Prints the prompt in the given colour.
        /// </summary>
        /// <param name="prompt"></param>
        /// <returns><see cref="true"/> if user answers 'y', or <see cref="false"/> otherwise.</returns>
        public bool GetUserApproval(string prompt, ConsoleColor colour)
        {
            var response = AwaitKey(prompt, colour);
            return char.ToLower(response) == 'y';
        }

        /// <summary>
        /// Begins a message block (text surrounded by vertical whitespace). Throws a <see cref="TerminalException"/> if another block is already active.
        /// </summary>
        /// <exception cref="TerminalException"></exception>
        public void BeginMessage()
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
        /// Forces a message block to begin. Useful for exceptions.
        /// </summary>
        public void ForceBeginMessage() // TODO: Replace manual block opening and closing with IDisposable object, so we don't need this
        {
            if (Status is TerminalBlockStatus.MessageBlockActive)
                Console.WriteLine(); // Write a single new line if a message block was active
            else if (Status is TerminalBlockStatus.ReportBlockActive)
                Console.WriteLine("\n"); // Write two new lines if a report block was active
            
            Status = TerminalBlockStatus.MessageBlockActive;
        }

        /// <summary>
        /// Prints an empty line to the console. Throws <see cref="TerminalException"/> if a message block is not currently active.
        /// </summary>
        public void Message()
        {
            Message("");
        }

        /// <summary>
        /// Prints the given message to the console. Throws <see cref="TerminalException"/> if a message block is not currently active.
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="TerminalException"></exception>
        public void Message(string message)
        {
            // Check Status
            if (Status is not TerminalBlockStatus.MessageBlockActive)
                throw new TerminalException("Tried to post a message outside a message block!");

            if (!IsSilent) Console.WriteLine(message);
        }

        /// <summary>
        /// Prints the given message to the console in the given <see cref="ConsoleColor"/>. Throws <see cref="TerminalException"/> if a message block is not currently active.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="colour"></param>
        /// <exception cref="TerminalException"></exception>
        public void Message(string message, ConsoleColor colour)
        {
            // Check Status
            if (Status is not TerminalBlockStatus.MessageBlockActive)
                throw new TerminalException("Tried to post a message outside a message block!");

            if (!IsSilent) WriteLineWithColour(message, colour);
        }

        /// <summary>
        /// Ends a message block. Throws <see cref="TerminalException"/> if a message block is not currently active.
        /// </summary>
        /// <exception cref="TerminalException"></exception>
        public void EndMessage()
        {
            // Check status
            if (Status is not TerminalBlockStatus.MessageBlockActive)
                throw new TerminalException("Tried to close a message block, when no message block was active!");

            Status = TerminalBlockStatus.NoBlockActive;
            if (!IsSilent) Console.WriteLine();
        }

        /// <summary>
        /// Prints a single message to the console, automatically opening and closing the required message block.
        /// Throws <see cref="TerminalException"/> if another block type is currently active.
        /// </summary>
        /// <param name="message"></param>
        public void MessageSingle(string message)
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
        public void MessageSingle(string message, ConsoleColor colour)
        {
            BeginMessage();
            Message(message, colour);
            EndMessage();
        }

        /// <summary>
        /// Begins a report (changing text on a single line - e.g. a progress report). Throws a <see cref="TerminalException"/> if another block is already active.
        /// </summary>
        /// <exception cref="TerminalException"></exception>
        public void BeginReport()
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
        public void BeginReport(string reportHeader)
        {
            BeginReport();
            if (!IsSilent) Console.WriteLine(reportHeader);
        }

        /// <summary>
        /// Begins a report (changing text on a single line - e.g. a progress report) with the given header, in the given colour. Throws a <see cref="TerminalException"/> if another block is already active.
        /// </summary>
        /// <param name="reportHeader"></param>
        public void BeginReport(string reportHeader, ConsoleColor colour)
        {
            BeginReport();
            if (!IsSilent) WriteLineWithColour(reportHeader, colour);
        }

        /// <summary>
        /// Clears the current line and prints a report message to the console. Throws <see cref="TerminalException"/> if a report block is not currently active.
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="TerminalException"></exception>
        public void Report(string message)
        {
            if (Status is not TerminalBlockStatus.ReportBlockActive)
                throw new TerminalException("Tried to post a report outside a report block!");

            if (!IsSilent)
            {
                Console.CursorLeft = 0;
                Console.Write(string.Concat(Enumerable.Repeat(" ", Console.WindowWidth)));
                Console.CursorLeft = 0;
                Console.Write(message);
            }
        }

        /// <summary>
        /// Clears the current line and prints a report message to the console in the given colour. Throws <see cref="TerminalException"/> if a report block is not currently active.
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="TerminalException"></exception>
        public void Report(string message, ConsoleColor colour)
        {
            if (Status is not TerminalBlockStatus.ReportBlockActive)
                throw new TerminalException("Tried to post a report outside a report block!");

            if (!IsSilent)
            {
                Console.CursorLeft = 0;
                Console.Write(string.Concat(Enumerable.Repeat(" ", Console.WindowWidth)));
                Console.CursorLeft = 0;
                WriteWithColour(message, colour);
            }
        }

        /// <summary>
        /// Clears the current line and prints final message, before ending a report. Throws <see cref="TerminalException"/> if a report block is not currently active.
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="TerminalException"></exception>
        public void EndReport(string message)
        {
            // Check status
            if (Status is not TerminalBlockStatus.ReportBlockActive)
                throw new TerminalException("Tried to close a report block, when no report block was active!");

            if (!IsSilent)
            {
                Report(message);
                Console.WriteLine();
                Console.WriteLine();
            }

            Status = TerminalBlockStatus.NoBlockActive;
        }

        /// <summary>
        /// Makes a final report in the given colour, before ending the block. Throws <see cref="TerminalException"/> if a report block is not currently active.
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="TerminalException"></exception>
        public void EndReport(string message, ConsoleColor colour)
        {
            // Check status
            if (Status is not TerminalBlockStatus.ReportBlockActive)
                throw new TerminalException("Tried to close a report block, when no report block was active!");

            if (!IsSilent)
            {
                Report(message, colour);
                Console.WriteLine();
                Console.WriteLine();
            }

            Status = TerminalBlockStatus.NoBlockActive;
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