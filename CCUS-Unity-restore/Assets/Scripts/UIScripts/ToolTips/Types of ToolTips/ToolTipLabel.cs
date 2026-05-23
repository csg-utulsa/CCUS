using UnityEngine;
using System;

public class ToolTipLabel : ToolTipType
{
    //Always enables label
    public override bool ShouldEnableToolTip(Tile tile){
        return true;
    }

    //Sets Text to tile name
    public override void EnableToolTip(Tile tile){
        base.EnableToolTip(tile);
        if(tile.tileScriptableObject != null){
            SetTipText("" + tile.tileScriptableObject.Name.ToUpper());
        }
    }
}
