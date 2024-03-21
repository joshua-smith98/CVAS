﻿namespace CVAS.CommandLine
{
    internal class PlayCmdLnArgument : CmdLnArgument
    {
        public override string Str => "-play";

        public override string? ShortStr => "-p";

        public override string Description => "Plays the given sentence, using the given library. The application will remain open until the sentence has finished playing.";

        protected override string[] ImportFromImpl(string[] args)
        {
            // Assign action to CmdLnContext.Action

            CmdLnContext.Instance.Action = () =>
            {
                // Verify: sentence exists (we do not need to verify library, as it must exist if sentence exists)
                if (CmdLnContext.Instance.Sentence is null) throw new CmdLnContextNotValidException("Tried to play, but no sentence was given!");

                // Play sentence
                AudioEngine.AudioEngine.PlayOnce(CmdLnContext.Instance.Sentence.GetAudioClip());
            };

            // Return trimmed args
            return args[1..];
        }
    }
}
