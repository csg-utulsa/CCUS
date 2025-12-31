using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionManager : MonoBehaviour
{
    public static ProgressionManager PM;

    //Stores all the buttons, to be added or removed. Assign in inspector.
    public buttonScript[] buttons;

    public bool[] progressEventHasOccurred;

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


    */
    public ProgressEvent[] progressEvents => new ProgressEvent[]{

        //Event 0: Enables the people button when you fix the maxed out carbon the first time
        new ProgressEvent(() => progressEventHasOccurred[1] && !LevelManager.overMaxCarbon(), () => {PeoplePanel._peoplePanel.EnablePeoplePanel();}, 6f),
        
        //Event 1: Adds the tree and grass buttons the first time you max out on carbon.
        new ProgressEvent(() => LevelManager.overMaxCarbon(), () => {TileSelectPanel.TSP.AddButton(buttons[0]); TileSelectPanel.TSP.AddButton(buttons[1]);}, 4.5f),

        //Event 2: Adds apartment
        new ProgressEvent(() => LevelManager.LM.NetMoney > 80, () => {TileSelectPanel.TSP.AddButton(buttons[3]);}, 1.5f),

        //Event 2: Adds Roads when you fix the maxed out carbon the first time
        //new ProgressEvent(() => progressEventHasOccurred[1] && !LevelManager.overMaxCarbon(), () => {TileSelectPanel.TSP.AddButton(buttons[2]);}, 3f),
    
        
    };

    
    
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
            }
        }
        
    }

    private void CheckProgressEventConditions(){
        
        for(int i = 0; i < progressEvents.Length; i++){
            if(!progressEventHasOccurred[i] && progressEvents[i].ProgressionCondition()){
                float delayTime = progressEvents[i].TimeTillExcecution;
                if(delayTime > 0){
                    StartCoroutine(delayProgressionEvent(progressEvents[i].TimeTillExcecution, progressEvents[i].ProgressionAction));
                    
                }else{
                    progressEvents[i].ProgressionAction();
                }
                progressEventHasOccurred[i] = true;
            }
        }
    }

    private IEnumerator delayProgressionEvent(float delayTime, Action progressAction){
        yield return new WaitForSeconds(delayTime);
        progressAction();
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
