using UnityEngine;

[CreateAssetMenu(fileName = "AudioData", menuName = "Scriptable Objects/AudioData")]
public class AudioData : ScriptableObject
{

    public int audioClipID;
    public AudioClip audioFile;
    public string audioFileName;

    public bool isLooping;
    public bool isEnabled;
    public EventType.E eventToStopOn;
    public EventType.E secondaryEventToStopOn;
    public EventType.E eventToStartOn;

    public void PlayMe(){
        if(!isLooping){
          AudioManager.current.PlaySound(audioClipID);  
        } else{
            AudioManager.current.PlaySoundContinuous(audioClipID);
        }
        
    }

    public void StopMe(){
        AudioManager.current.EndSound(audioClipID);
    }


    
}
