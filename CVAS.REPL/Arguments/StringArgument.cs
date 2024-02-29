using System.Text;

namespace CVAS.REPL
{
    internal class StringArgument : IArgument
    {
        public object? Value { get; private set; }

        public Type ValueType => typeof(string);

        internal StringArgument() { }

        public void ReadFrom(ref string str)
        {
            // Validity check: str must not be empty
            if (str == "") throw new ArgumentNotValidException();

            StringBuilder valueBuilder = new StringBuilder();

            // Case: str begins with double quotes
            if (str[0] == '"')
            {
                // Build string from str contents
                for (int i = 1; i < str.Length; i++)
                {
                    if (str[i] == '"') break;

                    valueBuilder.Append(str[i]);

                    if (i == str.Length - 1) throw new ArgumentNotValidException(); // Validity check: str must contain a second set of double quotes
                }

                Value = valueBuilder.ToString();

                // Trim ref str
                str = str.Substring(valueBuilder.Length + 2).TrimStart();
            }
            else
            {
                // Build string from str contents
                for (int i = 0; i < str.Length; i++)
                {
                    if (char.IsSeparator(str[i])) break;

                    valueBuilder.Append(str[i]);
                }

                Value = valueBuilder.ToString();

                // Trim ref str
                str = str.Substring(valueBuilder.Length).TrimStart();
            }
        }
    }
}
