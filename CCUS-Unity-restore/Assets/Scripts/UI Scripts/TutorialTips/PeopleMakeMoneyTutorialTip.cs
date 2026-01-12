using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// "Each person added to a home makes more money" Tutorial tip
public class PeopleMakeMoneyTutorialTip : TutorialTip
{
    //Constructor passes values to base class
    public PeopleMakeMoneyTutorialTip(int _tutorialTipTextID, TutorialTipManager _TTM, float _timeToWaitBeforeActivating, float _timeToWaitBeforeDeactivating) : base(_tutorialTipTextID, _TTM, _timeToWaitBeforeActivating, _timeToWaitBeforeDeactivating){
        //Checks if tip should be activated every time a progress event is called
        GameEventManager.current.ProgressEventJustCalled.AddListener(CheckIfTipShouldBeActivated);

        //Deactivates the tip when a person is added
        GameEventManager.current.PersonJustAdded.AddListener(CheckIfTipShouldBeDeactivated);
    }



    public void CheckIfTipShouldBeActivated(){
        //Checks that the progress event was people being unlocked

        if(GameEventManager.current.TypeOfLastProgressEventCalled == ProgressionManager.ProgressEventType.PeopleUnlocked){

            //Activates tutorial tip if no people have been added 
            if(!AtLeastOnePersonAdded()){

                ActivateTutorialTip();
            }
        }
    }

    public void CheckIfTipShouldBeDeactivated(){
        DeactivateTutorialTip();
    }

    //Checks if at least one person has been added
    private bool AtLeastOnePersonAdded(){
        return (PeopleManager.current.NumberOfPeople >= 1);
    }

}



