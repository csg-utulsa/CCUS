using UnityEngine;

public class CarbonCaptureTile : Tile
{
    public override bool CheckIfTileIsPlaceable(bool displayErrorMessages){
        if(!base.CheckIfTileIsPlaceable(displayErrorMessages)){
            return false;
        }
        //Checks if its under the max limit of carbon capture tiles
        if (!UnderMaxCarbonCaptureTiles())
        {
            if(displayErrorMessages){
                unableToPlaceTileUI._unableToPlaceTileUI.MaxCarbonCaptureTilesError();
            }
            return false;
        }

        return true;
    }
    public override void ThisTileJustPlaced(){
        
        MaxTileTypeCounter.current.UpdateNumberOfCarbonCaptureSystems();
        base.ThisTileJustPlaced();
    }

    public override void ThisTileAboutToBeDestroyed(){
        base.ThisTileAboutToBeDestroyed();
        MaxTileTypeCounter.current.UpdateNumberOfCarbonCaptureSystems();
    }

    public bool UnderMaxCarbonCaptureTiles(){

        return MaxTileTypeCounter.current.UnderMaxCarbonCaptureTiles();
    }

}
