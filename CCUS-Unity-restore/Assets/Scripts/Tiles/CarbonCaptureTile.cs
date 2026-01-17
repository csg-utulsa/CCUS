using UnityEngine;

public class CarbonCaptureTile : Tile
{
    public int MaxCarbonCaptureTiles{
        get{
            return MaxTileTypeCounter.current.MaxCarbonCaptureSystems;
        }
    }
    public override bool CheckIfTileIsPlaceable(bool displayErrorMessages){
        if(!base.CheckIfTileIsPlaceable(displayErrorMessages)){
            return false;
        }
        //Checks if it's under the max limit of carbon capture tiles
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
        
        //MaxTileTypeCounter.current.UpdateNumberOfCarbonCaptureSystems();
        base.ThisTileJustPlaced();
    }

    public override void ThisTileAboutToBeDestroyed(){
        base.ThisTileAboutToBeDestroyed();
        //MaxTileTypeCounter.current.UpdateNumberOfCarbonCaptureSystems();
    }

    public bool UnderMaxCarbonCaptureTiles(){

        MaxTileTypeCounter myMax = MaxTileTypeCounter.current;
        //Debug.Log(myMax);

        return MaxTileTypeCounter.current.UnderMaxCarbonCaptureTiles();
    }

}
