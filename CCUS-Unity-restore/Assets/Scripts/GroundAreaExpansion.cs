using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

public class GroundAreaExpansion : MonoBehaviour
{


    
    public static GroundAreaExpansion GAE;
    public GameObject camera;
    public GameObject groundChunkPrefab;
    private Vector3 cameraStartPosition;

    public GameObject previousVisibleGround;
    public GameObject currentVisibleGround;
    
    //public List<GameObject> allGroundChunks = new List<GameObject>();
    public List<Vector3> positionsOfGroundChunks = new List<Vector3>();
    public float timeToSwitchChunks = .2f;

    
    [SerializeField] private int widthOfGrid = 10;
    [SerializeField] private int distanceBetweenGroundChunks = 3;

    public int ActiveGroundChunk {get; set;} = 0;
    public int NumberOfGroundChunks { get; set; } = 1;

    private bool isSwitchingGroundChunks = false;
    

    void Start(){
        if(GAE == null){
            GAE = this;
        } else{
            Destroy(this);
        }

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
            //allGroundChunks.Add(allGroundChunksInScene[i]);

            //Saves position of each ground chunk
            positionsOfGroundChunks.Add(allGroundChunksInScene[i].transform.position);


            //Creates a new Grid Chunk Data Unit for each ground chunk
            GridDataLoader.current.CreateNewGridChunk();
        }
        NumberOfGroundChunks = positionsOfGroundChunks.Count;
    }



    public void AddGroundChunk(){

        //Creates a new ground chunk object to the right
        Vector3 currentFarRightChunk = positionsOfGroundChunks[positionsOfGroundChunks.Count - 1];
        Vector3 positionOfNewGroundChunk = new Vector3(currentFarRightChunk.x + widthOfGrid + distanceBetweenGroundChunks, currentFarRightChunk.y, currentFarRightChunk.z + widthOfGrid + distanceBetweenGroundChunks);
        // if(groundChunkPrefab != null)
        //     allGroundChunks.Add(Instantiate(groundChunkPrefab, positionOfNewGroundChunk, Quaternion.identity));
        NumberOfGroundChunks++;

        //Adds a new Ground Chunk Data Object
        GridDataLoader.current.CreateNewGridChunk();

        //Saves position of ground chunk
        positionsOfGroundChunks.Add(positionOfNewGroundChunk);

        //Turns the correct switch chunk arrow on
        SwitchChunkArrowManager.current.UpdateArrowVisibility(NumberOfGroundChunks, ActiveGroundChunk);
    }

    public void MoveRight(){

        //Arrows don't work while switching ground chunks
        if(isSwitchingGroundChunks) return;

        //Won't move right if it's as far right as possible
        if(ActiveGroundChunk == (positionsOfGroundChunks.Count - 1)) {
            return;
        }else{
            SwitchToGroundChunk(ActiveGroundChunk + 1);
            UpdateCameraPosition();

            //Updates visibility of left/right arrows
            SwitchChunkArrowManager.current.UpdateArrowVisibility(NumberOfGroundChunks, ActiveGroundChunk);
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
            
        }


    }

    public void SwitchToGroundChunk(int targetGroundChunk){

        //Calls event for when chunk is switched
        GameEventManager.current.BeginSwitchingCurrentGroundChunk.Invoke();

        isSwitchingGroundChunks = true;

        int previousGroundChunk = ActiveGroundChunk;
        ActiveGroundChunk = targetGroundChunk;

        //Moves the camera to the new position
        UpdateCameraPosition();

        //Updates visibility of left/right arrows
        SwitchChunkArrowManager.current.UpdateArrowVisibility(NumberOfGroundChunks, ActiveGroundChunk);


        //Instantiates new ground chunk area
        previousVisibleGround = currentVisibleGround;
        currentVisibleGround = CreateNewVisibleGroundForChunk(targetGroundChunk);

        //Load new chunk (also switches grid manager center)
        GridDataLoader.current.SwitchToGridChunk(targetGroundChunk, positionsOfGroundChunks[targetGroundChunk], timeToSwitchChunks);


        StartCoroutine(WaitToFinishSwitchingChunks());
    }

    public IEnumerator WaitToFinishSwitchingChunks() {
        yield return new WaitForSeconds(timeToSwitchChunks);
        isSwitchingGroundChunks = false;
        Destroy(previousVisibleGround);

        //Calls event for when chunk is switched
        GameEventManager.current.SwitchedCurrentGroundChunk.Invoke();
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
