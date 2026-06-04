//This is the manager that makes tiles around the mouse partially transparent
using UnityEngine;
using System.Collections.Generic;

public class MouseHoverTransparency : MonoBehaviour
{
    RaycastHit HitInfo;

    private List<GameObject> deactivatedModels = new List<GameObject>();
    private List<GameObject> newlyDeactivatedModels = new List<GameObject>();

    private List<GameObject> deactivatedUIPopUps = new List<GameObject>();
    private List<GameObject> newlyDeactivatedUIPopUps = new List<GameObject>();

    public bool useRaycastingToFadeTilesUnderMouse = false;

    public float radiusToDeactivate = .5f;
    public float hoverTransparency = .43f;
    public float hoverTransparencyOfUIPopUps = .1f;

    public Vector3 previousMouseWorldCoordinates;

    void Update(){

        //This commented out line only enables hiding objects when you press W
        //if(!Input.GetKey(KeyCode.W)) return;


        //Gets current mouse snapped world grid coordinates
        Vector3 currentMousePosition = BuildingSystem.GetMouseWorldPosition();
        Vector3 snappedCoordinates = BuildingSystem.current.SnapCoordinateToGrid(currentMousePosition);
        //MouseMovedWorldPosition(snappedCoordinates);

        if(snappedCoordinates != previousMouseWorldCoordinates){
            MouseMovedWorldPosition(snappedCoordinates);
            previousMouseWorldCoordinates = snappedCoordinates;
        }




        if(!useRaycastingToFadeTilesUnderMouse){



            
        } else {
            // newlyDeactivatedModels.Clear();

        

            // Vector3 currentMousePosition = BuildingSystem.GetMouseWorldPosition();
            // AddModelsAtPointToDeactivationList(currentMousePosition, newlyDeactivatedModels);

            // //Checks around point with a radius of radiusToDeactivate
            // foreach(Vector3 point in GetCircleOfPointsAroundPoint(currentMousePosition, radiusToDeactivate)){
                
            //     AddModelsAtPointToDeactivationList(point, newlyDeactivatedModels);
            // }

            // //Activates models not being hovered over 
            // foreach(GameObject model in deactivatedModels){
            //     if(!newlyDeactivatedModels.Contains(model) && model != null){
            //         EnableObject(model);
            //     }
            // }

            // //Deactivates models being hovered over
            // foreach(GameObject model in newlyDeactivatedModels){
            //     if(model != null)
            //         DisableObject(model);
            // }

            // deactivatedModels.Clear();
            // deactivatedModels.AddRange(newlyDeactivatedModels);
        }
        

    }

    public void MouseMovedWorldPosition(Vector3 snappedCoordinates){
        newlyDeactivatedModels.Clear();
        newlyDeactivatedUIPopUps.Clear();


        //Hides tiles around the tile the mouse is currently on
        GameObject[] neighboringTilesToHide = GetNeighborsForTileTransparency(snappedCoordinates);
        foreach(GameObject tile in neighboringTilesToHide){
            if (tile != null) {
                AddNewlyDeactivatedModel(tile);        
            }else{
            }
        }

        //Hides the UI Pop ups of the tiles around the tile the mouse is currently on
        GameObject[] neighboringUIPopUpsToHide = GetNeighborsForUIPopUpTransparency(snappedCoordinates);
        foreach(GameObject tile in neighboringUIPopUpsToHide){
            if(tile != null){
                newlyDeactivatedUIPopUps.Add(tile);
            }
        }


            

        //Unhides models not being hovered over 
        foreach(GameObject model in deactivatedModels){
            if(!newlyDeactivatedModels.Contains(model) && model != null){
                EnableObject(model);
            }
        }
        //Hides models being hovered over
        foreach(GameObject model in newlyDeactivatedModels){
            if(model != null){
                DisableObject(model);
            }   
        }
        deactivatedModels.Clear();
        deactivatedModels.AddRange(newlyDeactivatedModels);



        //Unhides UI Pop ups of tiles not being hovered over
        foreach(GameObject tile in deactivatedUIPopUps){
            if(!newlyDeactivatedUIPopUps.Contains(tile) && tile != null){
                EnableUIPopUp(tile);
            }
        }
        //Hides UI Pop Ups of tiles being hovered over
        foreach(GameObject tile in newlyDeactivatedUIPopUps){
            if(tile != null){
                DisableUIPopUp(tile);
            }   
        }
        deactivatedUIPopUps.Clear();
        deactivatedUIPopUps.AddRange(newlyDeactivatedUIPopUps);
    }

