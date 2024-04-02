using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Bandaid solution: Temporarily deafens the listener so that the player is not absolutely deafened by instantiation sounds on startup
public class DeafenFMODListener : MonoBehaviour
{
    void Start()
    {
        // Disable sound at the beginning
        DisableSound();

        // Invoke the method to enable the listener after a delay 
        Invoke("EnableSound", 1f);
    }

    void DisableSound()
    {
        // Mute all sound by setting the volume of the FMOD master bus to 0
        FMODUnity.RuntimeManager.GetBus("bus:/").setVolume(0);
    }

    void EnableSound()
    {
        // Enable the listener after 2 seconds by restoring the volume of the FMOD master bus
        FMODUnity.RuntimeManager.GetBus("bus:/").setVolume(1);
    }
}
