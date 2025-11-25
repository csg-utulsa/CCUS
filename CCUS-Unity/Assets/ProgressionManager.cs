using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionManager : MonoBehaviour
{
    //Stores all the buttons, to be added or removed. Assign in inspector.
    public buttonScript[] buttons;

    //Defines all the progress events
    public ProgressEvent[] progressEvents => new ProgressEvent[]{
        //Adds the tree and grass buttons the first time you max out on carbon.
        new ProgressEvent(() => LevelManager.LM.getMaxCarbon() <= LevelManager.LM.GetCarbon(), () => {TileSelectPanel.TSP.AddButton(buttons[0]); TileSelectPanel.TSP.AddButton(buttons[1]);}, 5f),
    
    
    };

    public bool[] progressEventHasOccurred;
    
    void Start(){
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
