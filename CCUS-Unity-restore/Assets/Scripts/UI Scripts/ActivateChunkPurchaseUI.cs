using UnityEngine;
using System;
using TMPro;

public class ActivateChunkPurchaseUI : MonoBehaviour
{
    public static ActivateChunkPurchaseUI current;

    public GameObject purchaseUIObject;

    public TextMeshProUGUI purchasePriceText;
    
    void Start(){
        if(current == null){
            current = this;
        }else{
            Destroy(this);
        }
        purchasePriceText = GetComponentInChildren<TextMeshProUGUI>(true);
        GameEventManager.current.SwitchedCurrentGroundChunk.AddListener(SwitchedGroundChunk);
        GameEventManager.current.BeginSwitchingCurrentGroundChunk.AddListener(BeganSwitchingGroundChunk);
        GameEventManager.current.PurchasedCurrentGroundChunk.AddListener(GroundChunkJustPurchased);
    }

    private void SwitchedGroundChunk(){
        if(!ChunkPurchaseManager.current.ActiveChunkIsPurchased){
            Activate();
        }
    }

    private void BeganSwitchingGroundChunk(){
        if(ChunkPurchaseManager.current.ActiveChunkIsPurchased){
            Deactivate();
        }
    }

    private void GroundChunkJustPurchased(){
        Deactivate();
    }
    
    //TODO: Add an animation to move the Chunk Purchase UI in with the ground
    private void Activate(){
        purchasePriceText.text = "$" + ChunkPurchaseManager.current.AvailableChunkPrice.ToString("N0");
        purchaseUIObject.SetActive(true);
    }

    private void Deactivate(){
        purchaseUIObject.SetActive(false);
    }
    
    



}