    //Returns an array of gameobjects that designates which tiles should be hidden
    public GameObject[] GetNeighborsForTileTransparency(Vector3 coordinatesOfThisCell){
        List<GameObject> tileTransparencyNeighbors = new List<GameObject>();

        //Adds the model directly under the mouse
        GameObject[] objectsOnThisCell = GridManager.GM.GetGameObjectsInGridCell(coordinatesOfThisCell);
        //Debug.Log("Number of tiles under the mouse: " + objectsOnThisCell.Length);
        foreach(GameObject objectOnThisCell in objectsOnThisCell){
            if(objectOnThisCell == null){
                //Debug.Log("But this one is null");
            }
            tileTransparencyNeighbors.Add(objectOnThisCell);
        }
            
        //Adds the models of the three neighbors of the tile underneath the mouse
        int[] neighborsNeeded = new int[]{ 2, 3, 4 };
        GameObject[] neighbors = GridManager.GM.GetTileNeighbors(coordinatesOfThisCell, neighborsNeeded);
        foreach(GameObject neighbor in neighbors){
            if (neighbor != null) {
                tileTransparencyNeighbors.Add(neighbor);        
            }
        }


        return tileTransparencyNeighbors.ToArray();
    }

    //Returns an array of gameobjects that designates which tiles' UI Pop Ups should be hidden
    public GameObject[] GetNeighborsForUIPopUpTransparency(Vector3 coordinatesOfThisCell){
        List<GameObject> UIPopUpsTransparencyNeighbors = new List<GameObject>();

        //Gets coordinates of the cell directly below the cell that the mouse is on top of
        Vector3 coordinatesOfCellBelow = new Vector3(coordinatesOfThisCell.x + 1f, coordinatesOfThisCell.y, coordinatesOfThisCell.z - 1f);

        //Adds the model of the tile directly below the tile that's under the mouse
        GameObject[] objectsOnCellBelow = GridManager.GM.GetGameObjectsInGridCell(coordinatesOfCellBelow);
        foreach(GameObject objectOnCellBelow in objectsOnCellBelow){
            UIPopUpsTransparencyNeighbors.Add(objectOnCellBelow);
        }
        
        //Adds the model of the tile 2 below the tile that's under the mouse
        Vector3 coordinatesOfCell2Below = new Vector3(coordinatesOfThisCell.x + 2f, coordinatesOfThisCell.y, coordinatesOfThisCell.z - 2f);

        //Adds the model of the tile 2 below the tile that's under the mouse
        GameObject[] objectsOnCell2Below = GridManager.GM.GetGameObjectsInGridCell(coordinatesOfCell2Below);
        foreach(GameObject objectOnCell2Below in objectsOnCell2Below){
            UIPopUpsTransparencyNeighbors.Add(objectOnCell2Below);
        }
            
        //Adds the models of all the neighbors of the tile below the tile that the mosue is on top of
        int[] neighborsNeeded = new int[]{ 0, 1, 2, 3, 4, 5, 6, 7 };
        GameObject[] neighbors = GridManager.GM.GetTileNeighbors(coordinatesOfCellBelow, neighborsNeeded);
        foreach(GameObject neighbor in neighbors){
            if (neighbor != null) {
                UIPopUpsTransparencyNeighbors.Add(neighbor);        
            }
        }

        

        return UIPopUpsTransparencyNeighbors.ToArray();
    }

    public void DisableObject(GameObject tile){
        //Debug.Log("disabling " + tile);
        //tile.SetActive(false);
        MouseHoverHideTile mouseHoverHideTile = tile.GetComponent<MouseHoverHideTile>();
        if(mouseHoverHideTile != null){
            mouseHoverHideTile.HideTile(hoverTransparency);
        }
    }
    public void EnableObject(GameObject tile){
        //tile.SetActive(true);
        MouseHoverHideTile mouseHoverHideTile = tile.GetComponent<MouseHoverHideTile>();
        if(mouseHoverHideTile != null){
            mouseHoverHideTile.UnhideTile();
        }
    }

    public void DisableUIPopUp(GameObject tile){
        MouseHoverHideTile tileHider = tile.GetComponent<MouseHoverHideTile>();
        if(tileHider != null){
            tileHider.HideTileUIPopUps(hoverTransparencyOfUIPopUps);
        }
    }
    public void EnableUIPopUp(GameObject tile){
        MouseHoverHideTile tileHider = tile.GetComponent<MouseHoverHideTile>();
        if(tileHider != null){
            tileHider.UnHideTileUIPopUps();
        }
    }

    public void AddNewlyDeactivatedModel(GameObject tile){
        if(ShouldDeactivateModel(tile)){
            newlyDeactivatedModels.Add(tile);
        } else{
        }
    }

    // public void AddModelsAtPointToDeactivationList(Vector3 point, List<GameObject> deactivationList){
    //     Ray ray = Camera.main.ScreenPointToRay(point);

