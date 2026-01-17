/*
*   GridLoader: stores and loads the tiles held in the GridManager into 
*
*/
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

public class GridDataLoader : MonoBehaviour
{
    public static GridDataLoader current;

    public int currentGridChunk = 0;
    public int gridChunkSize = 10;
    public List<GridChunkData> gridChunks;
    public GameObject[] tilePrefabs;
    private TileScriptableObject[] tileScriptables;

    public GridChunkData currentGridChunkData{
        get{
            return gridChunks[currentGridChunk];
        }
    }

    void Awake(){
        if(current == null){
            current = this;
        }
    }

    void Start(){
        gridChunks = new List<GridChunkData>();

        //Save the tile scriptable objects from the tile prefabs
        tileScriptables = new TileScriptableObject[tilePrefabs.Length];
        for(int i = 0; i < tilePrefabs.Length; i++){
            if(tilePrefabs[i].GetComponent<Tile>() != null){
                tileScriptables[i] = tilePrefabs[i].GetComponent<Tile>().tileScriptableObject;
            }
        }
    }


    //Switches which grid chunk is currently loaded
    public void SwitchToGridChunk(int newGridChunk, Vector3 worldCenterOfChunk, float timeToWaitBeforeDestroyingOldChunk){
        //Stores all the tiles on the active grid chunk
        SaveActiveGridChunk();

        //Removes all the current tiles from the grid manager
        UnloadGridChunkFromGridManager(currentGridChunk);

        //Switches the world center of the grid manager
        GridManager.GM.SwitchCenter(worldCenterOfChunk);

        //Loads the tiles on the new grid chunk into place
        LoadGridChunk(newGridChunk);

        //Delays visually destroying the tiles on the chunk until it is out of view.
        StartCoroutine(WaitToUnloadChunkVisually(currentGridChunk, timeToWaitBeforeDestroyingOldChunk));

        //Updates the current GridChunk
        currentGridChunk = newGridChunk;



    }

    public IEnumerator WaitToUnloadChunkVisually(int chunkToUnload, float timeToSwitchChunks){
        yield return new WaitForSeconds(timeToSwitchChunks);
        UnLoadGridChunkVisually(chunkToUnload);
    }

    public void SaveActiveGridChunk(){


        //Gets all the tiles on top of the active grid chunk
        // int halfOfGridChunkSize = gridChunkSize / 2;
        // Vector2Int bottomLeftGridPoint = new Vector2Int(-halfOfGridChunkSize, -halfOfGridChunkSize);
        // Vector2Int topRightGridPoint = new Vector2Int(halfOfGridChunkSize, halfOfGridChunkSize);
        // Tile[] allTileScripts = GridManager.GM.GetAllTilesInRange(bottomLeftGridPoint, topRightGridPoint);
        Tile[] allTileScripts = GridManager.GM.GetAllTilesOnActiveChunk();


        GameObject[] allTiles = new GameObject[allTileScripts.Length];
        for(int i = 0; i < allTileScripts.Length; i++){
            allTiles[i] = allTileScripts[i].gameObject;
        }


        Vector3[] allTilePositions = new Vector3[allTiles.Length];
        GameObject[] allTilePrefabs = new GameObject[allTiles.Length];
        GameObject[] allTileObjects = new GameObject[allTiles.Length];
        bool[] activatedTiles = new bool[allTiles.Length];

        for(int i = 0; i < allTiles.Length; i++){

            //Saves the tile object to the chunk, so it can be destroyed as necessary
            allTileObjects[i] = allTiles[i];

            //saves position of tile
            allTilePositions[i] = allTiles[i].transform.position;

            
            Tile tile = allTiles[i].GetComponent<Tile>();

            //saved the activation state of the tile
            if(tile is ActivatableTile activatableTile && activatableTile.IsActivated){
                activatedTiles[i] = true;
            }else{
                activatedTiles[i] = false;
            }

            //Saves prefab of tile
            if(tile != null){
 
                if(tileScriptables.Contains(tile.tileScriptableObject)){
                    allTilePrefabs[i] = tilePrefabs[System.Array.IndexOf(tileScriptables, tile.tileScriptableObject)];
                } else{
                    Debug.LogError("Missing the tile prefab: \"" +tile.tileScriptableObject.Name + "\" from the Grid Data Loader. Drag the missing Prefab in from the folder Assets/Prefabs/CurrentTiles to the array on this script called tilePrefabs.");
                }
            }
        }

        gridChunks[currentGridChunk].SetChunkData(allTilePositions, allTilePrefabs, allTileObjects, activatedTiles);
    }

