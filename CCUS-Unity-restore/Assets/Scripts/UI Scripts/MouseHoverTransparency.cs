using UnityEngine;
using System.Collections.Generic;

public class MouseHoverTransparency : MonoBehaviour
{
    RaycastHit HitInfo;

    private List<GameObject> deactivatedModels = new List<GameObject>();
    private List<GameObject> newlyDeactivatedModels = new List<GameObject>();

    public float radiusToDeactivate = .5f;

    void Update(){

        newlyDeactivatedModels.Clear();

        if(!Input.GetKey(KeyCode.W)) return;

        //Gets current mouse tile neighbors
        Vector3 currentMousePosition = BuildingSystem.GetMouseWorldPosition();
        Vector3 snappedCoordinates = BuildingSystem.current.SnapCoordinateToGrid(currentMousePosition);
        int[] neighborsNeeded = new int[]{ 2, 3, 4 };
        GameObject[] neighbors = GridManager.GM.GetTileNeighbors(snappedCoordinates, neighborsNeeded);
        foreach(GameObject neighbor in neighbors){
            //Only disables placed, non-terrain tiles
            if (neighbor != null && neighbor.GetComponent<Tile>() != null && neighbor.GetComponent<PlaceableObject>().placed && !neighbor.GetComponent<Tile>().tileScriptableObject.isTerrain) {
                newlyDeactivatedModels.Add(neighbor);                
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

    public void DisableObject(GameObject tile){
        //Debug.Log("disabling " + tile);
        //tile.SetActive(false);
        MouseHoverHideTile mouseHoverHideTile = tile.GetComponent<MouseHoverHideTile>();
        if(mouseHoverHideTile != null){
            mouseHoverHideTile.HideTile();
        }
    }
    public void EnableObject(GameObject tile){
        //tile.SetActive(true);
        MouseHoverHideTile mouseHoverHideTile = tile.GetComponent<MouseHoverHideTile>();
        if(mouseHoverHideTile != null){
            mouseHoverHideTile.UnhideTile();
        }
    }


    public void AddModelsAtPointToDeactivationList(Vector3 point, List<GameObject> deactivationList){
        Ray ray = Camera.main.ScreenPointToRay(point);

        //Debug.DrawRay(point, -Camera.main.transform.forward, Color.white, 30f);
        Debug.DrawRay(Camera.main.transform.position, (point - Camera.main.transform.position).normalized*30, Color.white, 30f);
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
                //Only disables models of tiles that are placed and aren't terrain
                if (hit.collider != null && hit.collider.gameObject.GetComponent<Tile>() != null && hit.collider.GetComponent<PlaceableObject>().placed && !hit.collider.gameObject.GetComponent<Tile>().tileScriptableObject.isTerrain) {
                    if(hit.collider.gameObject != null && !deactivationList.Contains(hit.collider.gameObject)){
                        deactivationList.Add(hit.collider.gameObject);
                        //hitModel.SetActive(false);
                    }
                
                }
            }
            
            
        }
    }

    public Vector3[] GetCircleOfPointsAroundPoint(Vector3 originPoint, float radius){
        Vector3[] returnArray = new Vector3[2];
        returnArray[0] = new Vector3(originPoint.x + radius, originPoint.y, originPoint.z);
        returnArray[1] = new Vector3(originPoint.x - radius, originPoint.y, originPoint.z);
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
