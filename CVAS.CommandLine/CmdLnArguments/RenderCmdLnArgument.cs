namespace CVAS.CommandLine
{
    internal class RenderCmdLnArgument : CmdLnArgument
    {
        public override string Str => "-render";

        public override string? ShortStr => "-r";

        public override string Description => "Renders a sentence to the file at the provided output path. Warning: this will replace any file that already exists!";

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
