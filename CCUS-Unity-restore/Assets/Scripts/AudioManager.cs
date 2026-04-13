/*
*
*   NOTE: Audio files must be in same folder as AudioResourceMap.csv to work.
*
*
*/



using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class AudioManager : MonoBehaviour
{
    public TextAsset AudioResourceMap;

    public static AudioManager current;
    public bool EnableSoundEffects = true;

    //Keeps an array of all the AudioSource gameobjects attached to this object
    public AudioSource[] audioSources;

    //Keeps track of all the audio sources that are currently playing a clip
    private List<OccupiedAudioSource> occupiedAudioSources = new List<OccupiedAudioSource>();

    //Keeps track of all the audio sources that are playing a clip on loop
    private List<AudioSource> loopingAudioSources = new List<AudioSource>();

    //List of all audio files
    private AudioClip[] audioClips;
    //Array corresponding to audioClips array that keeps track of each sound's ID number
    private int[] audioClipSoundIDs;
    //Array corresponding to audioClips array that keeps track of whether or not each clip is enabled
    private bool[] audioClipsEnabled;



    public int maxSoundsPlaying = 50;

    // Creates singleton
    // Note: Because "current" is assigned in the awake function, the earliest sounds can be played 
    //       by referencing the Singleton is in the Start() function.
    void Awake()
    {
        //Reads AudioResourceMap.csv and stores data
        LoadAudioResourceMap();

        if(current == null){
            current = this;
        } else{
            Destroy(this);
        }
    }

    //Plays a sound once.
    public void PlaySound(int soundID){

        //Prevents unplayable clips from playing
        if(!IsClipPlayable(soundID)) return;

        //Creates or assigns an audio source for the sound
        AudioSource audioSource = AssignAudioSourceToClip(soundID);

        audioSource.Play();

        AudioClip audioClip = GetAudioClipByID(soundID);

        //Clears audio source of its resource when it's done playing its sound
        StartCoroutine(WaitToClearAudioSource(audioSource, audioClip.length));
        
    }

    //Starts playing a sound on loop
    public void PlaySoundContinuous(int soundID){

        //Prevents unplayable clips from playing
        if(!IsClipPlayable(soundID)) return;

        //Creates or assigns an audio source for the sound
        AudioSource audioSource = AssignAudioSourceToClip(soundID);

        audioSource.Play();
        
        audioSource.loop = true;

    }

    //Forces all sounds of specified ID to stop playing
    public void EndSound(int soundID){
        AudioSource[] sourcesToStop = FindAudioSourcesPlayingClip(soundID);
        foreach(AudioSource source in sourcesToStop){
            StopAndClearAudioSource(source);
        }
    }

    //Assigns an audio source to the given sound clip
    private AudioSource AssignAudioSourceToClip(int soundID){

        //Makes sure there are enough free/unoccupied audio sources to play sound
        AudioSource[] unoccupiedSources = GetUnoccupiedAudioSources();
        if(unoccupiedSources.Length <= 0){
            DoubleNumberOfAudioSources();
            unoccupiedSources = GetUnoccupiedAudioSources();
            if(unoccupiedSources.Length <= 0){
                Debug.LogError("Hit the max number of sounds! Try increasing the maxSoundsPlaying int on this script.");
            }
        }

        //Chooses the first unused audio source to play sound
        AudioSource audioSource = unoccupiedSources.FirstOrDefault();

        //Finds audio clip by its ID number & makes it the resource of the audioSource
        AudioClip audioClip = GetAudioClipByID(soundID);
        audioSource.resource = audioClip;

        //Keeps track of all the audio sources that are occupied
        occupiedAudioSources.Add(new OccupiedAudioSource(audioSource, soundID));

        //Returns the audio source that was just created
        return audioSource;

    }

    // Clears an audio source once it's done playing its sound
    private IEnumerator WaitToClearAudioSource(AudioSource audioSource, float waitTime){

        //Waits for waitTime seconds before clearing audio source
        yield return new WaitForSeconds(waitTime);

        // Stops and clears audio source
        StopAndClearAudioSource(audioSource);
        
        
    }

    //Clears an audio source and forces it stop playing its sound
    private void StopAndClearAudioSource(AudioSource audioSource){
        audioSource.Stop();
        audioSource.loop = false;;
        audioSource.resource = null;

        //removes the audio source from occupied audio source
        OccupiedAudioSource  sourceToRemove = occupiedAudioSources.Find(x => (x.audioSource == audioSource));
        occupiedAudioSources.Remove(sourceToRemove);
    }

    // Increases number of audio sources if needed
    private void DoubleNumberOfAudioSources(){

        //TODO: Finish Method

        //Limits number of audio sources that can be created.
        if(audioSources.Length * 2 < maxSoundsPlaying){
            
            //Case for when there are no audio sources. Adds a single audio source
            if(audioSources.Length <= 0){
                AudioSource audioSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
                audioSources = new AudioSource[1];
                audioSources[0] = audioSource;
            }

            //Doubles length of array
            else{
                int newNumOfAudioSources = (audioSources.Length * 2);
                int numberOfSourcesToAdd = (audioSources.Length * 2) - audioSources.Length;

                AudioSource[] newAudioSources = new AudioSource[newNumOfAudioSources];
                int[] newListOfAudioClipsBeingPlayed = new int[newNumOfAudioSources];
                
                Array.Copy(audioSources, newAudioSources, audioSources.Length);

                //Creates new audio sources 
                for(int i = audioSources.Length; i < newNumOfAudioSources; i++){
                    newAudioSources[i] = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
                }

                //Replaces old array with new array
                audioSources = newAudioSources;
            }
            
        }
    }

    //returns all AudioSource components that are playing the given soundID
    private AudioSource[] FindAudioSourcesPlayingClip(int soundID){

        if(soundID == 0){
            return null;
        }

        //Finds all occupied audio sources playing the given sound
        List<OccupiedAudioSource> sourcesToRemoveList = occupiedAudioSources.FindAll(x => (x.soundID == soundID));

        //Converts OccupiedAudioSource list to AudioSource list
        List<AudioSource> sourcesToRemoveListUnwrapped = new List<AudioSource>();
        foreach(OccupiedAudioSource oac in sourcesToRemoveList){
            sourcesToRemoveListUnwrapped.Add(oac.audioSource);
        }
        AudioSource[] sourcesToRemoveArray = sourcesToRemoveListUnwrapped.ToArray();

        //Returns all the sources
        return sourcesToRemoveArray;
    }

    //Returns all audio sources that are currently playing a sound
    private AudioSource[] GetOccupiedAudioSources(){
        int numOfSources = occupiedAudioSources.Count;
        AudioSource[] occupiedSourcesAsArray = new AudioSource[numOfSources];
        for(int i = 0; i < numOfSources; i++){
            occupiedSourcesAsArray[i] = occupiedAudioSources[i].audioSource;
        }
        return occupiedSourcesAsArray;
    }

    //Returns all audio sources that aren't currently playing a sound
    private AudioSource[] GetUnoccupiedAudioSources(){

        List<AudioSource> audioSourceList = audioSources.Except(GetOccupiedAudioSources()).ToList();
        AudioSource[] unoccupiedSources = audioSourceList.ToArray();
        return unoccupiedSources;
    }

    private AudioClip GetAudioClipByID(int soundID){
        return audioClips[Array.IndexOf(audioClipSoundIDs, soundID)];
    }

    private bool IsClipPlayable(int soundID){
        return audioClipsEnabled[Array.IndexOf(audioClipSoundIDs, soundID)];
    }

    //Reads audio resource data from csv file
    private void LoadAudioResourceMap(){
        try{
            
            //Gets raw data from audio resource map as string
            String rawMapData  = AudioResourceMap.text;

            //Splits data into rows, since each row has the info for a single audio clip
            string[] dataRows = rawMapData.Split('\n');

            //Declares arrays for soundIDs, Audio Clips, and for clips enabled/disabled state
            int[] _audioSoundClipIDs = new int[dataRows.Length];
            AudioClip[] _audioClips = new AudioClip[dataRows.Length];
            bool[] _audioClipsEnabled = new bool[dataRows.Length];

            //Pulls the required data from each row
            for(int i = 0; i < dataRows.Length; i++){
                String[] rowElements = dataRows[i].Split(',');
                if(rowElements.Length != 4){
                    Debug.LogError("Error with audio resource map formatting. (Check AudioResourceMap.csv and make sure it follows the formatting found in the accompanying ReadMe.md file)");
                    continue;
                }

                //Pulls soundID from the first element in the row
                string soundIDAsString = rowElements[0];

                //Trys to convert the sound ID string to an integer
                if (int.TryParse(soundIDAsString, out int soundID)){
                    if(_audioSoundClipIDs.Contains(soundID)){
                        Debug.LogError("DUPLICATE SOUND CLIP ID NUMBER IN AUDIO RESOURCE MAP!!!! (THIS WILL BREAK THINGS)");
                    }
                    _audioSoundClipIDs[i] = soundID;
                } else{
                    Debug.LogError("Error with sound ID from audio resource map");
                }

                //Pulls whether or not audio clip is enabled from the 4th element in the row
                string audioClipEnabledRaw = rowElements[3];

                //If 4th element in row isn't 0, audio clip is enabled
                if(!audioClipEnabledRaw.Contains('0')){
                    _audioClipsEnabled[i] = true;
                } else{
                    _audioClipsEnabled[i] = false;
                }

                //Pulls the path of the audio file from the 3rd element in the row
                //Note: "Audio/" is the parent folder of all audio assets
                string audioClipPath = "Audio/" + rowElements[2];

                //Tries to pull the audio clip using the provided path
                try{
                    _audioClips[i] = Resources.Load<AudioClip>(audioClipPath);
                } catch{
                    Debug.LogError("Failure getting soundID at path " + audioClipPath);
                }
                
            }

            //Stores data from the csv file
            audioClips = _audioClips;
            audioClipSoundIDs = _audioSoundClipIDs;
            audioClipsEnabled = _audioClipsEnabled;

        } catch{
            Debug.LogError("Problem loading Audio Resource Map.");
            audioClips = new AudioClip[0];
            audioClipSoundIDs = new int[0];
            audioClipsEnabled = new bool[0];
        }
        

    }

}

//Object to hold information about an audio source when it's currently playing a sound
public class OccupiedAudioSource{
    public AudioSource audioSource = null;
    public bool isOccupied = false;
    public int soundID = 0;

    //Constructor (blank)
    public OccupiedAudioSource(AudioSource _audioSource){
        audioSource = _audioSource;
        isOccupied = false;
        soundID = 0;
    }

    //Constructor that lets you pass the id of the audio clip being played.
    public OccupiedAudioSource(AudioSource _audioSource, int _soundID){
        audioSource = _audioSource;
        isOccupied = true;
        soundID = _soundID;
    }
}