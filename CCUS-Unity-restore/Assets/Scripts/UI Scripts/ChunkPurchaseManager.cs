using UnityEngine;

public class ChunkPurchaseManager : MonoBehaviour
{
    public static ChunkPurchaseManager current;
    public GroundAreaExpansion groundAreaManager;

    public int[] pricesOfChunks;
    public bool[] purchasedChunks;

    private int availableChunkPrice = 5000;
    public int AvailableChunkPrice {
        get{
            int totalChunks = groundAreaManager.NumberOfGroundChunks;
            int maxChunks = groundAreaManager.MaxNumberOfChunks;
            if(totalChunks <= maxChunks){
                if(totalChunks <= pricesOfChunks.Length){
                    return pricesOfChunks[totalChunks - 1];
                } else{
                    return pricesOfChunks[pricesOfChunks.Length - 1];
                }
            }else{
                return pricesOfChunks[pricesOfChunks.Length - 1];
            }
        }
    }

    private bool activeChunkIsPurchased = true;
    public bool ActiveChunkIsPurchased {
        get{

            int currentChunk = groundAreaManager.ActiveGroundChunk;
            if(currentChunk < purchasedChunks.Length && purchasedChunks[currentChunk]){
                return true;
            }else{
                return false;
            }
            // int totalChunks = groundAreaManager.NumberOfGroundChunks;
            
            // if(totalChunks == 1){ //if there's only one chunk, it's purchased
            //     return true;
            // }else if(currentChunk == totalChunks - 1){ //if the current chunk is the last one, it's not purchased
            //     return false;
            // }else{
            //     return true;
            // }
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


        purchasedChunks = new bool[groundAreaManager.MaxNumberOfChunks];
        //first chunk is already purchased
        purchasedChunks[0] = true;
        for(int i = 1 ; i < purchasedChunks.Length; i++){
            purchasedChunks[i] = false;
        }
    }

    public void EnableBuyingGroundChunks(){
        GroundAreaExpansion.GAE.AddGroundChunk();
    }

    public void PurchaseAvailableChunk(){

        //Checks if there's enough money to purchase the available chunk
        bool chunkIsPurchaseable = CheckIfChunkIsPurchaseable(true);


        //Spends price of chunk
        LevelManager.LM.AdjustMoney(-AvailableChunkPrice);

        //Saves that this chunk has been purchased
        if(groundAreaManager.ActiveGroundChunk < purchasedChunks.Length){
           purchasedChunks[groundAreaManager.ActiveGroundChunk] = true; 
        }
        


        //If there's enough money, it will purchase the chunk
        if(chunkIsPurchaseable){
            groundAreaManager.AddGroundChunk();
            GameEventManager.current.PurchasedCurrentGroundChunk.Invoke();
        }

    }

    public bool CheckIfChunkIsPurchaseable(bool isAttemptingToPurchase){
        if(LevelManager.LM.GetMoney() >= AvailableChunkPrice){
            return true;
        } else{
            if(isAttemptingToPurchase){
                unableToPlaceTileUI._unableToPlaceTileUI.notEnoughMoney();
            }
            return false;
        }
    }
}
