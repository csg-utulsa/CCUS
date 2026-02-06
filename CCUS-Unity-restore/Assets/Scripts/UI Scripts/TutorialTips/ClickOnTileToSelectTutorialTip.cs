using UnityEngine;
using System.Collections.Generic;
using System.Collections;

//"Click on a tile to select it" tutorial tip
public class ClickOnTileToSelectTutorialTip : TutorialTip
{
    private bool atLeastOneTileHasBeenPlaced = false;

    //Constructor passes values to base class
    public ClickOnTileToSelectTutorialTip(int _tutorialTipTextID, TutorialTipManager _TTM, float _timeToWaitBeforeActivating, float _timeToWaitBeforeDeactivating) : base(_tutorialTipTextID, _TTM, _timeToWaitBeforeActivating, _timeToWaitBeforeDeactivating){
        
    }

    public override void InitializeThisTutorialTip(){
        //Immediately starts countdown to show tutorial tip
        //Checks if it should be deactivated every time a tile is placed & every time a button is selected
        ActionDelayer.DelayAction(CheckIfTipShouldBeActivated, 1f); 
        GameEventManager.current.ButtonHasBeenSelected.AddListener(CheckIfTipShouldBeDeactivated);
        GameEventManager.current.TileJustPlaced.AddListener(CheckIfTipShouldBeDeactivated);
    }

    public void CheckIfTipShouldBeActivated(){
        if(!ButtonIsCurrentlySelected()){
            ActivateTutorialTip();
        }
    }

    //Checks if the Tip should be deactivated. Runs every time a tile is placed
    public void CheckIfTipShouldBeDeactivated(){
        if((tutorialTipIsActivated && ButtonSelectedAtLeastOnce()) || ButtonIsCurrentlySelected()){
            DeactivateTutorialTip();
        }
    }

    // private bool AtLeastOneTilePlaced(){
    //     if(atLeastOneTileHasBeenPlaced){
    //         return true;
    //     }
    //     if(GridManager.GM.AtLeastOneTileIsOnChunk()){
    //         atLeastOneTileHasBeenPlaced = true;
    //         return true;
    //     }
    //     return false;
    // }
    
    private bool ButtonSelectedAtLeastOnce(){
        return TileSelectPanel.TSP.AButtonHasBeenSelectedAtLeastOnce;
    }

    private bool ButtonIsCurrentlySelected(){
        return TileSelectPanel.TSP.ButtonIsCurrentlySelected;
    }
}
