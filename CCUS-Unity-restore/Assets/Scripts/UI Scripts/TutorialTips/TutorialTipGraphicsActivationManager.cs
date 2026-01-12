//Activates the Tutorial tip background and X button
using UnityEngine;

public class TutorialTipGraphicsActivationManager : MonoBehaviour
{
    public GameObject tutorialTipBackground;
    public GameObject tutorialTipXButton;

    public void ActivateTutorialTipBackground(){
        tutorialTipBackground.SetActive(true);
        tutorialTipXButton.SetActive(true);
        GetComponent<FadeChildGraphicsToTransparency>().FadeAllChildGraphicsToTransparency(1f);
        //Also add an intro sizing emphasis


    }

    public void DeactivateTutorialTipBackground(){
        //tutorialTipBackground.SetActive(false);
        //tutorialTipXButton.SetActive(false);
        GetComponent<FadeChildGraphicsToTransparency>().FadeAllChildGraphicsToTransparency(0f);
    }
}
