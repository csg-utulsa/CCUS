using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

public class GroundAreaExpansion : MonoBehaviour
{

    
    
    public static GroundAreaExpansion current;
    public GameObject camera;
    public GameObject groundChunkPrefab;
    private Vector3 cameraStartPosition;

    public GameObject previousVisibleGround;
    public GameObject currentVisibleGround;
    
    //public List<GameObject> allGroundChunks = new List<GameObject>();
    public List<Vector3> positionsOfGroundChunks = new List<Vector3>();
    public float timeToSwitchChunks = .2f;

    public int MaxNumberOfChunks {
        get{
            return maxNumberOfChunks;
        }
        set{
            maxNumberOfChunks = value;

            bool[] newPurchasedChunkArray = new bool[maxNumberOfChunks];

            bool[] oldPurchasedChunks = ChunkPurchaseManager.current.purchasedChunks;

            for(int i = 0; i < oldPurchasedChunks.Length && i < newPurchasedChunkArray.Length; i++){
                newPurchasedChunkArray[i] = oldPurchasedChunks[i];
            }

            ChunkPurchaseManager.current.purchasedChunks = newPurchasedChunkArray;
        }
    }
    [SerializeField] private int maxNumberOfChunks = 7;
    [SerializeField] public int widthOfGrid = 10;
    [SerializeField] private int distanceBetweenGroundChunks = 3;

    public int ActiveGroundChunk {get; set;} = 0;
    public int NumberOfGroundChunks { get; set; } = 1;

    public bool IsSwitchingGroundChunks{
        get{
            return isSwitchingGroundChunks;
        }
    }
    private bool isSwitchingGroundChunks = false;

    void Awake(){
        if(current == null){
            current = this;
        } else{
            Destroy(this);
        }
    }
    

    void Start(){
        
        cameraStartPosition = camera.transform.position;

        //Adds all the ground chunks currently in the scene
        GameObject[] allGroundChunksInScene = GameObject.FindGameObjectsWithTag("Ground");
        if(allGroundChunksInScene.Length > 1){
            SelectionSortByZPosition(allGroundChunksInScene);
        }

        //Stores the visible ground object
        previousVisibleGround = allGroundChunksInScene[0];
        currentVisibleGround = previousVisibleGround;


        for(int i = 0; i < allGroundChunksInScene.Length; i++){

            //Saves position of each ground chunk
            positionsOfGroundChunks.Add(allGroundChunksInScene[i].transform.position);

        }
        NumberOfGroundChunks = positionsOfGroundChunks.Count;
    }



    public void AddGroundChunk(){

        //Doesn't add a new ground chunk if the number of ground chunks is maxed out.
        if(NumberOfGroundChunks >= MaxNumberOfChunks) return;

        //If this is the first new ground chunk, it alerts the user
        if(NumberOfGroundChunks == 1){
            unableToPlaceTileUI._unableToPlaceTileUI.NewAreaUnlockedNotification();
        }
        


        //Creates a new ground chunk object to the right
        Vector3 currentFarRightChunk = positionsOfGroundChunks[positionsOfGroundChunks.Count - 1];
        Vector3 positionOfNewGroundChunk = new Vector3(currentFarRightChunk.x + widthOfGrid + distanceBetweenGroundChunks, currentFarRightChunk.y, currentFarRightChunk.z + widthOfGrid + distanceBetweenGroundChunks);

        NumberOfGroundChunks++;


        //Saves position of ground chunk
        positionsOfGroundChunks.Add(positionOfNewGroundChunk);

        //Adds new Chunk to GridManager
        GridManager.GM.AddNewChunk(positionsOfGroundChunks[positionsOfGroundChunks.Count - 1]);

        //Turns the correct switch chunk arrow on
        SwitchChunkArrowManager.current.UpdateArrowVisibility(NumberOfGroundChunks, ActiveGroundChunk);

        //Calls event for when a new area is unlocked
        GameEventManager.current.GetEvent(EventType.E.NewAreaUnlocked).Invoke();
    }

    public void MoveRight(){

        //Arrows don't work while switching ground chunks
        if(isSwitchingGroundChunks) return;

        //Won't move right if it's as far right as possible
        if(ActiveGroundChunk == (positionsOfGroundChunks.Count - 1)) {
            return;
        }else{
            SwitchToGroundChunk(ActiveGroundChunk + 1);

            //Calls event for switching to the ground chunk on the right
            GameEventManager.current.GetEvent(EventType.E.SwitchedChunkRight).Invoke();
        }
       
    }

