using CVAS.DataStructure;
using CVAS.AudioEngine;

// Phrase algorithm test
List<Phrase> Phrases = new List<Phrase>
{
    new("the train on platform", new AudioFileCached("sounds/the train on platform.mp3"), Inflection.Middle),
    new("one", new AudioFileCached("sounds/one.mp3"), new AudioFileCached("sounds/one.m.mp3")),
    new("goes to", new AudioFileCached("sounds/goes to.mp3"), Inflection.Middle),
    new("the train on platform one", new AudioFileCached("sounds/the train on platform one.mp3"), Inflection.Middle),
    new("central", new AudioFileCached("sounds/central.mp3"), new AudioFileCached("sounds/central.m.mp3")),
    new("first stop", new AudioFileCached("sounds/first stop.mp3"), Inflection.Middle),
    new("strathfield", new AudioFileCached("sounds/strathfield.mp3"), new AudioFileCached("sounds/strathfield.m.mp3")),
    new("then", new AudioFileCached("sounds/then.mp3"), Inflection.Middle),
    new("redfern", new AudioFileCached("sounds/redfern.mp3"), new AudioFileCached("sounds/redfern.m.mp3")),
    new("and", new AudioFileCached("sounds/and.mp3")),
};

Library library = new(Phrases.ToArray());

Sentence testSentence = library.GetSentence("The train on platform | one goes to Strathfield. First stop Central, then Redfern, Central, and Strathfield.");

Console.Write("Result: ");
foreach (IPhrase subPhrase in testSentence.spokenPhrases)
{
    Console.Write($"[{subPhrase.str}] ");
}

// Audio engine test
Console.ReadKey();

Playlist playlist = testSentence.GetAudioClip();
Console.WriteLine("Playing:");

foreach (IAudioClip audioClip in playlist.audioClips)
{
    if (audioClip is IAudioFile)
        Console.WriteLine(((IAudioFile)audioClip).path);
    else if (audioClip is Silence)
        Console.WriteLine(((Silence)audioClip).milliseconds);
}
AudioPlayer.instance.Play(playlist);

Console.ReadKey();

AudioPlayer.instance.Play(testSentence.GetAudioClip());

Console.ReadKey();