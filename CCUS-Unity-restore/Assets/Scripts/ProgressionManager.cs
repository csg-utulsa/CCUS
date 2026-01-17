using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProgressionManager : MonoBehaviour
{
    public static ProgressionManager PM;

    //Stores all the buttons, to be added or removed. Assign in inspector.
    public buttonScript[] buttons;

    public bool[] progressEventHasOccurred;

    //Stores the type of the last progress event that was called
    public ProgressEventType TypeOfLastProgressEventCalled {get; set;}

    //progressEvents defines all the progress events
    /*
    Instructions for use:
        In order to add a Progress Event, add a new line that begins with new ProgressEvent(), followed by a comma.
        Each time you create a new progress event, you have to pass in three new parameters to it, which are:
            1) the condition, 2) the action, 3) and the time until excecution

        Define conditions using Lambda notation, like this: () => (Insert some boolean condition)
            Example: () => LevelManager.LM.money > 1000;
                This condition, when money is greater than 1000, would activate the progress event 

        You also define actions using Lambda notation, like this: () => (Insert some action)

        The final input is the Delay Until Excecution, which tells the program how long to wait to excecute the
        action after the condition is met.

        ALSO BE SURE TO UPDATE the enum ProgressEventType WHEN YOU ADD A NEW PROGRESS EVENT.
    */
    public ProgressEvent[] progressEvents => new ProgressEvent[]{

        //DO NOT DELETE A PROGRESS EVENT
        // Several other places are dependant on their exact position in the array.
        // If you need to add a new one, add it to the end of the array.
        // If you need to disable one, replace the first parameter with "() => false"
        // If you need to rearrange them, just also rearrange the enum ProgressEventType

        //Event 0: Enables the people panel when you fix the maxed out carbon the first time
        new ProgressEvent(() => progressEventHasOccurred[1] && !LevelManager.overMaxCarbon(), () => {PeoplePanel._peoplePanel.EnablePeoplePanel();}, 25f),
        
        //Event 1: Adds the tree and grass buttons the first time you max out on carbon.
        new ProgressEvent(() => LevelManager.overMaxCarbon(), () => {TileSelectPanel.TSP.AddButton(buttons[0]); TileSelectPanel.TSP.AddButton(buttons[1]);}, 5f),

        //Event 2: Adds apartment
        new ProgressEvent(() => LevelManager.LM.NetMoney > 80, () => {TileSelectPanel.TSP.AddButton(buttons[3]);}, 1.5f),

        //Event 3: Unlocks carbon capture systems and increases max number of carbon capture systems to 5
        new ProgressEvent(() => LevelManager.LM.NetMoney > 1300, () => {TileSelectPanel.TSP.AddButton(buttons[4]);}, 0f),

        //Event 4: Add Factories
        new ProgressEvent(() => LevelManager.LM.NetMoney > 800, () => {TileSelectPanel.TSP.AddButton(buttons[5]);}, 0f),

        //Event 5: Adds More Ground
        new ProgressEvent(() => LevelManager.LM.NetMoney > 650, () => {GroundAreaExpansion.GAE.AddGroundChunk();}, 0f),

        //Event 6: Unlock Wind Turbines
        new ProgressEvent(() => LevelManager.LM.NetMoney > 1700, () => {TileSelectPanel.TSP.AddButton(buttons[6]);}, 0f),

        //Event 7: Unhides the Carbon Dial when you max out the carbon
        new ProgressEvent(() => LevelManager.overMaxCarbon(), () => {CarbonDial.current.UnhideCarbonDial();}, 0f),

        //Event 2: Adds Roads when you fix the maxed out carbon the first time
        //new ProgressEvent(() => progressEventHasOccurred[1] && !LevelManager.overMaxCarbon(), () => {TileSelectPanel.TSP.AddButton(buttons[2]);}, 3f),
    
        
    };

    public enum ProgressEventType
    {
        PeopleUnlocked,
        TreesAndGrassUnlocked,
        ApartmentsUnlocked,
        CarbonCaptureSystemsUnlocked,
        FactoriesUnlocked,
        NewGroundUnlocked,
        
    }

    
    
    void Start(){
        
        if(PM == null){
            PM = this;
        } else{
            Destroy(this);
        }

        TickManager.TM.PollutionTick.AddListener(OnPollutionTick);
        TickManager.TM.MoneyTick.AddListener(OnMoneyTick);
        progressEventHasOccurred = new bool[progressEvents.Length];
        for(int i = 0; i < progressEventHasOccurred.Length; i++){
            progressEventHasOccurred[i] = false;
        }
    }

    void OnMoneyTick(){
        CheckProgressEventConditions();
    }
    void OnPollutionTick(){
        CheckProgressEventConditions();
    }

    public void CallProgressEvents(int[] progressEventsToCall){
        
        foreach(int progressEventToCall in progressEventsToCall){
            if(progressEventToCall < progressEvents.Length){
                progressEvents[progressEventToCall].ProgressionAction();
                progressEventHasOccurred[progressEventToCall] = true;
            }
        }
        
    }

    private void CheckProgressEventConditions(){
        
        for(int i = 0; i < progressEvents.Length; i++){
            if(!progressEventHasOccurred[i] && progressEvents[i].ProgressionCondition()){
                float delayTime = progressEvents[i].TimeTillExcecution;
                if(delayTime > 0){
                    StartCoroutine(delayProgressionEvent(progressEvents[i].TimeTillExcecution, i));
                    
                }else{
                    CallAProgressEvent(i);
                }
                progressEventHasOccurred[i] = true;
            }
        }
    }

    private IEnumerator delayProgressionEvent(float delayTime, int progressEventToCall){
        yield return new WaitForSeconds(delayTime);
        CallAProgressEvent(progressEventToCall);
    }

    private void CallAProgressEvent(int progressEventToCall){
        //Debug.Log("Called progress event number " + Array.IndexOf(progressEvents, progressEvent));
        progressEvents[progressEventToCall].ProgressionAction();
        TypeOfLastProgressEventCalled =  (ProgressEventType) progressEventToCall;
        GameEventManager.current.ProgressEventJustCalled.Invoke();

    }



    // Update is called once per frame
    void Update()
    {

    }
}

public class ProgressEvent
{
    //public bool HasOccurred { get; set; }
    public float TimeTillExcecution { get; set; }
    public Func<bool> ProgressionCondition { get; set; }
    public Action ProgressionAction { get; set; }

    public ProgressEvent(Func<bool> _progressionCondition, Action _progressionAction, float _timeTillExcecution){
        ProgressionAction = _progressionAction;
        ProgressionCondition = _progressionCondition;
        TimeTillExcecution = _timeTillExcecution;
        //HasOccurred = false;
    }
}
