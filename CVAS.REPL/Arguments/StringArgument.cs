using System.Text;

namespace CVAS.REPL
{
    public class StringArgument : IArgument
    {
        public object? Value { get; private set; }

        public Type ValueType => typeof(string);

        public void ReadFrom(ref string str)
        {
            // Validity check: str must not be empty, and must start with double quotes
            if (str == "" || str[0] != '"') throw new ArgumentNotValidException();

            // Build string from str contents
            StringBuilder valueBuilder = new StringBuilder();

            for (int i = 1; i < str.Length; i++)
            {
                if (str[i] == '"') break;

                valueBuilder.Append(str[i]);

                if (i == str.Length - 1) throw new ArgumentNotValidException(); // Validity check: str must contain a second set of double quotes
            }

            Value = valueBuilder.ToString();

            // Trim ref str
            str = str.Substring(valueBuilder.Length + 2).TrimStart();s
        }
    }
}
