/*
* To create a new Tutorial Tip:
*
*  Basically, you just have to make a new tutorial tip text object, then create a corresponding class that defines
*  when it should activate & deactivate. Here are the specifics:
*
* 1) In the Editor, under the Tutorial Tip Manager -> TutorialTipTextObjects, make the new text object
*    a) IN THE INSPECTOR, add that text object to the tutorialTipTextObjects array of this script.
*    b) IN THE INSPECTOR, also input the time to wait before activating & deactivating the tutorial tips on this script.
*
* 2) IN  THE TutorialTip.cs file, Create a new tutorial Tip class that inherits from the parent TutorialTip class. Add it it the bottom of the TutorialTip.cs file
*    a) Further instructions for creating the new class in the TutorialTip script
*
* 3) In the Start() function of this script, create an instance of the new TutorialTip child class you just made. 
*    a) Do so by editing this line: new ConnectHousesTutorialTip( 0, this, timesToWaitBeforeActivating[0], timesToWaitBeforeDeactivating[0]);
*       i) Update to the name of the new class you created
*        ii) Update the numbers from 0
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
    public GameObject[] tutorialTipTextObjects;
    public float[] timesToWaitBeforeActivating;
    public float[] timesToWaitBeforeDeactivating;
    private List<GameObject> activatedTutorialTips = new List<GameObject>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        backgroundActivator = GetComponent<TutorialTipGraphicsActivationManager>();

        new ConnectHousesTutorialTip( 0, this, timesToWaitBeforeActivating[0], timesToWaitBeforeDeactivating[0]);
        new TreesRemoveCarbonTutorialTip( 1, this, timesToWaitBeforeActivating[1], timesToWaitBeforeDeactivating[1]);
        new PeopleMakeMoneyTutorialTip( 2, this, timesToWaitBeforeActivating[2], timesToWaitBeforeDeactivating[2]);
        new ConnectedFactoriesTutorialTip( 3, this, timesToWaitBeforeActivating[3], timesToWaitBeforeDeactivating[3]);

    }

    //Called from a TutorialTip object whose activation conditions have been met
    //Activates text object associated with the TutorialTip object
    public void ActivateTutorialTip(int tutorialTipTextID){

        //Fades tutotial tip background to full opacity & does the intro-sizing animation
        backgroundActivator.ActivateTutorialTipBackground();

        //Stores the tutorialTipTextObject that needs to be activated
        GameObject tipToActivate = tutorialTipTextObjects[tutorialTipTextID];
        
        //Makes sure that the tip to activate is the last element in the activatedTutorialTips list
        if(activatedTutorialTips.Contains(tipToActivate)){
            activatedTutorialTips.Remove(tipToActivate);
        }
        activatedTutorialTips.Add(tipToActivate);

        //Activates the active tutorial tip
        tipToActivate.SetActive(true);

        //Deactivates every other tutorial tip text object
        foreach(GameObject tutorialTipTextObject in tutorialTipTextObjects){
            if(tutorialTipTextObject != tipToActivate){
                tutorialTipTextObject.SetActive(false);
            }
        }

        
    }

    public void DeactivateTutorialTip(int tutorialTipTextID){

        //Stores the tutorialTipTextObject that needs to be deactivated
        GameObject tipToDeactivate = tutorialTipTextObjects[tutorialTipTextID];
        
        //Removes the tutorial tip to deactivate from the activatedTutorialTips List
        if(activatedTutorialTips.Contains(tipToDeactivate)){
            activatedTutorialTips.Remove(tipToDeactivate);
        }

        //Deactivates every other tutorial tip text object
        foreach(GameObject tutorialTipTextObject in tutorialTipTextObjects){
            tutorialTipTextObject.SetActive(false);
        }
        
        if(activatedTutorialTips.Count > 0){
            //Activates the last element in the activatedTutorialTips list
            activatedTutorialTips[activatedTutorialTips.Count - 1].SetActive(true);
        } else{
            //Deactivates the tutorial tip background if there are no activated Tutorial Tip Text Objects
            backgroundActivator.DeactivateTutorialTipBackground();
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

}
