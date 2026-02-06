using UnityEngine;
using System.Collections.Generic;
using System.Collections;

// "You can buy more area to the right" tutorial tip
public class NewAreaUnlockedTutorialTip : TutorialTip
{
    private bool chunkOneHasBeenLookedAt = false;
    //Constructor passes values to base class
    public NewAreaUnlockedTutorialTip(int _tutorialTipTextID, TutorialTipManager _TTM, float _timeToWaitBeforeActivating, float _timeToWaitBeforeDeactivating) : base(_tutorialTipTextID, _TTM, _timeToWaitBeforeActivating, _timeToWaitBeforeDeactivating){
        
    }

    public override void InitializeThisTutorialTip(){

        //Checks if tip should be activated when a new area is unlocked
        GameEventManager.current.NewAreaUnlocked.AddListener(CheckIfTipShouldBeActivated);

        //Checks if tip should be deactivated every time the current ground chunk is switched
        GameEventManager.current.SwitchedCurrentGroundChunk.AddListener(CheckIfTipShouldBeDeactivated);
    }

    public void CheckIfTipShouldBeActivated(){
        //Checks if the player has unlocked the ability to buy new area.
        if(ChunkOfIndexIsViewable(1) && !ChunkOneHasBeenLookedAt()){
            ActivateTutorialTip();
        }
    }

    //Deactivates tip if the second chunk is being looked at
    public void CheckIfTipShouldBeDeactivated(){
        if(ChunkOneHasBeenLookedAt()){
            DeactivateTutorialTip();
        }
    }

    //Checks if the chunk of the given index is viewable
    private bool ChunkOfIndexIsViewable(int indexNum){
        GroundAreaExpansion chunkManager = GroundAreaExpansion.GAE;
        if(indexNum <= chunkManager.NumberOfGroundChunks - 1){
            return true;
        }
        return false;
    }

    //Checks if the chunk at a given index has been looked at
    private bool ChunkOneHasBeenLookedAt(){
        if(chunkOneHasBeenLookedAt) return true;
        if(GroundAreaExpansion.GAE.ActiveGroundChunk == 1){
            chunkOneHasBeenLookedAt = true;
            return true;
        }
        return false;
    }

}
