using UnityEngine;

public class PeopleHousedToolTip : ToolTipType
{
    //Enables tool tip for buildings that house people and only after people panel has been activated
    public override bool ShouldEnableToolTip(Tile tile){
        if(tile.tileScriptableObject != null && tile.tileScriptableObject.MaxPeople > 0 && PeoplePanelIsEnabled()){
            return true;
        }else{
            return false;
        }
    }

    private bool PeoplePanelIsEnabled(){
        if(ProgressionManager.PM.progressEventHasOccurred[(int)ProgressionManager.ProgressEventType.PeopleUnlocked]){
            return true;
        } else{
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
