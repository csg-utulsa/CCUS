using UnityEngine;

public class MoneyToolTip : ToolTipType
{
    public override bool ShouldEnableToolTip(Tile tile){
        if(tile.tileScriptableObject != null && tile.tileScriptableObject.AnnualIncome > 0){
            return true;
        }else{
            return false;
        }
    }

    public override void EnableToolTip(Tile tile){
        base.EnableToolTip(tile);
        if(tile.tileScriptableObject != null){
            SetTipText("" + tile.tileScriptableObject.AnnualIncome);
        }
    }
}
