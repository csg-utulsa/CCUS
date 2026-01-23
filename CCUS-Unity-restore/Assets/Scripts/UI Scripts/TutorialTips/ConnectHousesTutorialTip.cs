using UnityEngine;
using System.Collections.Generic;
using System.Collections;

//"Houses Connected by roads make money" tutorial tip
public class ConnectHousesTutorialTip : TutorialTip
{
    //Constructor passes values to base class
    public ConnectHousesTutorialTip(int _tutorialTipTextID, TutorialTipManager _TTM, float _timeToWaitBeforeActivating, float _timeToWaitBeforeDeactivating) : base(_tutorialTipTextID, _TTM, _timeToWaitBeforeActivating, _timeToWaitBeforeDeactivating){
        
    }

    public override void InitializeThisTutorialTip(){
        //Checks if houses are connected every time a tile is placed
        GameEventManager.current.TileJustPlaced.AddListener(CheckIfTipShouldBeDeactivated);
        ActionDelayer.DelayAction(CheckIfTipShouldBeActivated, 1f); 
    }
    
    


    public void CheckIfTipShouldBeActivated(){
        if(!HousesAreConnected()){
            ActivateTutorialTip();
        }
    }

    //Checks if the Tip should be deactivated. Runs every time a tile is placed
    public void CheckIfTipShouldBeDeactivated(){
        if(HousesAreConnected()){
            DeactivateTutorialTip();
        }
    }

    //Checks if any houses are connected
    private bool HousesAreConnected(){
        //Checks if any of the houses are connected by roads
        bool houseIsConnected = false;
        foreach(Tile tile in TileTypeCounter.current.ResidenceTileTracker.GetAllTiles()){
            if(tile is ActivatableTile activatableTile){
                if(activatableTile.IsActivated){
                    houseIsConnected = true;
                }
            }
        }
        return houseIsConnected;
    }

    //Checks if the unlock houses progress event has occurred
    // private bool HousesAreUnlocked(){
    //     if(ProgressionManager.PM.progressEventHasOccurred[(int)ProgressionManager.ProgressEventType.HouseAndRoadUnlocked]){
    //         return true;
    //     }else{
    //         return true;
    //     }
    // }

}