    //     //Debug.DrawRay(point, -Camera.main.transform.forward, Color.white, 30f);
    //     //Debug.DrawRay(Camera.main.transform.position, (point - Camera.main.transform.position).normalized*30, Color.white, 30f);
    //     RaycastHit[] hits = Physics.RaycastAll(Camera.main.transform.position, (point - Camera.main.transform.position).normalized*30, 30f);
    //     // Debug.Log("All hits:");
    //     // foreach(RaycastHit test in hits){
    //     //     if(test.collider != null && test.collider.gameObject != null){
    //     //         Debug.Log(test.collider.gameObject);
    //     //     }
            
    //     // }
    //     // Debug.Log("that's all the hits");
    //     if (hits.Length != 0){
    //         foreach(RaycastHit hit in hits){
                
    //             if (hit.collider != null && hit.collider.gameObject != null) {
    //                 if(!deactivationList.Contains(hit.collider.gameObject)){
    //                     AddNewlyDeactivatedModel(hit.collider.gameObject);
    //                     //deactivationList.Add(hit.collider.gameObject);
    //                     //hitModel.SetActive(false);
    //                 }
                
    //             }
    //         }
            
            
    //     }
    // }

    //Only disables models of tiles that are placed, aren't terrain, and don't have the delete box over them
    public bool ShouldDeactivateModel(GameObject model){

        Tile modelTile = model.GetComponent<Tile>();
        
        if(modelTile == null) return false;

        if(!model.GetComponent<PlaceableObject>().placed) return false;

        //Only Deactivates terrain tiles if it's under the mouse and another terrain tile is selected by the Building System
        if(modelTile.tileScriptableObject.isTerrain){
            bool activeTileIsTerrain = BuildingSystem.current.activeTile != null && BuildingSystem.current.activeTile.tileScriptableObject.isTerrain;
            if(!(activeTileIsTerrain && modelTile.MouseIsOnTile())){
                return false;
            }
        }

        if(TrashButtonScript.TBS.isSelected && modelTile.MouseIsOnTile()){

            return false;

        } 

        return true;
    }

    // public Vector3[] GetCircleOfPointsAroundPoint(Vector3 originPoint, float radius){
    //     Vector3[] returnArray = new Vector3[3];
    //     returnArray[0] = new Vector3(originPoint.x + (radius / Mathf.Sqrt(2f)), originPoint.y, originPoint.z + (radius / Mathf.Sqrt(2f)));
    //     returnArray[1] = new Vector3(originPoint.x - (radius / Mathf.Sqrt(2f)), originPoint.y, originPoint.z - (radius / Mathf.Sqrt(2f)));
    //     returnArray[2] = new Vector3(originPoint.x + (radius / Mathf.Sqrt(2f)), originPoint.y, originPoint.z - (radius / Mathf.Sqrt(2f)));
    //     //returnArray[4] = new Vector3(originPoint.x + radius, originPoint.y, originPoint.z);
    //     //returnArray[1] = new Vector3(originPoint.x + (radius / Mathf.Sqrt(2f)), originPoint.y, originPoint.z - (radius / Mathf.Sqrt(2f)));
    //     //returnArray[2] = new Vector3(originPoint.x, originPoint.y, originPoint.z - radius);
    //     //returnArray[3] = new Vector3(originPoint.x - (radius / Mathf.Sqrt(2f)), originPoint.y, originPoint.z - (radius / Mathf.Sqrt(2f)));
    //     //returnArray[4] = new Vector3(originPoint.x - radius, originPoint.y, originPoint.z);
    //     //returnArray[5] = new Vector3(originPoint.x - (radius / Mathf.Sqrt(2f)), originPoint.y, originPoint.z + (radius / Mathf.Sqrt(2f)));
    //     //returnArray[6] = new Vector3(originPoint.x, originPoint.y, originPoint.z + radius);
    //     //returnArray[7] = new Vector3(originPoint.x + (radius / Mathf.Sqrt(2f)), originPoint.y, originPoint.z + (radius / Mathf.Sqrt(2f)));
    //     return returnArray;
    // }

    // public GameObject GetFirstActiveChild(GameObject tileObject){
    //     //Debug.Log("Tile Object: " + tileObject);
    //     MeshRenderer[] allMeshRenderersInChildren = tileObject.GetComponentsInChildren<MeshRenderer>();
    //     for (int i = 0; i < allMeshRenderersInChildren.Length; i++)
    //     {
    //         if(allMeshRenderersInChildren[i].gameObject.activeSelf == true)
    //         {

    //             //if(tileObject.GetComponent<Tile>() != null)
    //             //Debug.Log("child game object: " + tileObject.gameObject.transform.GetChild(i).gameObject);
    //             return allMeshRenderersInChildren[i].gameObject;
    //         }
    //     }
    //     return null;
        
    // }
   
}
