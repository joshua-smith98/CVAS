using CVAS.DataStructure;
using CVAS.AudioEngine;

// Phrase algorithm test
List<Phrase> Phrases = new List<Phrase>
{
    new("the train on platform", new AudioFile("sounds/the train on platform.mp3")),
    new("one", new AudioFile("sounds/one.mp3")),
    new("goes to", new AudioFile("sounds/goes to.mp3")),
    new("the train on platform one", new AudioFile("sounds/the train on platform one.mp3")),
    new("central", new AudioFile("sounds/central.mp3")),
    new("first stop", new AudioFile("sounds/first stop.mp3")),
    new("strathfield", new AudioFile("sounds/strathfield.mp3")),
    new("then", new AudioFile("sounds/then.mp3")),
    new("redfern", new AudioFile("sounds/redfern.mp3")),
    new("and", new AudioFile("sounds/and.mp3")),
};

Library library = new(Phrases.ToArray());

Phrase[] subPhrases = new Phrase("The train on platform | one goes to Strathfield. First stop Central, then Redfern, Central, and Strathfield").FindSubPhrases(library);

Console.Write("Result: ");
foreach (Phrase subPhrase in subPhrases)
{
    Console.Write($"[{subPhrase.str}] ");
}

// Audio engine test
Console.ReadKey();

Playlist playlist = new Playlist(subPhrases.Select(x => x.linkedAudio).ToArray());
Console.WriteLine("Playing:");

foreach (IAudioClip audioClip in playlist.audioClips)
{
    if (audioClip is AudioFile)
        Console.WriteLine(((AudioFile)audioClip).path);
    else if (audioClip is Delay)
        Console.WriteLine(((Delay)audioClip).milliseconds);
}
AudioPlayer.instance.Play(playlist);

Console.ReadKey();

playlist = new(subPhrases.Select(x => x.linkedAudio).ToArray()); // Playing it a second time to test overlapping audio

AudioPlayer.instance.Play(playlist);

Console.ReadKey();