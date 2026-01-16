using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActionDelayer : MonoBehaviour
{
    public static ActionDelayer current;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(current == null){
            current = this;
        }else{
            Destroy(this);
        }
    }

    public static void DelayAction(Action delayedAction, float secondsToWait){
        current.StartCoroutine(current.DelayActionCoroutine(delayedAction, secondsToWait));
    }

    IEnumerator DelayActionCoroutine(Action delayedAction, float secondsToWait){
        yield return new WaitForSeconds(secondsToWait);
        delayedAction();
    }
}
