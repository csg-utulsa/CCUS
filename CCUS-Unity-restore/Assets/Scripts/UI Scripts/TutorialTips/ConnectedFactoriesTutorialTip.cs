using UnityEngine;
using System.Collections.Generic;
using System.Collections;

// "Only factories connect by roads make money" Tutorial tip
public class ConnectedFactoriesTutorialTip : TutorialTip
{
    //Constructor passes values to base class
    public ConnectedFactoriesTutorialTip(int _tutorialTipTextID, TutorialTipManager _TTM, float _timeToWaitBeforeActivating, float _timeToWaitBeforeDeactivating) : base(_tutorialTipTextID, _TTM, _timeToWaitBeforeActivating, _timeToWaitBeforeDeactivating){
        //Checks if tip should be activated every time a progress event is called
        GameEventManager.current.ProgressEventJustCalled.AddListener(CheckIfTipShouldBeActivated);

        //Checks if tip should be deactivated every time a tile is placed
        GameEventManager.current.TileJustPlaced.AddListener(CheckIfTipShouldBeDeactivated);
    }

    public void CheckIfTipShouldBeActivated(){
        //Checks that the progress event was factories being unlocked
        if(GameEventManager.current.TypeOfLastProgressEventCalled == ProgressionManager.ProgressEventType.FactoriesUnlocked){
            //Activates tutorial tip if no factories are connected
            if(!FactoriesAreConnected()){
                ActivateTutorialTip();
            }
        }
    }

    //Deactivates tip if any factories are connected
    public void CheckIfTipShouldBeDeactivated(){
        if(tutorialTipIsActivated && FactoriesAreConnected()){
            DeactivateTutorialTip();
        }
    }

    //Checks if any factories are connected
    private bool FactoriesAreConnected(){
        //Checks if any of the factories are connected by roads
        bool factoryIsConnected = false;
        foreach(Tile tile in TileTypeCounter.current.FactoryTileTracker.GetAllTiles()){
            if(tile is ActivatableTile activatableTile){
                if(activatableTile.IsActivated){
                    factoryIsConnected = true;
                }
            }
        }
        return factoryIsConnected;
    }

}
