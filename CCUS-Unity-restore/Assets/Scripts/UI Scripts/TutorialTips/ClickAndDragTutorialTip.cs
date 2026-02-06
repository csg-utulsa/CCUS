using UnityEngine;
using System.Collections.Generic;
using System.Collections;

//"You can use click and drag to place tile" tutorial tip
public class ClickAndDragTutorialTip : TutorialTip
{
    public int numberOfTilesToPlaceBeforeActivating = 30;
    private int numberOfTilesPlaced = 0;
    private bool enoughTilesPlaced = false;

    //Constructor passes values to base class
    public ClickAndDragTutorialTip(int _tutorialTipTextID, TutorialTipManager _TTM, float _timeToWaitBeforeActivating, float _timeToWaitBeforeDeactivating) : base(_tutorialTipTextID, _TTM, _timeToWaitBeforeActivating, _timeToWaitBeforeDeactivating){
        
    }

    public override void InitializeThisTutorialTip(){
        
        //Checks if tutorial tip should be deactivated every time a tile is placed
        GameEventManager.current.TileJustPlaced.AddListener(CheckIfTipShouldBeDeactivated);
        GameEventManager.current.TileJustPlaced.AddListener(CheckIfTipShouldBeActivated);
    }

    public void CheckIfTipShouldBeActivated(){
        if(!ClickAndDragHasBeenUsed() && CheckIfEnoughTilesHaveBeenPlaced()){
            ActivateTutorialTip();
        }
    }

    //Checks if the Tip should be deactivated. Runs every time a tile is placed
    public void CheckIfTipShouldBeDeactivated(){
        if(ClickAndDragHasBeenUsed()){
            DeactivateTutorialTip();
        }
    }

    //Checks if the click and drag feature has been used yet
    private bool ClickAndDragHasBeenUsed(){
        return BuildingSystem.current.ClickAndDragHasBeenUsed;
    }

    //Checks if enough tiles have been placed to activate thte tutorial tip.
    private bool CheckIfEnoughTilesHaveBeenPlaced(){
        if(enoughTilesPlaced) return true;

        numberOfTilesPlaced++;
        if(numberOfTilesPlaced > numberOfTilesToPlaceBeforeActivating){
            Debug.Log("Number of tiles placed: " + numberOfTilesToPlaceBeforeActivating);
            enoughTilesPlaced = true;
            return true;
        }
        return false;
    }

    
}
