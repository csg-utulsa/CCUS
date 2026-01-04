using UnityEngine;
using System.Collections.Generic;

public class MouseHoverTransparency : MonoBehaviour
{
    RaycastHit HitInfo;

    private List<GameObject> deactivatedModels = new List<GameObject>();
    private List<GameObject> newlyDeactivatedModels = new List<GameObject>();

    public bool useRaycastingToFadeTilesUnderMouse = false;

    public float radiusToDeactivate = .5f;
    public float hoverTransparency = .43f;

    void Update(){

        //This commented out line only enables hiding objects when you press W
        //if(!Input.GetKey(KeyCode.W)) return;



        if(!useRaycastingToFadeTilesUnderMouse){
            newlyDeactivatedModels.Clear();

            //Gets current mouse tile neighbors
            Vector3 currentMousePosition = BuildingSystem.GetMouseWorldPosition();
            Vector3 snappedCoordinates = BuildingSystem.current.SnapCoordinateToGrid(currentMousePosition);
            int[] neighborsNeeded = new int[]{ 2, 3, 4 };

            //Disables model directly under mouse
            GameObject[] objectsOnThisCell = GridManager.GM.GetGameObjectsInGridCell(snappedCoordinates);
            foreach(GameObject tile in objectsOnThisCell){
                AddNewlyDeactivatedModel(tile);
            }

            GameObject[] neighbors = GridManager.GM.GetTileNeighbors(snappedCoordinates, neighborsNeeded);
            foreach(GameObject neighbor in neighbors){
                //Disables neighbors of model under mouse
                if (neighbor != null) {
                    AddNewlyDeactivatedModel(neighbor);        
                }
            }

            //Activates models not being hovered over 
            foreach(GameObject model in deactivatedModels){
                if(!newlyDeactivatedModels.Contains(model) && model != null){
                    EnableObject(model);
                }
            }

            //Deactivates models being hovered over
            foreach(GameObject model in newlyDeactivatedModels){
                if(model != null)
                    DisableObject(model);
            }

            deactivatedModels.Clear();
            deactivatedModels.AddRange(newlyDeactivatedModels);
        } else {
            newlyDeactivatedModels.Clear();

        

            Vector3 currentMousePosition = BuildingSystem.GetMouseWorldPosition();
            AddModelsAtPointToDeactivationList(currentMousePosition, newlyDeactivatedModels);

            //Checks around point with a radius of radiusToDeactivate
            foreach(Vector3 point in GetCircleOfPointsAroundPoint(currentMousePosition, radiusToDeactivate)){
                
                AddModelsAtPointToDeactivationList(point, newlyDeactivatedModels);
            }

            //Activates models not being hovered over 
            foreach(GameObject model in deactivatedModels){
                if(!newlyDeactivatedModels.Contains(model) && model != null){
                    EnableObject(model);
                }
            }

            //Deactivates models being hovered over
            foreach(GameObject model in newlyDeactivatedModels){
                if(model != null)
                    DisableObject(model);
            }

            deactivatedModels.Clear();
            deactivatedModels.AddRange(newlyDeactivatedModels);
        }
        







        





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

    public void AddNewlyDeactivatedModel(GameObject tile){
        if(ShouldDeactivateModel(tile)){
            newlyDeactivatedModels.Add(tile);
        }
    }

    public void AddModelsAtPointToDeactivationList(Vector3 point, List<GameObject> deactivationList){
        Ray ray = Camera.main.ScreenPointToRay(point);

        //Debug.DrawRay(point, -Camera.main.transform.forward, Color.white, 30f);
        //Debug.DrawRay(Camera.main.transform.position, (point - Camera.main.transform.position).normalized*30, Color.white, 30f);
        RaycastHit[] hits = Physics.RaycastAll(Camera.main.transform.position, (point - Camera.main.transform.position).normalized*30, 30f);
        // Debug.Log("All hits:");
        // foreach(RaycastHit test in hits){
        //     if(test.collider != null && test.collider.gameObject != null){
        //         Debug.Log(test.collider.gameObject);
        //     }
            
        // }
        // Debug.Log("that's all the hits");
        if (hits.Length != 0){
            foreach(RaycastHit hit in hits){
                
                if (hit.collider != null && hit.collider.gameObject != null) {
                    if(!deactivationList.Contains(hit.collider.gameObject)){
                        AddNewlyDeactivatedModel(hit.collider.gameObject);
                        //deactivationList.Add(hit.collider.gameObject);
                        //hitModel.SetActive(false);
                    }
                
                }
            }
            
            
        }
    }

    //Only disables models of tiles that are placed, aren't terrain, and don't have the delete box over them
    public bool ShouldDeactivateModel(GameObject model){
        Debug.Log("Checking if we should deactivate this model");
        if(model.GetComponent<Tile>() == null) return false;

        if(!model.GetComponent<PlaceableObject>().placed) return false;

        if(model.GetComponent<Tile>().tileScriptableObject.isTerrain) return false;

        if(TrashButtonScript.TBS.isSelected && model.GetComponent<Tile>().MouseIsOnTile()){
            Debug.Log("trash should be disabled");
            return false;

        } 

        return true;
    }

    public Vector3[] GetCircleOfPointsAroundPoint(Vector3 originPoint, float radius){
        Vector3[] returnArray = new Vector3[3];
        returnArray[0] = new Vector3(originPoint.x + (radius / Mathf.Sqrt(2f)), originPoint.y, originPoint.z + (radius / Mathf.Sqrt(2f)));
        returnArray[1] = new Vector3(originPoint.x - (radius / Mathf.Sqrt(2f)), originPoint.y, originPoint.z - (radius / Mathf.Sqrt(2f)));
        returnArray[2] = new Vector3(originPoint.x + (radius / Mathf.Sqrt(2f)), originPoint.y, originPoint.z - (radius / Mathf.Sqrt(2f)));
        //returnArray[4] = new Vector3(originPoint.x + radius, originPoint.y, originPoint.z);
        //returnArray[1] = new Vector3(originPoint.x + (radius / Mathf.Sqrt(2f)), originPoint.y, originPoint.z - (radius / Mathf.Sqrt(2f)));
        //returnArray[2] = new Vector3(originPoint.x, originPoint.y, originPoint.z - radius);
        //returnArray[3] = new Vector3(originPoint.x - (radius / Mathf.Sqrt(2f)), originPoint.y, originPoint.z - (radius / Mathf.Sqrt(2f)));
        //returnArray[4] = new Vector3(originPoint.x - radius, originPoint.y, originPoint.z);
        //returnArray[5] = new Vector3(originPoint.x - (radius / Mathf.Sqrt(2f)), originPoint.y, originPoint.z + (radius / Mathf.Sqrt(2f)));
        //returnArray[6] = new Vector3(originPoint.x, originPoint.y, originPoint.z + radius);
        //returnArray[7] = new Vector3(originPoint.x + (radius / Mathf.Sqrt(2f)), originPoint.y, originPoint.z + (radius / Mathf.Sqrt(2f)));
        return returnArray;
    }

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
