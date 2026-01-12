using UnityEngine;
using System.Collections.Generic;
using System.Collections;

// "Trees and grass remove the carbon houses make" Tutorial tip
public class TreesRemoveCarbonTutorialTip : TutorialTip
{
    //Constructor passes values to base class
    public TreesRemoveCarbonTutorialTip(int _tutorialTipTextID, TutorialTipManager _TTM, float _timeToWaitBeforeActivating, float _timeToWaitBeforeDeactivating) : base(_tutorialTipTextID, _TTM, _timeToWaitBeforeActivating, _timeToWaitBeforeDeactivating){
        //Checks if "trees remove carbon" should be activated every time a progress event is called
        GameEventManager.current.ProgressEventJustCalled.AddListener(CheckIfTipShouldBeActivated);

        //Checks if "trees remove carbon" should be deactivated every time a tile is placed
        GameEventManager.current.TileJustPlaced.AddListener(CheckIfTipShouldBeDeactivated);
    }

    public void CheckIfTipShouldBeActivated(){

        //Checks that the progress event was trees and grass being unlocked
        if(GameEventManager.current.TypeOfLastProgressEventCalled == ProgressionManager.ProgressEventType.TreesAndGrassUnlocked){
            //Activates tutorial tip if no net-negative carbon tiles are placed
            if(!NetNegativeCarbonTilePlaced()){
                ActivateTutorialTip();
            }
        }
    }

    public void CheckIfTipShouldBeDeactivated(){
        if(tutorialTipIsActivated && NetNegativeCarbonTilePlaced()){
            DeactivateTutorialTip();
        }
    }

    //Checks if any net negative carbon tiles are placed
    private bool NetNegativeCarbonTilePlaced(){
        bool netNegativeCarbonTileIsPlaced = false;
        foreach(Tile tile in TileTypeCounter.current.CarbonTileTracker.GetAllTiles()){
            if(tile.tileScriptableObject != null){
                if(tile.tileScriptableObject.AnnualCarbonAdded < 0){
                    netNegativeCarbonTileIsPlaced = true;
                }
            }
        }
        return netNegativeCarbonTileIsPlaced;
    }

}



