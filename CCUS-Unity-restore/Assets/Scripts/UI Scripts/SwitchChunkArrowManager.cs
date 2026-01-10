using UnityEngine;

public class SwitchChunkArrowManager : MonoBehaviour
{

    public GameObject rightArrow;
    public GameObject leftArrow;

    public static SwitchChunkArrowManager current;

    void Start(){
        if(current == null){
            current = this;
        }else{
            Destroy(this);
        }
    }

    public void UpdateArrowVisibility(int numberOfGroundChunks, int activeGroundChunk){
        
        if(activeGroundChunk == 0){
            leftArrow.SetActive(false);
        } else{
            leftArrow.SetActive(true);
        }
        if(activeGroundChunk == (numberOfGroundChunks - 1)){
            rightArrow.SetActive(false);
        }else{
            rightArrow.SetActive(true);
        }
    }

}
