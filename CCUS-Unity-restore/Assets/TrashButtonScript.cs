using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashButtonScript : MonoBehaviour
{
    public GameObject selectedButtonGraphic;

    public GameObject redDeleteCubePrefab;

    public GameObject currentRedDeleteCube;

    public BuildingSystem _buildingSystem;

    public static TrashButtonScript TBS;

    bool isSelected = false;

    public void Start(){
        if(TBS == null){
            TBS = this;
        }
        _buildingSystem = BuildingSystem.current;
    }

    public void trashButtonClicked(){
        if(isSelected){
            trashButtonDeselected();
            return;
        }
        isSelected = true;
        selectedButtonGraphic.SetActive(true);
        selectedButtonGraphic.transform.position = transform.position;
        currentRedDeleteCube = Instantiate(redDeleteCubePrefab, redDeleteCubePrefab.transform.position, redDeleteCubePrefab.transform.rotation);
        BuildingSystem.current.deselectActiveObject();
    }

    public void trashButtonDeselected(){
        isSelected = false;
        //fix next line
        selectedButtonGraphic.SetActive(false);

        Destroy(currentRedDeleteCube);
    }



    // Update is called once per frame
    void Update()
    {

        if(isSelected && currentRedDeleteCube != null && BuildingSystem.isMouseOverScreen()){
            currentRedDeleteCube.SetActive(true);
            Vector3 mouseWorldPosition = BuildingSystem.GetMouseWorldPosition();
            Vector3 mouseGridCoordinate = BuildingSystem.current.SnapCoordinateToGrid(mouseWorldPosition);
            Debug.Log(mouseGridCoordinate);
            currentRedDeleteCube.transform.position = new Vector3(mouseGridCoordinate.x, currentRedDeleteCube.transform.position.y, mouseGridCoordinate.z);
            if(Input.GetMouseButtonDown(0)){ // object deleting
                //TODO: later make it first delete the top tile, then the terrain
                if(!isDeleteCubeOverVoid()){
                    GameObject[] gameObjectsToDelete =  GridManager.GM.GetGameObjectsInGridCell(currentRedDeleteCube);
                    foreach(GameObject gameObjectToDelete in gameObjectsToDelete){
                        GridManager.GM.RemoveObject(gameObjectToDelete);
                        Destroy(gameObjectToDelete);
                    }
                }
                
            }
        } else if(!BuildingSystem.isMouseOverScreen() && currentRedDeleteCube != null){
            currentRedDeleteCube.SetActive(false);
        }
        
    }

    //This function raycasts straight down over a tile to find out what's on it
    public bool isDeleteCubeOverVoid(){
        BuildingSystem current = BuildingSystem.current;
        Vector3 activeObjectPositionOnGrid = current.SnapCoordinateToGrid(currentRedDeleteCube.transform.position);
        Vector3 raycastStartPosition = new Vector3(activeObjectPositionOnGrid.x, activeObjectPositionOnGrid.y + 5.0f, activeObjectPositionOnGrid.z);

        bool isOverVoid = true;
        isOverVoid = !(Physics.Raycast(raycastStartPosition, -Vector3.up, 100.0F, current.groundLayer));

        return isOverVoid;
    }



}
