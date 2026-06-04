//Activates the Tutorial tip background and X button
using UnityEngine;

public class TutorialTipGraphicsActivationManager : MonoBehaviour
{
    //Prevents isVisible being externally edited
    private bool isVisible = false;
    public bool IsVisible{
        get{
            return isVisible;
        }
    }

    public GameObject tutorialTipBackground;
    public GameObject tutorialTipXButton;

    void Start(){
        isVisible = tutorialTipBackground.activeSelf;
    }

    public void ActivateTutorialTipBackground(){
        isVisible = true;
        tutorialTipBackground.SetActive(true);
        tutorialTipXButton.SetActive(true);
        GetComponent<FadeChildGraphicsToTransparency>().FadeAllChildGraphicsToTransparency(1f);
        //Also add an intro sizing emphasis

    }

    public void DeactivateTutorialTipBackground(){
        isVisible = false;
        //tutorialTipBackground.SetActive(false);
        //tutorialTipXButton.SetActive(false);
        GetComponent<FadeChildGraphicsToTransparency>().FadeAllChildGraphicsToTransparency(0f);
    }
}
