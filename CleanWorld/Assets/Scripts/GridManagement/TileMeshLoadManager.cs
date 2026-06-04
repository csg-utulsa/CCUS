using UnityEngine;

public class TileMeshLoadManager : MonoBehaviour
{

    public static TileMeshLoadManager current;
    
    void Awake(){
        if(current == null){
            current = this;
        } else{
            Destroy(this);
        }

    }


    // Unloads the meshes for all tiles on a given chunk
    public void UnloadGridChunk(int gridChunkIndex){

        // Gets array of all the tiles on the grid chunk
        Tile[] allTilesOnGridChunk = GridManager.GM.gridChunks[gridChunkIndex].GetAllTilesOnGridChunk();

        // Go through all the tiles and tell them to turn off their meshes
        foreach(Tile tile in allTilesOnGridChunk){
            TileMeshLoader tileMeshLoader = tile.GetComponent<TileMeshLoader>();
            if(tileMeshLoader != null){
                tileMeshLoader.UnloadTileMesh();
            }
        }

    }

    // Loads the meshes for all tiles on a given chunk
    public void LoadGridChunk(int gridChunkIndex){

        // Gets array of all the tiles on the grid chunk
        Tile[] allTilesOnGridChunk = GridManager.GM.gridChunks[gridChunkIndex].GetAllTilesOnGridChunk();

        // Go through all the tiles and tell them to turn off their meshes
        foreach(Tile tile in allTilesOnGridChunk){
            TileMeshLoader tileMeshLoader = tile.GetComponent<TileMeshLoader>();
            if(tileMeshLoader != null){
                tileMeshLoader.LoadTileMesh();
            }
        }

    }



}
