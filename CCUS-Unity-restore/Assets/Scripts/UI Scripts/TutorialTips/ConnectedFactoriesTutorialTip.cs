using UnityEngine;
using System.Collections.Generic;
using System.Collections;

// "Only factories connect by roads make money" Tutorial tip
public class ConnectedFactoriesTutorialTip : TutorialTip
{
    //Constructor passes values to base class
    public ConnectedFactoriesTutorialTip(int _tutorialTipTextID, TutorialTipManager _TTM, float _timeToWaitBeforeActivating, float _timeToWaitBeforeDeactivating) : base(_tutorialTipTextID, _TTM, _timeToWaitBeforeActivating, _timeToWaitBeforeDeactivating){
        
        
    }

    public override void InitializeThisTutorialTip(){
        //Checks if tip should be activated every time a tile is placed
        GameEventManager.current.TileJustPlaced.AddListener(CheckIfTipShouldBeActivated);

        //Checks if tip should be deactivated every time a tile is placed
        GameEventManager.current.TileJustPlaced.AddListener(CheckIfTipShouldBeDeactivated);
    }

    public void CheckIfTipShouldBeActivated(){
        //Checks that factories have been unlocked
        if(ProgressionManager.PM.progressEventHasOccurred[(int)ProgressionManager.ProgressEventType.FactoriesUnlocked]){
            //Activates tutorial tip if no factories are connected
            if(FactoriesArePlaced() && !FactoriesAreConnected()){
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

    //Checks if any factories are placed
    private bool FactoriesArePlaced(){
        //Checks if any factories are placed
        bool factoryIsPlaced = false;
        foreach(Tile tile in TileTypeCounter.current.FactoryTileTracker.GetAllTiles()){
            if(tile is FactoryTile factoryTile){
                return true;
            }
        }
        return false;
    }

}
