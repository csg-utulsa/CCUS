using UnityEngine;
using System;
using System.Collections.Generic;

public class GroundAreaExpansion : MonoBehaviour
{
    
    public static GroundAreaExpansion GAE;
    public GameObject camera;
    private Vector3 cameraStartPosition;
    
    public List<GameObject> allGroundChunks = new List<GameObject>();
    public GameObject groundChunkPrefab;
    [SerializeField] private int widthOfGrid = 10;
    [SerializeField] private int distanceBetweenGroundChunks = 3;
    public int ActiveGroundChunk {get; set;} = 0;
    public int NumberOfGroundChunks { get; set; } = 1;

    void Awake(){
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
        for(int i = 0; i < allGroundChunksInScene.Length; i++){
            allGroundChunks.Add(allGroundChunksInScene[i]);
        }
        NumberOfGroundChunks = allGroundChunks.Count;
    }

    public void AddGroundChunk(){
        Vector3 currentFarRightChunk = allGroundChunks[allGroundChunks.Count - 1].transform.position;
        Vector3 positionOfNewGroundChunk = new Vector3(currentFarRightChunk.x + widthOfGrid + distanceBetweenGroundChunks, currentFarRightChunk.y, currentFarRightChunk.z + widthOfGrid + distanceBetweenGroundChunks);
        if(groundChunkPrefab != null)
            allGroundChunks.Add(Instantiate(groundChunkPrefab, positionOfNewGroundChunk, Quaternion.identity));
        NumberOfGroundChunks++;

        //Turns the move arrow to the right on
        SwitchChunkArrowManager.SCAM.UpdateArrowVisibility(NumberOfGroundChunks, ActiveGroundChunk);
    }

    public void MoveRight(){
        //Won't move right if it's as far right as possible
        if(ActiveGroundChunk == (allGroundChunks.Count - 1)) {
            return;
        }else{
            ActiveGroundChunk++;
            UpdateCameraPosition();

            //Updates visibility of left/right arrows
            SwitchChunkArrowManager.SCAM.UpdateArrowVisibility(NumberOfGroundChunks, ActiveGroundChunk);
        }
       
    }

    public void MoveLeft(){
        //Won't move left if it's already as far left as possible
        if(ActiveGroundChunk == 0){
            return;
        }else{
            ActiveGroundChunk--;
            UpdateCameraPosition();

            //Updates visibility of left/right arrows
            SwitchChunkArrowManager.SCAM.UpdateArrowVisibility(NumberOfGroundChunks, ActiveGroundChunk);
        }


    }

    public void UpdateCameraPosition(){
        float offset = (widthOfGrid * ActiveGroundChunk) + (distanceBetweenGroundChunks * ActiveGroundChunk);
        Vector3 newCameraPosition = new Vector3(cameraStartPosition.x + offset, cameraStartPosition.y, cameraStartPosition.z + offset);
        camera.GetComponent<CameraMove>().MoveCamera(newCameraPosition);
        //camera.transform.position = newCameraPosition;
    }

    //Puts the GameObjects in order from lowest to highest z position.
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
