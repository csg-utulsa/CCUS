// Plays the rising sound effect when the people button is pressed.
// It's separate from the audio system so as to control the pitch of the sound.

using UnityEngine;

public class PeopleButtonRisingSoundController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip risingSoundClip;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        GameEventManager.current.GetEvent(EventType.E.BeginFillingPeopleButton).AddListener(StartSound);
        GameEventManager.current.GetEvent(EventType.E.FinishFillingPeopleButton).AddListener(EndSound);
        GameEventManager.current.GetEvent(EventType.E.PeopleButtonReleased).AddListener(EndSound);

        //Adds audioSource if there's not one on the object.
        if(audioSource == null){
            audioSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        }

        //Assigns sound clip to audio source
        if(audioSource != null){
            if(risingSoundClip != null){
                audioSource.clip = risingSoundClip;
            } else{
                Debug.LogError("Missing the PeopleButtonRise clip on AudioManager>PeopleButtonRisingSFXController");
            }
        } else{
            Debug.LogError("Missing audio source on AudioManager>PeopleButtonRisingSFXController");
        }
    }

    private void StartSound(){

        //What percent the people button is filled
        float percentageFilled = PeopleManager.current.PercentFilled;

        //Calculates at what time the clip should be starting at
        float clipTimeStamp = percentageFilled * PressPeopleButton.current.maxTimeToPressButtonDown;

        if(audioSource != null){
            audioSource.time = clipTimeStamp;
            audioSource.Play();
        } else{
            Debug.LogError("Missing audioSource");
        }

    }

    private void EndSound(){
        if(audioSource != null){
            audioSource.Stop();
        } else{
            Debug.LogError("Missing audioSource");
        }
    }


}
