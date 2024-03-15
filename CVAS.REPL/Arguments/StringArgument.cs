using System.Text;

namespace CVAS.REPL
{
    /// <summary>
    /// An <see cref="Argument"/> that attempts to read a string surrounded by whitespace or double quotes (").
    /// </summary>
    internal class StringArgument : Argument
    {
        public string Name { get; }
        
        public object? Value { get; private set; }

        public Type ValueType => typeof(string);

        internal StringArgument(string name)
        {
            Name = name;
        }

        public void ReadFrom(ref string str)
        {
            // Validity check: str must not be empty
            if (str == "") throw new ArgumentNotValidException($"Expected argument [{Name}], found nothing!");

            StringBuilder valueBuilder = new StringBuilder();

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

                // Trim ref str
                str = str.Substring(valueBuilder.Length + 2).TrimStart();
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

                // Trim ref str
                str = str.Substring(valueBuilder.Length).TrimStart();
            }
        }
    }
}
