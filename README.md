

# CVAS

CVAS stands for ***Concatenative Voice Announcement System***. CVAS takes a series of recorded phrases and attempts to speak a given sentence by stringing those phrases together. Think: automatic train announcements.

To download CVAS, check the [Releases](https://github.com/joshua-smith98/CVAS/releases) page for the latest binary.

**Please note: CVAS is currently terminal-only, and only supports Windows.
If you're looking for something cross-platform with a GUI, try this one - its quite nice:** [jaboles/DVA5](https://github.com/jaboles/DVA5)

# Features & Roadmap
## v0.5.0 - Terminal UI & Basic Functions ✔️
 - [x] String together audio files to speak Sentences
 - [x] Test, speak and render Sentences to a file
 - [x] Automatically load a Library of audio files from a folder
 - [x] Support for Middle and Ending Inflections for each Phrase
 - [x] Caching of Libraries for almost instant loading after first analysis
 - [x] Use CVAS via REPL or with Command-line Arguments

## v1.0.0 - WPF GUI & Analysis Tools
 - [ ] **Fully Functional Windows WFP GUI**
 - [ ] ~~Cross-platform Terminal Support for Linux & MacOS~~ **(Still TBD)**
 - [ ] Import Libraries from a folder for quick and easy loading
 - [ ] Save Sentences for easy editing and playback
 - [ ] Analyse a set of Sentences to find the Phrases that are required to say them
 - [ ] Analyse a Library of Phrases to find missing words
 - [ ] *More TBD...*

***
Follow the design and development process more closely [on CVAS's Trello board.](https://trello.com/b/Z1Bclmuy/cvas)

# Installation

## Build with Visual Studio
CVAS comes as a Visual Studio 2022 solution. To build CVAS with Visual Studio:
 1. Clone the repository and open the solution with VS2022 or equivalent.
 2. Ensure CVAS.Main is selected as the startup project.
 3. Build and run the solution, or publish the CVAS.Main project with your required settings.

(It's recommended that you publish as *Self-Contained* and with *"Publish as a Single File"* checked.)

## Build with dotnet
To build CVAS with *dotnet*, ensure you have [.NET 8.0 SDK installed](https://dotnet.microsoft.com/en-us/download), and then use the following commands in the Terminal:

    git clone https://github.com/joshua-smith98/CVAS.git
    cd CVAS/CVAS.Main/
    dotnet publish --sc -p:PublishSingleFile=true

***Note:** CVAS is currently a Windows-Only application. While building on Linux or MacOS with dotnet is technically possible, CVAS will open to a fatal DllNotFoundException. This is because CVAS currently uses [NAudio](https://github.com/naudio/NAudio) for audio rendering and playback, which unfortunately is a Windows-Only library.*

# Usage via REPL
CVAS comes with a REPL interface by default. To use the REPL, simply run *cvas.exe*.
The REPL is designed to be used with the following workflow:

 1. Load a Library.
 2. Test a new sentence, and refine.
 3. 'Say' the sentence.
 4. Render the sentence to a file.

## Loading a Library
Once in the CVAS REPL, use the following command:

    load [path to library]
CVAS will then attempt to load an audio library from the given folder (see [Audio Libraries](https://github.com/joshua-smith98/CVAS/tree/main?tab=readme-ov-file#audio-libraries) below for more information on audio libraries).

*Note that all strings with whitespace must be encased in double-quotes (").*

## Testing a Sentence
After you have loaded a library, you can test a sentence by using the following command:

    test [sentence]
CVAS will then print out all the phrases that are needed to speak that sentence, and highlight any unknown words in red. You can then refine your sentence by using the *test* command again.

CVAS will also remember the last sentence you have used so you can use it again easily in future commands.

## 'Saying' a Sentence
After you have tested a sentence, you may want to have CVAS 'say' it, to check it sounds like you expect. To 'say' a sentence, you can use the following command:

    say [sentence]
Or, if you have used your sentence before in another command, you do not need to provide it again:

    say
## Rendering a Sentence to a File
Finally, you may want to render a sentence to a file for external use (note that currently only wav is supported). To render a sentence, you can use the following command:

    render [path] [sentence]
And again, if you have used a sentence before, you do not need to provide it again:

    render [path]
***
For a full list of REPL commands and their usage, use:

    help

# Usage via Command-line Arguments
CVAS can also be used via command-line arguments. CVAS's command-line syntax is as follows:

    cvas.exe -[option] (argument) -[option] (argument) -[option] (argument) ...
Using command-line arguments, you can play and render sentences.

## Playing a Sentence via Command-line
To play a sentence via command-line, you can use the following options:

    cvas.exe -library [path to library] -sentence [sentence] -play
*Note that the order of the options is not important.*

## Rendering a Sentence via Command-line
To render a sentence to a wav file via Command-line, you can use the following options:

    cvas.exe -library [path to library] -sentence [sentence] -output [path to output file] -render
***
For a full list of Command-line arguments and their functions, use:

    cvas.exe -help

# Audio Libraries
CVAS requires you to load an audio library before you can process any sentences. Libraries in CVAS are just folders containing specially named audio files.

The required formatting of audio file names is based on that of [jaboles/DVA5](https://github.com/jaboles/DVA5) to allow for cross-compatibility:

    [words contained in file](.f)[.mp3|.wav|.ogg|etc.]
*Where the presence of '.f' specifies that the phrase contained in the file ends a sentence.*

For example: a file containing the phrase "train stopping all stations", which ends a sentence would be named:

    train stopping all stations.f.mp3
Whereas a file containing the phrase "the train arriving on platform", which does not end a sentence would be named:

    the train arriving on platform.mp3
*Note that MP3 files are used here as an example only. CVAS supports almost all audio file formats. For more information about supported audio formats, see [naudio/NAudio](https://github.com/naudio/NAudio)*.

For more examples of valid file names, you can take a look inside the 'sounds' folder of [jaboles/DVA5](https://github.com/jaboles/DVA5). All audio file names in DVA5 are compatible with CVAS.
