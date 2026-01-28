using UnityEngine;
using System;
using TMPro;

public class ActivateChunkPurchaseUI : MonoBehaviour
{
    public static ActivateChunkPurchaseUI current;

    public GameObject purchaseUIObject;

    public TextMeshProUGUI purchasePriceText;

    public MouseOverImage areaPriceImage;

    private bool isActivated = false;
    
    void Start(){
        if(current == null){
            current = this;
        }else{
            Destroy(this);
        }
        purchasePriceText = GetComponentInChildren<TextMeshProUGUI>(true);

        if(purchaseUIObject.GetComponentInChildren<MouseOverImage>() != null){
           areaPriceImage = purchaseUIObject.GetComponentInChildren<MouseOverImage>(); 
        }
        
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
        isActivated = true;
    }

    private void Deactivate(){
        purchaseUIObject.SetActive(false);
        isActivated = false;
    }


    //Detects if mouse is over the price button
    public bool IsMouseOverAreaPrice(){
        if(isActivated && areaPriceImage != null && areaPriceImage.IsMouseOverImage()){
            return true;
        } else{
            return false;
        }
    }
    
    



}
