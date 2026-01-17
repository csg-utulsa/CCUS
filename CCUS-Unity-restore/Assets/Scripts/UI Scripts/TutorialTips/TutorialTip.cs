/*
* To create a new TutorialTip child class:
*
* 1) In the child class's constructor, pass the constructor values to the base class
* 2) In the child class, create override methods for CheckIfTipShouldBeActivated() and CheckIfTipShouldBeDeactivated()
        a) In the override functions, you can turn the tips off an on using ActivateTutorialTip() and DeactivateTutorialTip()
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTip : MonoBehaviour
{
    protected bool tutorialTipIsActivated = false;
    private bool displayOnce = true;
    private bool alreadyDisplayed = false;
    private int tutorialTipTextID = 0;
    protected float timeToWaitBeforeActivating = 0f;
    protected float timeToWaitBeforeDeactivating = 0f;
    protected bool tutorialTipConditionMet = false;
    TutorialTipManager TTM;

    //In child classes, do something like this: "public ConnectHousesTutorialTip(_tutorialTipTextID, _TTM, _timeToWaitBeforeActivating, _timeToWaitBeforeDeactivating) : base(int _tutorialTipTextID, TutorialTipManager _TTM, float _timeToWaitBeforeActivating, float _timeToWaitBeforeDeactivating){}
    public TutorialTip(int _tutorialTipTextID, TutorialTipManager _TTM, float _timeToWaitBeforeActivating, float _timeToWaitBeforeDeactivating){
        //By default, the tutorial tips display only once
        displayOnce = true;
        tutorialTipTextID = _tutorialTipTextID;
        TTM = _TTM;
        timeToWaitBeforeActivating = _timeToWaitBeforeActivating;
        timeToWaitBeforeDeactivating = _timeToWaitBeforeDeactivating;
        
    }

    public void InitializeValues(int _tutorialTipTextID, TutorialTipManager _TTM, float _timeToWaitBeforeActivating, float _timeToWaitBeforeDeactivating){
        //By default, the tutorial tips display only once
        displayOnce = true;
        tutorialTipTextID = _tutorialTipTextID;
        TTM = _TTM;
        timeToWaitBeforeActivating = _timeToWaitBeforeActivating;
        timeToWaitBeforeDeactivating = _timeToWaitBeforeDeactivating;

        InitializeThisTutorialTip();

        
    }

    public virtual void InitializeThisTutorialTip(){

    }

    //Checks if the Tutorial Tip Should be activated. Create overrid method in child classes
    // public virtual void CheckIfTipShouldBeActivated(){

    // }

    //Checks if the Tutorial Tip Should be activated. Create overrid method in child classes
    // public virtual void CheckIfTipShouldBeActivated(){

    // }

    //Checks if the Tip should be deactivated. Create Override method in child classes
    // public virtual void CheckIfTipShouldBeDeactivated(){

    // }
    
    //Removes any Unity event listeners on object
    public virtual void RemoveAllListeners(){

    }

    protected void ActivateTutorialTip(){
        tutorialTipConditionMet = true;
        ActionDelayer.DelayAction(DelayedActivateTutorialTip, timeToWaitBeforeActivating);
    }

    private void DelayedActivateTutorialTip(){
        if((!displayOnce || !alreadyDisplayed) && !tutorialTipIsActivated && tutorialTipConditionMet){
            tutorialTipIsActivated = true;
            TTM.ActivateTutorialTip(tutorialTipTextID);
        }
    }
    protected void DeactivateTutorialTip(){
        tutorialTipConditionMet = false;
        ActionDelayer.DelayAction(DelayedDeactivateTutorialTip, timeToWaitBeforeDeactivating);
    }

    private void DelayedDeactivateTutorialTip(){
        if(tutorialTipIsActivated && !tutorialTipConditionMet){
            tutorialTipIsActivated = false;
            TTM.DeactivateTutorialTip(tutorialTipTextID);
            if(displayOnce){

                //Destroy(this);
            }
        }
    }


}








// public class TemplateTutorialTipClass : TutorialTip
// {
//     //Constructor passes values to base class
//     public TemplateTutorialTipClass(int _tutorialTipTextID, TutorialTipManager _TTM, float _timeToWaitBeforeActivating, float _timeToWaitBeforeDeactivating) : base(_tutorialTipTextID, _TTM, _timeToWaitBeforeActivating, _timeToWaitBeforeDeactivating){
//     }

//     public override void CheckIfTipShouldBeActivated(){
//         if(!TutorialTipAppearanceCondition()){
//             ActivateTutorialTip();
//         }
//     }

//     public override void CheckIfTipShouldBeDeactivated(){
//         if(TutorialTipAppearanceCondition()){
//             DeactivateTutorialTip();
//         }
//     }

//     private bool TutorialTipAppearanceCondition(){
//         return true;
//     }

// }