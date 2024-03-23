
# CVAS

CVAS stands for ***Concatenative Voice Announcement System***. CVAS takes a series of recorded phrases and attempts to speak a given sentence by stringing those phrases together. Think: automatic train announcements.

**Please note: CVAS is currently terminal-only.
If you're looking for a program with UI, try this one - its quite nice:** [jaboles/DVA5](https://github.com/jaboles/DVA5)
## Installation
*For a binary, check releases.*

### Build with Visual Studio
CVAS comes as a Visual Studio 2022 solution. To build CVAS with Visual Studio:
 1. Clone the repository and open the solution with VS2022 or equivalent.
 2. Ensure CVAS.Main is selected as the startup project.
 3. Build and run the solution, or publish the CVAS.Main project with your required settings.

(It's recommended that you publish as *Self-Contained* and with *"Publish as a Single File"* checked.)

### Build with dotnet
To build CVAS with *dotnet*, ensure you have [.NET 8.0 SDK installed](https://dotnet.microsoft.com/en-us/download), and then use the following commands in the Terminal:

    git clone https://github.com/joshua-smith98/CVAS.git
    cd CVAS/CVAS.Main/
    dotnet publish --sc -p:PublishSingleFile=true

***Note:** CVAS is currently a Windows-Only application. While building on Linux with dotnet is technically possible, CVAS will open to a fatal DllNotFoundException. This is because CVAS uses [NAudio](https://github.com/naudio/NAudio) for audio rendering and playback, which unfortunately is a Windows-Only library.*

## Usage
TODO
## Roadmap
### v0.5.0 - Back-end, Command-line & README.md ✔️
 - [x] Sentence, phase & library functionality
 - [x] Audio playback of sentences and phrases
 - [x] Audio caching
 - [x] Inflection support
 - [x] Automatically load a library from a folder
 - [x] Render spoken sentences to files
 - [x] Library caching on the disk
 - [x] Perform functions via REPL
 - [x] Perform functions via command line
 - [x] A completed README.md, with "Usage" and "Installation" sections.

### v1.0.0 - WPF GUI & Analysis Tools
- This will be expanded once v0.5.0 is complete.

***
Follow the design and development process more closely [on CVAS's Trello board.](https://trello.com/b/Z1Bclmuy/cvas)
