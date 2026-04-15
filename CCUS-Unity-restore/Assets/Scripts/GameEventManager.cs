/*
*   This class keeps track of most of the in game events (besides ticks)
*
*   How to add a new event: 
*       1) Declare it like this: "public UnityEvent SomeGameEvent {get; set;}"
*       2) Initiate it in Awake(), like this: "SomeGameEvent = new UnityEvent();"
*       3) Call it from another script using GameEventManager.current.SomeGameEvent.Invoke();
*/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameEventManager : MonoBehaviour
{

    //Progress events
    public UnityEvent ProgressEventJustCalled {get; set;}
    public UnityEvent NewTileUnlocked {get; set;}
    
    //People Events
    public UnityEvent PersonJustAdded {get; set;}
    public UnityEvent NumberOfPeopleChanged {get; set;}

    //UI Interaction Events
    public UnityEvent MouseMovedToNewGridTile {get; set;}
    public UnityEvent ButtonHasBeenSelected{get; set;}

    //Money / Carbon Changed
    public UnityEvent NetCarbonUpdated {get; set;}
    public UnityEvent NetMoneyUpdated {get; set;}
    public UnityEvent MoneyAmountUpdated {get; set;}

    //Chunk Loading events
    public UnityEvent SwitchedCurrentGroundChunk {get; set;}
    public UnityEvent SwitchedCurrentGroundChunkLate {get; set;}
    public UnityEvent PurchasedCurrentGroundChunk {get; set;}
    public UnityEvent BeginSwitchingCurrentGroundChunk {get; set;}
    public UnityEvent BeginSwitchingCurrentGroundChunkLate {get; set;}
    public UnityEvent NewAreaUnlocked {get; set;}

    //Tile destruction and activation
    public UnityEvent TileJustPlaced {get; set;}
    public UnityEvent ActivatableTileJustPlaced {get; set;}
    public UnityEvent ActivatableTileJustDestroyed {get; set;}
    public UnityEvent TileJustDestroyed {get; set;}
    public UnityEvent BuildingActivationStateChanged {get; set;}
    public UnityEvent NumOfCarbonCaptureTilesChanged {get; set;}
    public UnityEvent NumOfWorkPlaceTilesChanged {get; set;}
    public UnityEvent FailedToPlaceTile {get; set;}

    //UI Events. NOTE: Move to separate class eventually
    public UnityEvent TileSelectPanelScrolled {get; set;}
    public UnityEvent TutorialTipHasAppeared {get; set;}
    public UnityEvent CloseTutorialNotification {get; set;}

    public static GameEventManager current;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        TileJustPlaced = new UnityEvent();
        ProgressEventJustCalled = new UnityEvent();
        PersonJustAdded = new UnityEvent();
        NetCarbonUpdated = new UnityEvent();
        NetMoneyUpdated = new UnityEvent();
        MoneyAmountUpdated = new UnityEvent();
        SwitchedCurrentGroundChunk = new UnityEvent();
        PurchasedCurrentGroundChunk = new UnityEvent();
        BeginSwitchingCurrentGroundChunk = new UnityEvent();
        ActivatableTileJustPlaced = new UnityEvent();
        ActivatableTileJustDestroyed = new UnityEvent();
        TileJustDestroyed = new UnityEvent();
        BuildingActivationStateChanged = new UnityEvent();
        NumOfCarbonCaptureTilesChanged = new UnityEvent();
        MouseMovedToNewGridTile = new UnityEvent();
        ButtonHasBeenSelected = new UnityEvent();
        NewAreaUnlocked = new UnityEvent();
        NumberOfPeopleChanged = new UnityEvent();
        NumOfWorkPlaceTilesChanged = new UnityEvent();
        NewTileUnlocked = new UnityEvent();
        SwitchedCurrentGroundChunkLate = new UnityEvent();
        TileSelectPanelScrolled = new UnityEvent();
        BeginSwitchingCurrentGroundChunkLate = new UnityEvent();
        FailedToPlaceTile = new UnityEvent();
        TutorialTipHasAppeared = new UnityEvent();
        CloseTutorialNotification = new UnityEvent();

        if(current == null){
            current = this;
        }else{
            Destroy(this);
        }
    }


}
