using UnityEngine;

public class MaxCarbonCaptureToolTip : ToolTipType
{
    public override bool ShouldEnableToolTip(Tile tile){
        if(tile is CarbonCaptureTile carbonCaptureTile){
            return true;
        }else{
            return false;
        }
    }

    public override void EnableToolTip(Tile tile){
        base.EnableToolTip(tile);
        SetTipText("" + MaxTileTypeCounter.current.MaxCarbonCaptureSystems);
    }
}
