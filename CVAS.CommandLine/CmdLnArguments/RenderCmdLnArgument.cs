namespace CVAS.CommandLine
{
    /// <summary>
    /// A <see cref="CmdLnArgument"/> that renders a sentence to a file.
    /// </summary>
    internal class RenderCmdLnArgument : CmdLnArgument
    {
        public override string Str => "-render";

        public override string? ShortStr => "-r";

        public override string[] DescriptionLines { get; } =
        {
            "Renders a sentence to the file at the provided output path.",
            "Warning: this will replace any file that already exists!",
        };

        public override string Usage => "-render | -r";

        protected override string[] ImportFromImpl(string[] args)
        {
            // Assign action to CmdLnContext
            CmdLnContext.Instance.Action = () =>
            {
                // Verify: sentence exists
                if (CmdLnContext.Instance.Sentence is null) throw new CmdLnContextNotValidException("Tried to render to file, but no sentence was provided!");

                // Verify: output path exists
                if (CmdLnContext.Instance.OutputPath is null) throw new CmdLnContextNotValidException("Tried to render a sentence, but no output file was provided!");

                // Render sentence
                AudioEngine.AudioEngine.Render(CmdLnContext.Instance.Sentence.GetAudioClip(), CmdLnContext.Instance.OutputPath);
            };

            // return trimmed args
            return args[1..];
        }
    }
}
