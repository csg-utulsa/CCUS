using UnityEngine;

//Alerts the player if there is more area available, after they have bought the first area, if they haven't looked at it yet
public class MoreAreaAvailableTutorialTip : TutorialTip
{
    private bool chunkTwoHasBeenLookedAt = false;

    //Constructor passes values to base class
    public MoreAreaAvailableTutorialTip(int _tutorialTipTextID, TutorialTipManager _TTM, float _timeToWaitBeforeActivating, float _timeToWaitBeforeDeactivating) : base(_tutorialTipTextID, _TTM, _timeToWaitBeforeActivating, _timeToWaitBeforeDeactivating){
        
        
    }

    public override void InitializeThisTutorialTip(){
        //Checks if tip should be activated every time a new tile is purchased
        GameEventManager.current.PurchasedCurrentGroundChunk.AddListener(CheckIfTipShouldBeActivated);

        //Checks if tip should be deactivated every time the current ground chunk is switched
        GameEventManager.current.SwitchedCurrentGroundChunk.AddListener(CheckIfTipShouldBeDeactivated);
    }

    public void CheckIfTipShouldBeActivated(){
        //Checks if the second unlockable chunk has been unlocked
        if(ChunkOfIndexIsViewable(2)){
            //Activates tutorial tip if no factories are connected
            ActivateTutorialTip();
        }
    }

    //Deactivates tip if the second chunk is being looked at
    public void CheckIfTipShouldBeDeactivated(){
        if(ChunkOfIndexHasBeenLookedAt(2)){
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
    private bool ChunkOfIndexHasBeenLookedAt(int indexNum){
        if(chunkTwoHasBeenLookedAt) return true;
        if(GroundAreaExpansion.GAE.ActiveGroundChunk == indexNum){
            chunkTwoHasBeenLookedAt = true;
            return true;
        }
        return false;
    }



}
