using System.Text;

namespace CVAS.REPL
{
    /// <summary>
    /// An <see cref="Argument"/> that attempts to read a string surrounded by whitespace or double quotes (").
    /// </summary>
    internal class StringArgument : Argument
    {
        public override Type ValueType => typeof(string);

        public StringArgument(string name, bool isCompulsory) : base(name, isCompulsory) { }

        protected override string ReadFromImpl(string str)
        {
            StringBuilder valueBuilder = new();

            // Case: str begins with double quotes
            if (str[0] == '"')
            {
                // Build string from str contents
                for (int i = 1; i < str.Length; i++)
                {
                    if (str[i] == '"') break; // Case: end of value

                    valueBuilder.Append(str[i]);

                    if (i == str.Length - 1) throw new ArgumentNotValidException($"Expected second set of double-quotes (\") in argument: [{Name}]"); // Validity check: str must contain a second set of double quotes
                }

                Value = valueBuilder.ToString();

                // Return trimmed str
                return str[(valueBuilder.Length + 2)..].TrimStart();
            }
            else // Case: str doesn't begin with quotes
            {
                // Build string from str contents
                for (int i = 0; i < str.Length; i++)
                {
                    if (char.IsSeparator(str[i])) break; // Case: end of value

                    valueBuilder.Append(str[i]);
                }

                Value = valueBuilder.ToString();

                // Return trimmed str
                return str[valueBuilder.Length..].TrimStart();
            }
        }
    }
}
