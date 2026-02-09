/*
* To create a new Tutorial Tip:
*
*  Basically, you just have to make a new tutorial tip text object, then create a corresponding class that defines
*  when it should activate & deactivate. Here are the specifics:
*
* 1) In the Editor, under the Tutorial Tip Manager -> TutorialTipTextObjects, make the new TextMeshPro text object
*      a) IN THE INSPECTOR, add that text object to the tutorialTipTextObjects array of this script.
*      b) IN THE INSPECTOR, also input the time to wait before activating & deactivating the tutorial tips on this script.
*
* 2) Using the base class found in the TutorialTip.cs file, Create a new tutorial Tip class that inherits from the parent TutorialTip class. 
*    Put it in a new script in the folder found at Assets/Scripts/UI Scripts/TutorialTips/TutorialTipTypes
*      a) Further instructions for creating the new class in the TutorialTip script
*
* 3) Attach the new script you created to the associated text object you created.
*
*
*/

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class TutorialTipManager : MonoBehaviour
{
    public TutorialTipGraphicsActivationManager backgroundActivator;
    public TutorialTip[] tutorialTips;
    public GameObject[] tutorialTipTextObjects;
    public float[] timesToWaitBeforeActivating;
    public float[] timesToWaitBeforeDeactivating;
    public float timeToFadeTutorialTip = .25f;
    [SerializeField] private float timeBetweenActivatingSequentialTutorialTips = .5f;
    private List<GameObject> activatedTutorialTips = new List<GameObject>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        backgroundActivator = GetComponent<TutorialTipGraphicsActivationManager>();

        if(GetComponent<FadeChildGraphicsToTransparency>() != null){
            GetComponent<FadeChildGraphicsToTransparency>().timeToFade = timeToFadeTutorialTip;
        }

        tutorialTips = new TutorialTip[tutorialTipTextObjects.Length];
        for(int i = 0; i < tutorialTips.Length; i++){
            tutorialTips[i] = tutorialTipTextObjects[i].GetComponent<TutorialTip>();
            if(tutorialTips[i] != null){
                tutorialTips[i].InitializeValues(i, this, timesToWaitBeforeActivating[i], timesToWaitBeforeDeactivating[i]);
            }
        }

        // new ConnectHousesTutorialTip( 0, this, timesToWaitBeforeActivating[0], timesToWaitBeforeDeactivating[0]);
        // new TreesRemoveCarbonTutorialTip( 1, this, timesToWaitBeforeActivating[1], timesToWaitBeforeDeactivating[1]);
        // new PeopleMakeMoneyTutorialTip( 2, this, timesToWaitBeforeActivating[2], timesToWaitBeforeDeactivating[2]);
        // new ConnectedFactoriesTutorialTip( 3, this, timesToWaitBeforeActivating[3], timesToWaitBeforeDeactivating[3]);
        // new MoreAreaAvailableTutorialTip(4, this, timesToWaitBeforeActivating[4], timesToWaitBeforeDeactivating[4]);

    }

    //Called from a TutorialTip object whose activation conditions have been met
    //Activates text object associated with the TutorialTip object
    public void ActivateTutorialTip(int tutorialTipTextID){

        //Fades tutorial tip background to full opacity
        backgroundActivator.ActivateTutorialTipBackground();

        //Stores the tutorialTipTextObject that needs to be added
        GameObject tipToAdd = tutorialTipTextObjects[tutorialTipTextID];
        
        //Makes sure that the tip to activate is the last element in the activatedTutorialTips list
        if(activatedTutorialTips.Contains(tipToAdd)){
            activatedTutorialTips.Remove(tipToAdd);
        }
        activatedTutorialTips.Insert(0, tipToAdd);

        //Stores the tutorialTipTextObject that needs to be activated
        GameObject tipToActivate = activatedTutorialTips[activatedTutorialTips.Count - 1];

        //Activates the active tutorial tip
        tipToActivate.SetActive(true);

        //Deactivates every other tutorial tip text object
        foreach(GameObject tutorialTipTextObject in tutorialTipTextObjects){
            if(tutorialTipTextObject != tipToActivate){
                tutorialTipTextObject.SetActive(false);
            }
        }

        
    }

    //Called from tutorial tips for when to deactivate their tutorial tip
    public void DeactivateTutorialTip(int tutorialTipTextID){

        //Stores the tutorialTipTextObject that needs to be deactivated
        GameObject tipToDeactivate = tutorialTipTextObjects[tutorialTipTextID];

        bool tipToDeactivateIsTheTopOne = false;
        
        //Makes sure the tip that it is being instructed to deactivate is activated
        if(activatedTutorialTips.Contains(tipToDeactivate)){

            //Checks if the tip to deactivate is the top one
            int activatedTutorialTipIndex = activatedTutorialTips.IndexOf(tipToDeactivate);
            if(activatedTutorialTipIndex == activatedTutorialTips.Count - 1){
                tipToDeactivateIsTheTopOne = true;
            }

            //Removes the tutorial tip to deactivate from the activatedTutorialTips List
            activatedTutorialTips.Remove(tipToDeactivate);
        }

        
        
        //Runs if the deactivated tutorial tip was the visible one
        if(tipToDeactivateIsTheTopOne){

            //Deactivates every other tutorial tip text object
            foreach(GameObject tutorialTipTextObject in tutorialTipTextObjects){
                tutorialTipTextObject.SetActive(false);
            }

            //Fades away the tutorial tip background
            backgroundActivator.DeactivateTutorialTipBackground();

            //If there are more tutorial tips that should be active, it delays activating the next one
            if(activatedTutorialTips.Count > 0){
                ActionDelayer.DelayAction(ActivateTutorialTipBelow, timeToFadeTutorialTip + timeBetweenActivatingSequentialTutorialTips);
            }
        }  

    }

    //Deactivates the tutorial tip that is currently displaying
    public void DeactivateTopTutorialTip(){

        if(activatedTutorialTips.Count > 0){
            int indexOfTipToDeactivate = activatedTutorialTips.IndexOf(activatedTutorialTips[activatedTutorialTips.Count - 1]);
            if(indexOfTipToDeactivate != -1){
                DeactivateTutorialTip(Array.IndexOf(tutorialTipTextObjects, activatedTutorialTips[indexOfTipToDeactivate]));
            }
            
        }
    }

    public void ActivateTutorialTipBelow(){

        //Fades tutorial tip background to full opacity & does the intro-sizing animation
        backgroundActivator.ActivateTutorialTipBackground();

        //Activates the last element in the activatedTutorialTips list
        if(activatedTutorialTips.Count > 0){
            activatedTutorialTips[activatedTutorialTips.Count - 1].SetActive(true);
        }
        
    }

}
