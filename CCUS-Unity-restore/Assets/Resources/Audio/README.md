## Using Audio Resource Map
**Example of this row in the AudioResourceMap.csv** <br>
**"00001,Place Tile,PlaceTile,0,TilePlacedWithoutDrag,0,none"**
- "00001": SoundID of the audio clip for in-code use
- "Place Tile": Description of when the sound plays
- "PlaceTile": Exact name of the file audio clip. DO NOT INCLUDE FILE EXTENSION
- "1": Audio clip is enabled. If it were 0, it wouldn't be enabled.
- "TilePlacedWithoutDrag": name of the event the audio will be played on
- "0": Whether or not the sound is looping
- "none": event sound would be stopped on if it were continuous
- NOTE: Each part of the line must be seperated by a comma, with no spaces.
<br>
<br>
## Adding a New Sound Effect
1. Put the new audio file in the Resources/Audio folder so it's packed at runtime (mp3 format works best).
1. Add a new line in the AudioResourceMap.csv folder based on the description at the top of this README file.
Note: You may have to create a new event if one doesn't already exist that fits your needs.
