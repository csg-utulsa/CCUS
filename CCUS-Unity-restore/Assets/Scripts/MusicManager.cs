using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    public EventReference MenuMusicEventPath;

    public EventReference LevelMusicEventPath;

    public EventReference soundEventPath;

    private FMOD.Studio.EventInstance MenuMusicInstance;
    
    private FMOD.Studio.EventInstance LevelMusicInstance;
    private FMOD.Studio.EventInstance AmbienceInstance;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;

            // Play ambience
            AmbienceInstance = FMODUnity.RuntimeManager.CreateInstance(soundEventPath);
            DontDestroyOnLoad(gameObject);

            AmbienceInstance.start();
        }
        else
        {
            Destroy(gameObject);
        }
    }
//Below Play____Music methods are called by PlayMusic.cs components which I've placed on Cameras in scenes where music changes. Consider refactoring to merge with sceneTransitionManager once we have it finalized.
    public void PlayMenuMusic(){
        StopMusic(LevelMusicInstance);
        MenuMusicInstance = FMODUnity.RuntimeManager.CreateInstance(MenuMusicEventPath);
        MenuMusicInstance.start();
    }

    public void PlayLevelMusic(){
        StopMusic(MenuMusicInstance);
        LevelMusicInstance = FMODUnity.RuntimeManager.CreateInstance(LevelMusicEventPath);
        LevelMusicInstance.start();
    }
    
    public void StopMusic(FMOD.Studio.EventInstance eventInstance){
    eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    eventInstance.release();
}
    
}