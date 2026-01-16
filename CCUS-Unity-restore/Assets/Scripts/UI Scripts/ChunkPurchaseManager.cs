using UnityEngine;

public class ChunkPurchaseManager : MonoBehaviour
{
    public static ChunkPurchaseManager current;
    public GroundAreaExpansion groundAreaManager;

    private bool activeChunkIsPurchased = true;
    public bool ActiveChunkIsPurchased {
        get{

            int currentChunk = groundAreaManager.ActiveGroundChunk;
            int totalChunks = groundAreaManager.NumberOfGroundChunks;
            
            if(totalChunks == 1){ //if there's only one chunk, it's purchased
                return true;
            }else if(currentChunk == totalChunks - 1){ //if the current chunk is the last one, it's not purchased
                return false;
            }else{
                return true;
            }
        }
    }

    void Start()
    {
        if(current == null){
            current = this;
        }else{
            Destroy(this);
        }
        groundAreaManager = GetComponent<GroundAreaExpansion>();
    }

    public void EnableBuyingGroundChunks(){
        GroundAreaExpansion.GAE.AddGroundChunk();
    }

    public void PurchaseAvailableChunk(){
        GroundAreaExpansion.GAE.AddGroundChunk();
        GameEventManager.current.PurchasedCurrentGroundChunk.Invoke();
    }
}
