using UnityEngine;

public class RoadConnectionsRequiredToolTip : ToolTipType
{
    //Displays "MUST BE CONNECTED BY ROADS" for every activatable tile other than roads
    public override bool ShouldEnableToolTip(Tile tile){
        if(tile is ActivatableTile activatableTile){
            if(activatableTile is RoadTile roadTile){
                return false;
            }
            return true;
        }else{
            return false;
        }
    }
}
