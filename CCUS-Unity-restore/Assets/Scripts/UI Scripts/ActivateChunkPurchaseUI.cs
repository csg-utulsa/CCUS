using UnityEngine;

public class ActivateChunkPurchaseUI : MonoBehaviour
{
    public static ActivateChunkPurchaseUI current;

    public GameObject purchaseUIObject;
    
    void Start(){
        if(current == null){
            current = this;
        }else{
            Destroy(this);
        }
        GameEventManager.current.SwitchedCurrentGroundChunk.AddListener(SwitchedGroundChunk);
        GameEventManager.current.BeginSwitchingCurrentGroundChunk.AddListener(SwitchedGroundChunk);
        GameEventManager.current.PurchasedCurrentGroundChunk.AddListener(GroundChunkJustPurchased);
    }

    private void SwitchedGroundChunk(){
        if(!ChunkPurchaseManager.current.ActiveChunkIsPurchased){
            Activate();
        }else{
            Deactivate();
        }
    }

    private void GroundChunkJustPurchased(){
        Deactivate();
    }
    
    private void Activate(){
        purchaseUIObject.SetActive(true);
    }

    private void Deactivate(){
        purchaseUIObject.SetActive(false);
    }
    
    



}
