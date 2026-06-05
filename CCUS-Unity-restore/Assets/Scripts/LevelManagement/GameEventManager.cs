/*
*   This class keeps track of most of the in game events (besides ticks)
*
*   How to add a new event: See instructions in EventType.cs file
*
*/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameEventManager : MonoBehaviour
{

    public List<GameEvent> gameEvents = new List<GameEvent>();

    
    public static GameEventManager current;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {

        if(current == null){
            current = this;
        }else{
            Destroy(this);
        }

        //Creates an event for each EventType
        foreach(EventType.E myEvent in EventType.E.GetValues(typeof(EventType.E))){
            gameEvents.Add(new GameEvent(myEvent));
        }

    }

    //Returns an event for a given eventType
    public UnityEvent GetEvent(EventType.E eventType){
        return gameEvents.Find(x => x.myEventType == eventType).myEvent;

    }


}

public class GameEvent{

    public UnityEvent myEvent;
    //public SpecialEventData mySpecialEventData;
    public EventType.E myEventType;

    public GameEvent(EventType.E _myEventType){
        myEventType = _myEventType;
        myEvent = new UnityEvent();
    }

    
}
