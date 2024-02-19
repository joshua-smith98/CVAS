using CVAS.DataStructure;
using CVAS.AudioEngine;

// Phrase algorithm test
List<Phrase> Phrases = new List<Phrase>
{
    new("the train on platform", new AudioFileStreaming("sounds/the train on platform.mp3")),
    new("one", new AudioFileStreaming("sounds/one.mp3")),
    new("goes to", new AudioFileStreaming("sounds/goes to.mp3")),
    new("the train on platform one", new AudioFileStreaming("sounds/the train on platform one.mp3")),
    new("central", new AudioFileStreaming("sounds/central.mp3")),
    new("first stop", new AudioFileStreaming("sounds/first stop.mp3")),
    new("strathfield", new AudioFileStreaming("sounds/strathfield.mp3")),
    new("then", new AudioFileStreaming("sounds/then.mp3")),
    new("redfern", new AudioFileStreaming("sounds/redfern.mp3")),
    new("and", new AudioFileStreaming("sounds/and.mp3")),
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
    if (audioClip is AudioFileStreaming)
        Console.WriteLine(((AudioFileStreaming)audioClip).path);
    else if (audioClip is Delay)
        Console.WriteLine(((Delay)audioClip).milliseconds);
}
AudioPlayer.instance.Play(playlist);

Console.ReadKey();

playlist = new(subPhrases.Select(x => x.linkedAudio).ToArray()); // Playing it a second time to test overlapping audio

AudioPlayer.instance.Play(playlist);

Console.ReadKey();