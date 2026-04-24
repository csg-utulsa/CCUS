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

    Vector3 previousGridPosition;

    public bool isSelected {get; set;} = false;

    public bool HasBeenSelectedAtLeastOnce {get; set;} = false;

    bool hasMoved = false;

    bool chunkIsSwitching = false;

    public void Start(){
        if(TBS == null){
            TBS = this;
        }
        _buildingSystem = BuildingSystem.current;

        GameEventManager.current.GetEvent(EventType.E.BeginSwitchingCurrentGroundChunk).AddListener(BeganSwitchingChunks);
        GameEventManager.current.GetEvent(EventType.E.SwitchedCurrentGroundChunk).AddListener(FinishedSwitchingChunks);
    }   

    //These two functions keep track of whether or not the chunk is currently switching.
    private void BeganSwitchingChunks(){
        chunkIsSwitching = true;
    }
    private void FinishedSwitchingChunks(){
        chunkIsSwitching = false;
    }

    public void trashButtonClicked(){
        HasBeenSelectedAtLeastOnce = true;
        if(isSelected){
            trashButtonDeselected();
            GameEventManager.current.GetEvent(EventType.E.DeselectedTrashButton).Invoke();
            return;
        }
        isSelected = true;
        selectedButtonGraphic.transform.position = transform.position;
        currentRedDeleteCube = Instantiate(redDeleteCubePrefab, redDeleteCubePrefab.transform.position, redDeleteCubePrefab.transform.rotation);
        BuildingSystem.current.deselectActiveObject();
        selectedButtonGraphic.SetActive(true);

        //Sets the previousGridPosition variable :)
        Vector3 mouseWorldPosition = BuildingSystem.GetMouseWorldPosition();
        Vector3 mouseGridCoordinate = BuildingSystem.current.SnapCoordinateToGrid(mouseWorldPosition);
        previousGridPosition = new Vector3(mouseGridCoordinate.x, currentRedDeleteCube.transform.position.y, mouseGridCoordinate.z);
        
        GameEventManager.current.GetEvent(EventType.E.SelectedTrashButton).Invoke();
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
        //Deselects the trash button if you right click
        if(Input.GetMouseButtonDown(1)){
            trashButtonDeselected();
        }

        if(isSelected && currentRedDeleteCube != null && BuildingSystem.isMouseOverScreen()){
            currentRedDeleteCube.SetActive(true);
            Vector3 mouseWorldPosition = BuildingSystem.GetMouseWorldPosition();
            Vector3 mouseGridCoordinate = BuildingSystem.current.SnapCoordinateToGrid(mouseWorldPosition);
            currentRedDeleteCube.transform.position = new Vector3(mouseGridCoordinate.x, currentRedDeleteCube.transform.position.y, mouseGridCoordinate.z);
            
            if(Input.GetMouseButtonDown(0) || (Input.GetMouseButton(0) && previousGridPosition != currentRedDeleteCube.transform.position)){ // object deleting     || (Input.GetMouseButton(0) && hasMoved)
                deleteTile();
            }

            previousGridPosition = currentRedDeleteCube.transform.position;
        } else if(!BuildingSystem.isMouseOverScreen() && currentRedDeleteCube != null){
            currentRedDeleteCube.SetActive(false);
        }
        
    }

    public bool mouseIsOverTrashButton(){
        Vector2 mousePos = Input.mousePosition;
        if(GetComponent<RectTransform>() != null){
            return RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), mousePos);
        }
        return false;
    }

    //Deletes a tile
    public void deleteTile(){
        //Doesn't try to delete if the user is currently switching chunks or if the delete cube is over the void
        if(!chunkIsSwitching && !isDeleteCubeOverVoid()){
            GameObject[] gameObjectsToDelete =  GridManager.GM.GetGameObjectsInGridCell(currentRedDeleteCube);

            //Deletes the objects on top of terrain first, before deleting the terrain
            if(gameObjectsToDelete.Length > 1){
                foreach(GameObject gameObjectToDelete in gameObjectsToDelete){
                    Tile gameObjectToDeleteTile = gameObjectToDelete.GetComponent<Tile>();
                    if(gameObjectToDeleteTile != null && !gameObjectToDeleteTile.tileScriptableObject.isTerrain){
                        gameObjectToDeleteTile.DeleteThisTile();
                    }
                }
            }else{
                //if only one object on tile, deletes just that tile
                foreach(GameObject gameObjectToDelete in gameObjectsToDelete){
                    Tile gameObjectToDeleteTile = gameObjectToDelete.GetComponent<Tile>();
                    if(gameObjectToDeleteTile != null){
                        gameObjectToDeleteTile.DeleteThisTile();
                    }
                } 
            }
            
        }
        //LevelManager.LM.UpdateNetCarbonAndMoney();
    }

    //This function raycasts straight down over a tile to find out what's on it. TODO FIXME: Make & use a Grid System function for this
    public bool isDeleteCubeOverVoid(){
        BuildingSystem current = BuildingSystem.current;
        Vector3 activeObjectPositionOnGrid = current.SnapCoordinateToGrid(currentRedDeleteCube.transform.position);
        Vector3 raycastStartPosition = new Vector3(activeObjectPositionOnGrid.x, activeObjectPositionOnGrid.y + 5.0f, activeObjectPositionOnGrid.z);

        bool isOverVoid = true;
        isOverVoid = !(Physics.Raycast(raycastStartPosition, -Vector3.up, 100.0F, current.groundLayer));

        return isOverVoid;
    }



}