    public void MoveLeft(){
        //Arrows don't work while switching ground chunks
        if(isSwitchingGroundChunks) return;

        //Won't move left if it's already as far left as possible
        if(ActiveGroundChunk == 0){
            return;
        }else{
            SwitchToGroundChunk(ActiveGroundChunk - 1);

            //Calls event for switching to the ground chunk on the right
            GameEventManager.current.GetEvent(EventType.E.SwitchedChunkLeft).Invoke();
            
        }


    }

    private void SwitchToGroundChunk(int targetGroundChunk){

        

        isSwitchingGroundChunks = true;

        int previousGroundChunk = ActiveGroundChunk;

        bool isSwitchingLeft = (targetGroundChunk < previousGroundChunk);

        ActiveGroundChunk = targetGroundChunk;

        //Moves the camera to the new position
        UpdateCameraPosition();

        //Updates visibility of left/right arrows
        SwitchChunkArrowManager.current.UpdateArrowVisibility(NumberOfGroundChunks, ActiveGroundChunk);


        //Instantiates new ground chunk area
        previousVisibleGround = currentVisibleGround;
        currentVisibleGround = CreateNewVisibleGroundForChunk(targetGroundChunk);


        //Visually Loads the new chunk
        TileMeshLoadManager.current.LoadGridChunk(targetGroundChunk);

        //Calls event for whether the chunk is switched to the left or the right
        if(isSwitchingLeft){
           GameEventManager.current.GetEvent(EventType.E.BeginSwitchingChunkLeft).Invoke(); 
        } else{
            GameEventManager.current.GetEvent(EventType.E.BeginSwitchingChunkRight).Invoke();
        }
        
        //Calls event for when chunk is switched
        GameEventManager.current.GetEvent(EventType.E.BeginSwitchingCurrentGroundChunk).Invoke();
        GameEventManager.current.GetEvent(EventType.E.BeginSwitchingCurrentGroundChunkLate).Invoke();

        StartCoroutine(WaitToFinishSwitchingChunks(previousGroundChunk));
    }

    private IEnumerator WaitToFinishSwitchingChunks(int previousGroundChunk) {
        yield return new WaitForSeconds(timeToSwitchChunks);
        isSwitchingGroundChunks = false;
        Destroy(previousVisibleGround);

        //Visually disables the meshes for the old chunk
        TileMeshLoadManager.current.UnloadGridChunk(previousGroundChunk);

        //Calls event for when chunk is switched
        GameEventManager.current.GetEvent(EventType.E.SwitchedCurrentGroundChunk).Invoke();

        //Calls late event for after chunk is switched. Useful to update things after everything else has changed.
        GameEventManager.current.GetEvent(EventType.E.SwitchedCurrentGroundChunkLate).Invoke();
    }

    private GameObject CreateNewVisibleGroundForChunk(int groundChunkNumber){
        return Instantiate(groundChunkPrefab, positionsOfGroundChunks[groundChunkNumber], Quaternion.identity);
    }

    public void UpdateCameraPosition(){
        float offset = (widthOfGrid * ActiveGroundChunk) + (distanceBetweenGroundChunks * ActiveGroundChunk);
        Vector3 newCameraPosition = new Vector3(cameraStartPosition.x + offset, cameraStartPosition.y, cameraStartPosition.z + offset);
        camera.GetComponent<CameraMove>().MoveCamera(newCameraPosition, timeToSwitchChunks);
    }



    //Puts the GameObjects in order from lowest to highest z position. (uses selection sort)
    public void SelectionSortByZPosition(GameObject[] objectsToSort){
        for(int i = 0; i < objectsToSort.Length; i++){

            float lowestZPosition = objectsToSort[i].transform.position.z;
            int positionOfLowestValue = i;

            for(int j = i; j < objectsToSort.Length; j++){
                if(objectsToSort[j].transform.position.z < lowestZPosition){
                    lowestZPosition = objectsToSort[j].transform.position.z;
                    positionOfLowestValue = j;
                }
            }
            GameObject storedGameObject = objectsToSort[i];
            objectsToSort[i] = objectsToSort[positionOfLowestValue];
            objectsToSort[positionOfLowestValue] = storedGameObject;
        }
    }


}
