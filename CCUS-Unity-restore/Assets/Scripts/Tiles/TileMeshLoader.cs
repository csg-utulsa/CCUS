// Description: Turns off tile's Mesh when chunk loads or unloads by disabling all of the tile's children


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileMeshLoader : MonoBehaviour
{
    public GameObject[] tileChildrenObjects;

    bool isLoaded = true;

    public List<GameObject> activatedChildrenObjects;

    //Finds all tile meshes in children and stores them to tileMeshObjects
    void Start(){
        GetTileMesh();
    }
    void Awake(){
        GetTileMesh();
    }
    private void GetTileMesh(){
        Transform[] tileObjects = GetComponentsInChildren<Transform>(true);
        if(tileObjects.Length <= 0){
            Debug.LogError("\"No MeshRenderer attached to this tile!\" - Tile Mesh Loader");
        }

        tileChildrenObjects = new GameObject[tileObjects.Length];
        for(int i = 0; i < tileChildrenObjects.Length; i++){
            if(tileObjects[i] != null && tileObjects[i].gameObject != this.gameObject){
                tileChildrenObjects[i] = tileObjects[i].gameObject;
            }
        }
    }


    //Activates tile mesh when chunk is loaded
    public void LoadTileMesh(){

        Debug.Log("isLoading tile mesh");

        //If it's already loaded, it doesn't try to load it again
        //if(isLoaded) return;


        //Turns on all the child objects that were activated when the tile was unloaded
        foreach(GameObject activatedChildObject in activatedChildrenObjects){
            if(activatedChildObject != null){
                activatedChildObject.SetActive(true);
            }
                
        }

        isLoaded = true;
        
    }

    //Deactivates tile mesh when chunk is unloaded
    public void UnloadTileMesh(){

       

        //If it's already unloaded, it doesn't try to unload it again
        if(!isLoaded) return;

         Debug.Log("Unloading tile mesh");

        isLoaded = false;

        activatedChildrenObjects.Clear();

        GetTileMesh();

        //Goes through each activated child object, adds it to the list, and deactivates it
        foreach(GameObject tileChildObject in tileChildrenObjects){

            if(tileChildObject != null && tileChildObject.activeInHierarchy){
                activatedChildrenObjects.Add(tileChildObject);
                tileChildObject.SetActive(false);
            }
                
        }
        
    }

}