    public void CreateNewGridChunk(){
        gridChunks.Add(new GridChunkData());

    }


    //Must update Grid Manager with new object references
    //Must tell objects they're placed
    public void LoadGridChunk(int gridChunkNum){
        GameObject[] tilePrefabs = gridChunks[gridChunkNum].PrefabsOfTiles;
        
        Vector3[] tilePositions = gridChunks[gridChunkNum].PositionsOfTiles;

        bool[] tileIsActivated = gridChunks[gridChunkNum].activatedTiles;

        GameObject[] instantiatedTiles = new GameObject[tilePrefabs.Length];
        //Instantiates each tile stored in the Grid Chunk
        for(int i = 0; i < tilePrefabs.Length; i++){
            instantiatedTiles[i] = Instantiate(tilePrefabs[i], tilePositions[i], tilePrefabs[i].transform.rotation);
        }
        for(int i = 0; i < instantiatedTiles.Length; i++){
            GameObject instantiatedTile = instantiatedTiles[i];
            GridManager.GM.AddObject(instantiatedTile, true);
            instantiatedTile.GetComponent<ObjectDrag>().LoadedTile();
            // RoadConnections roadConnections = instantiatedTile.GetComponent<RoadConnections>();
            // if(roadConnections != null){
            //     roadConnections.UpdateModelConnections(false);
            // }

            //Sets tile activation
            ActivatableTile activatableTile = instantiatedTile.GetComponent<ActivatableTile>();
            if(activatableTile != null){
                if(tileIsActivated[i]){
                    activatableTile.LoadActivatedBuilding();
                }else{
                    activatableTile.LoadDeactivatedBuilding();
                }
            }

            //Recalculates residence connections. -- Causing lag
            //RoadAndResidenceConnectionManager.current.LoadResidenceConnections(instantiatedTile);
        }
    }

    public void UnLoadGridChunkVisually(int gridChunkNum){
        GameObject[] tilesOnChunk = gridChunks[gridChunkNum].TileObjects;
        foreach(GameObject tile in tilesOnChunk){
            if(tile != null){
                Destroy(tile);
            }
        }
    }

    public void UnloadGridChunkFromGridManager(int gridChunkNum){
        GameObject[] tilesOnChunk = gridChunks[gridChunkNum].TileObjects;
        foreach(GameObject tile in tilesOnChunk){
            if(tile != null){
                GridManager.GM.RemoveObject(tile, true);
            }
        }
    }

}

public class GridChunkData{

    public GridCell[][] GridCells {get; set;}

    public Vector3[] PositionsOfTiles = new Vector3[0];
    public GameObject[] PrefabsOfTiles = new GameObject[0];

    public GameObject[] TileObjects = new GameObject[0];

    public bool[] activatedTiles = new bool[0];


    public void SetChunkData(Vector3[] _positionsOfTiles, GameObject[] _prefabsOfTiles, GameObject[] _tileObjects, bool[] _activatedTiles){
        PositionsOfTiles = _positionsOfTiles;
        PrefabsOfTiles = _prefabsOfTiles;
        TileObjects = _tileObjects;
        activatedTiles = _activatedTiles;


    }

    // public GridChunkData(GridCell[][] _gridCells){
    //     GridCells = _gridCells
    // }

    public void FillWorldPositions(Vector3[] positions){
        PositionsOfTiles = positions;
    }
}
