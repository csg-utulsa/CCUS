using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    public EventReference MusicEventPath;

    public EventReference soundEventPath;

    private FMOD.Studio.EventInstance musicEventInstance;
    private FMOD.Studio.EventInstance soundEventInstance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            // Play the first sound event
            musicEventInstance = FMODUnity.RuntimeManager.CreateInstance(MusicEventPath);

            // Play the second sound event
            soundEventInstance = FMODUnity.RuntimeManager.CreateInstance(soundEventPath);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        musicEventInstance.start();
        soundEventInstance.start();
    }
}