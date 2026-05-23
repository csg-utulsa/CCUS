using UnityEngine;

public class RoadConnectionsRequiredToolTip : ToolTipType
{
    //Displays "MUST BE CONNECTED BY ROADS" for every activatable tile other than roads
    public override bool ShouldEnableToolTip(Tile tile){

        //If tiles must be connected by roads, displays "Must Be Connected By Roads"
        if(tile.tileScriptableObject != null && tile.tileScriptableObject.MustBeConnectedByRoads){

            //Returns false for road tiles
            if(tile is ActivatableTile activatableTile){
                if(activatableTile is RoadTile roadTile){
                    return false;
                }
            }
            
            return true;
        }else{
            return false;
        }
    }
}
