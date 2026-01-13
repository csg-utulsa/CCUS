using UnityEngine;

public class PeopleHousedToolTip : ToolTipType
{
    public override bool ShouldEnableToolTip(Tile tile){
        if(tile.tileScriptableObject != null && tile.tileScriptableObject.MaxPeople > 0){
            return true;
        }else{
            return false;
        }
    }

    public override void EnableToolTip(Tile tile){
        base.EnableToolTip(tile);
        if(tile.tileScriptableObject != null){
            SetTipText("" + tile.tileScriptableObject.MaxPeople);
        }
    }
}
