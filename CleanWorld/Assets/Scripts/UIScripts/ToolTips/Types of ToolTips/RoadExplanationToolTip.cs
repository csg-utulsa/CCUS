using UnityEngine;

public class RoadExplanationToolTip : ToolTipType
{
    //Road explanation tip only enabled for road tiles
    public override bool ShouldEnableToolTip(Tile tile){
        if(tile is RoadTile roadTile){
            return true;
        }else{
            return false;
        }
    }

}
