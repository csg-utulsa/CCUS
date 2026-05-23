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

    // //Progress events
    // public UnityEvent ProgressEventJustCalled {get; set;}
    // public UnityEvent NewTileUnlocked {get; set;}
    
    // //People Events
    // public UnityEvent PersonJustAdded {get; set;}
    // public UnityEvent NumberOfPeopleChanged {get; set;}

    // //UI Interaction Events
    // public UnityEvent MouseMovedToNewGridTile {get; set;}
    // public UnityEvent ButtonHasBeenSelected{get; set;}

    // //Money / Carbon Changed
    // public UnityEvent NetCarbonUpdated {get; set;}
    // public UnityEvent NetMoneyUpdated {get; set;}
    // public UnityEvent MoneyAmountUpdated {get; set;}

    // //Chunk Loading events
    // public UnityEvent SwitchedCurrentGroundChunk {get; set;}
    // public UnityEvent SwitchedCurrentGroundChunkLate {get; set;}
    // public UnityEvent PurchasedCurrentGroundChunk {get; set;}
    // public UnityEvent BeginSwitchingCurrentGroundChunk {get; set;}
    // public UnityEvent BeginSwitchingCurrentGroundChunkLate {get; set;}
    // public UnityEvent NewAreaUnlocked {get; set;}

    // //Tile destruction and activation
    // public UnityEvent TileJustPlaced {get; set;}
    // public UnityEvent ActivatableTileJustPlaced {get; set;}
    // public UnityEvent ActivatableTileJustDestroyed {get; set;}
    // public UnityEvent TileJustDestroyed {get; set;}
    // public UnityEvent BuildingActivationStateChanged {get; set;}
    // public UnityEvent NumOfCarbonCaptureTilesChanged {get; set;}
    // public UnityEvent NumOfWorkPlaceTilesChanged {get; set;}
    // public UnityEvent FailedToPlaceTile {get; set;}

    // //UI Events. NOTE: Move to separate class eventually
    // public UnityEvent TileSelectPanelScrolled {get; set;}
    // public UnityEvent TutorialTipHasAppeared {get; set;}
    // public UnityEvent CloseTutorialNotification {get; set;}

    public static GameEventManager current;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        // TileJustPlaced = new UnityEvent();
        // ProgressEventJustCalled = new UnityEvent();
        // PersonJustAdded = new UnityEvent();
        // NetCarbonUpdated = new UnityEvent();
        // NetMoneyUpdated = new UnityEvent();
        // MoneyAmountUpdated = new UnityEvent();
        // SwitchedCurrentGroundChunk = new UnityEvent();
        // PurchasedCurrentGroundChunk = new UnityEvent();
        // BeginSwitchingCurrentGroundChunk = new UnityEvent();
        // ActivatableTileJustPlaced = new UnityEvent();
        // ActivatableTileJustDestroyed = new UnityEvent();
        // TileJustDestroyed = new UnityEvent();
        // BuildingActivationStateChanged = new UnityEvent();
        // NumOfCarbonCaptureTilesChanged = new UnityEvent();
        // MouseMovedToNewGridTile = new UnityEvent();
        // ButtonHasBeenSelected = new UnityEvent();
        // NewAreaUnlocked = new UnityEvent();
        // NumberOfPeopleChanged = new UnityEvent();
        // NumOfWorkPlaceTilesChanged = new UnityEvent();
        // NewTileUnlocked = new UnityEvent();
        // SwitchedCurrentGroundChunkLate = new UnityEvent();
        // TileSelectPanelScrolled = new UnityEvent();
        // BeginSwitchingCurrentGroundChunkLate = new UnityEvent();
        // FailedToPlaceTile = new UnityEvent();
        // TutorialTipHasAppeared = new UnityEvent();
        // CloseTutorialNotification = new UnityEvent();

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
        // GameEvent matchingGameEvent = gameEvents.Find(x => x.myEventType.Equals(eventType));
        // if(matchingGameEvent == null){
        //     Debug.Log("Can't find matching game event of type " + eventType);

        //     foreach(GameEvent myEvent in gameEvents){
        //         bool isTheSame = (eventType == myEvent.myEventType);
        //         Debug.Log("Game event: *" + myEvent.myEventType + "* is not *" + eventType.Replace("\r", "") + "*. " + isTheSame);
        //     }
        // }
        // return matchingGameEvent.myEvent;
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
