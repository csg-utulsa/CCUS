## Using Audio Resource Map
**Example of this row in the AudioResourceMap.csv** <br>
**"00002,Switch Area Chunks,SwitchArea,1,switched_current_ground_chunk,0"**
- "00002": SoundID of the audio clip for in-code use
- "Switch Area Chunks": Description of when the sound plays
- "SwitchArea": Exact name of the file audio clip. DO NOT INCLUDE FILE EXTENSION
- "1": Audio clip is enabled. If it were 0, it wouldn't be enabled.
- "switched_current_ground_chunk": ID name of the event the audio will be played at.
- "0": Whether or not the sound is looping
- NOTE 1: if the sound is looping, you should add another element with the ID name of the event the audio will be stopped by.
- NOTE 2: Each part of the line must be seperated by a comma, with no spaces.
<br>
<br>
## Adding a New Sound Effect
1. Put the new audio file in the Resources/Audio folder so it's packed at runtime (mp3 format works best).
1. Add a new line in the AudioResourceMap.csv folder based on the description at the top of this README file.
1. Add a new function in the AudioEventLister.cs file that calls audioMan.PlaySound(SoundID). Note: Be sure you are calling that function when you want the audio to play. For example, placing something like this is the Awake() function of the AudioEventListener: GameEventManager.current.CoolEventThatNeedsASound.AddListener(CoolNewSound()).
