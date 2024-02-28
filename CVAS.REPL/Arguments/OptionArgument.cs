namespace CVAS.REPL
{
    public class OptionArgument : IArgument
    {
        public object? Value { get; private set; }

        public Type ValueType => typeof(string);

        public void ReadFrom(ref string str)
        {
            // Validity check: string must not be empty, and must start with '-'
            if (str == "" || str[0] != '-') throw new ArgumentNotValidException();

            var option = str.Split().First().Substring(1); // Get option, and remove '-' from the beginning
            Value = option;

            str = str.Substring(option.Length + 1).TrimStart();
        }
    }
}
