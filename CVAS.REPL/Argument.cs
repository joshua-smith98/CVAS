namespace CVAS.REPL
{
    public static class Argument
    {
        private static readonly IArgument[] Arguments =
        {

        };

        public static IArgument? ReadFrom(ref string str)
        {
            foreach (IArgument arg in Arguments)
            {
                try
                {
                    arg.ReadFrom(ref str);
                    return arg; // Case: a valid argument is found
                }
                catch (ArgumentInvalidException) { }
            }

            return null; // Case: no valid argument is found
        }
    }
}
