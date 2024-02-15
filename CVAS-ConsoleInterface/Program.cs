using CVAS.DataStructure;

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

Phrase[] subPhrases = new Phrase("the train on platform one goes to central first stop strathfield then redfern and central").FindSubPhrases(library);

Console.Write("Result: ");
foreach (Phrase subPhrase in subPhrases)
{
    Console.Write($"[{subPhrase.str}] ");
}

Console.ReadKey();

Playlist playlist = new(subPhrases.Select(x => x.linkedAudio).ToArray());
Console.WriteLine("Playing:");

foreach (AudioFile audioFile in playlist.audioFiles)
{
    Console.WriteLine(audioFile.path);
}
AudioPlayer.instance.Play(playlist);

Console.ReadKey();

playlist.Dispose();
playlist = new(subPhrases.Select(x => x.linkedAudio).ToArray());

AudioPlayer.instance.Play(playlist);

Console.ReadKey();