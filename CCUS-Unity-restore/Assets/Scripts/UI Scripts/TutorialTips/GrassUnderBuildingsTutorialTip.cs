using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GrassUnderBuildingsTutorialTip : TutorialTip
{
    //Stores whether terrain has ever been placed under a building
    private bool terrainHasBeenUnderBuilding = false;

    //Constructor passes values to base class
    public GrassUnderBuildingsTutorialTip(int _tutorialTipTextID, TutorialTipManager _TTM, float _timeToWaitBeforeActivating, float _timeToWaitBeforeDeactivating) : base(_tutorialTipTextID, _TTM, _timeToWaitBeforeActivating, _timeToWaitBeforeDeactivating){
        
    }

    public override void InitializeThisTutorialTip(){
        //Checks if any grass is under a building every time a tile is placed
        GameEventManager.current.TileJustPlaced.AddListener(CheckIfTipShouldBeDeactivated);
        GameEventManager.current.TileJustPlaced.AddListener(CheckIfTipShouldBeActivated); 
    }
    
    //Checks if tip should be activated; runs at every tile placement
    public void CheckIfTipShouldBeActivated(){
        if(GrassTilesUnlocked() && !TerrainIsUnderBuilding()){
            ActivateTutorialTip();
        }
    }

    //Checks if the Tip should be deactivated. Runs every time a tile is placed
    public void CheckIfTipShouldBeDeactivated(){
        if(TerrainIsUnderBuilding()){
            DeactivateTutorialTip();
        }
    }


    //Checks if the grid cell under the mouse has a terrain and placeable object
    private bool TerrainIsUnderBuilding(){
        if(terrainHasBeenUnderBuilding){
            return true;
        }


        Vector3 mousePos = BuildingSystem.GetMouseWorldPosition();
        GameObject[] gameObjectsOnCell = GridManager.GM.GetGameObjectsInGridCell(mousePos);
        foreach(GameObject tileInCell in gameObjectsOnCell){
            if(gameObjectsOnCell.Length >= 2){
                if(tileInCell.GetComponent<Tile>() != null){
                    if(tileInCell.GetComponent<Tile>().tileScriptableObject.AnnualCarbonAdded >= 0)
                    {
                        terrainHasBeenUnderBuilding = true;
                        return true;
                    }
                }
            }
            
        }

        return false;
    }

    //Detects whether or not grass tiles have been unlocked
    private bool GrassTilesUnlocked(){
        if(ProgressionManager.PM.progressEventHasOccurred[(int)ProgressionManager.ProgressEventType.TreesAndGrassUnlocked]){
            return true;
        } else{
            return false;
        }

    }


}
