using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GridManager : MonoBehaviour
{

    public List<GridChunk> gridChunks = new List<GridChunk>();

    public GridCell[][] positionsOfCells;

    public Vector3 gridManagerCenter = new Vector3(0f, 0f, 0f);

    public static GridManager GM;

    private int xLengthOfGrid = 100;
    private int yLengthOfGrid = 100;

    private int ActiveGridChunkIndex{
        get{
            if(GroundAreaExpansion.GAE != null){
                return GroundAreaExpansion.GAE.ActiveGroundChunk;
            } else{
                return 0;
            }
            
        }
    }

    private GridChunk ActiveGridChunk{
        get{
            if(ActiveGridChunkIndex < gridChunks.Count){
               return gridChunks[ActiveGridChunkIndex]; 
            } else{
                return null;
            }
            
        }
    }

    private List<Tile> allTilesOnActiveChunk = new List<Tile>();

    public bool AtLeastOneTileIsOnChunk() {
        return ActiveGridChunk.AtLeastOneTileIsOnChunk();
    }

    #region Unity Functions

    void Awake(){

        if(GM == null){
           GM = this; 
        }else{
            //Destroy(this);
        }
    }


    void Start(){

        // //Declares new 100 by 100 fragmented array of grid cells
        // //Sets up the Grid Manager
        // positionsOfCells = new GridCell[xLengthOfGrid][];
        // for(int i = 0; i < xLengthOfGrid; i++)
        // {
        //     positionsOfCells[i] = new GridCell[yLengthOfGrid];
        //     for (int q = 0; q < yLengthOfGrid; q++) {
        //         positionsOfCells[i][q] = new GridCell();
        //         positionsOfCells[i][q].xArrayLocation = i; //i - (xLengthOfGrid / 2) + .5f
        //         positionsOfCells[i][q].yArrayLocation = q; //q - (yLengthOfGrid / 2) + .5f
        //     }
        // }

        // GameEventManager.current.SwitchedCurrentGroundChunk.AddListener(SwitchedGridChunks);

        //Adds new Grid Chunk
        gridChunks.Add(new GridChunk(new Vector3( 0f, 0f, 0f)));

    }

    // public void SetGridCellPositions(){
    //     for(int i = 0; i < xLengthOfGrid; i++)
    //     {
    //         for (int q = 0; q < yLengthOfGrid; q++) {
    //             Vector2Int gridPosition = SwitchToGridCoordinates(new Vector3((float) i, 0f, (float) q));
    //             //Debug.Log("Converted " + q + ", " + i + " to " + gridPosition.x + ", " + gridPosition.y);
    //             positionsOfCells[i][q].xArrayLocation = gridPosition.x;//i - (xLengthOfGrid / 2) + .5f;
    //             positionsOfCells[i][q].yArrayLocation = gridPosition.y;//q - (yLengthOfGrid / 2) + .5f;
    //         }
    //     }
    // }



    #endregion


    #region Get Functions
    //Only used by grid visualizer utility
    public GridCell[] GetAllGridCells(){
        return ActiveGridChunk.GetAllGridCells();
    }

    public Tile[] GetAllTilesInRange(Vector2Int bottomLeftGridPoint, Vector2Int topRightGridPoint){
        return ActiveGridChunk.GetAllTilesInRange(bottomLeftGridPoint, topRightGridPoint);
    }

    public Tile[] GetAllTilesOnActiveChunk(){
        return ActiveGridChunk.allTilesOnChunk.ToArray();
    }

    //returns all neighbors of input tile
    public GameObject[] GetTileNeighbors(Vector3 tilePosition, int[] neighborsToReturn){
        return ActiveGridChunk.GetTileNeighbors(tilePosition, neighborsToReturn);        
    }
    

    //returns all the objects sitting in a cell
    public GameObject[] GetGameObjectsInGridCell(GridCell currentGridCell){
        return ActiveGridChunk.GetGameObjectsInGridCell(currentGridCell);
    }

    //Overload methods for GetGameObjectsInGridCell
    public GameObject[] GetGameObjectsInGridCell(Vector2Int gridPosition){
        return ActiveGridChunk.GetGameObjectsInGridCell(gridPosition);
    }
    public GameObject[] GetGameObjectsInGridCell(Vector3 worldPosition){
        return ActiveGridChunk.GetGameObjectsInGridCell(worldPosition);
    }
    public GameObject[] GetGameObjectsInGridCell(GameObject gameObjectInGridCell){
        return ActiveGridChunk.GetGameObjectsInGridCell(gameObjectInGridCell);
    }


    //returns the Grid Cell Object for a given point
    public GridCell GetGridCell(Vector2Int gridPosition)
    {
        return ActiveGridChunk.GetGridCell(gridPosition);
    }

    //Checks if the grid cell at the given point contains a terrain tile
    // public bool TileIsOverGround(Vector3 tilePosition){
    //     return ActiveGridChunk.TileIsOverGround(tilePosition);
    // }

    #endregion

    #region Edit GridCells

    //TODO: remove isloading parameter
    public void AddObject(GameObject objectToAdd, bool isLoading)
    {
        ActiveGridChunk.AddObject(objectToAdd);


    }

    //TODO: remove isunloading parameter
    public void RemoveObject(GameObject objectToRemove, bool isUnloading)
    {
        ActiveGridChunk.RemoveObject(objectToRemove);
    }


    #endregion

    #region Chunk Functions

    public void AddNewChunk(Vector3 newChunkCenter){
        gridChunks.Add(new GridChunk(newChunkCenter));
    }

    // private void SwitchedGridChunks(){
    //     ReCalculateAllTilesOnActiveChunk();
    // }

    // public void ReCalculateAllTilesOnActiveChunk(){

        
    //     //Gets all the tiles on top of the active grid chunk
    //     int halfOfGridChunkSize = GridDataLoader.current.gridChunkSize / 2;
    //     Vector2Int bottomLeftGridPoint = new Vector2Int(-halfOfGridChunkSize, -halfOfGridChunkSize);
    //     Vector2Int topRightGridPoint = new Vector2Int(halfOfGridChunkSize, halfOfGridChunkSize);

    //     //Adds all tiles on the active chunk to the list
    //     allTilesOnActiveChunk.Clear();
    //     allTilesOnActiveChunk.AddRange(GridManager.GM.GetAllTilesInRange(bottomLeftGridPoint, topRightGridPoint));

    //     //Clears tile counters and recalculates them
    //     TileTypeCounter.current.ClearTileTrackers();
    //     foreach(Tile tile in allTilesOnActiveChunk){
    //         TileTypeCounter.current.CheckTileTrackersForAddition(tile.gameObject);
    //     }
    // }



    #endregion

    #region Set Functions

    public void AddTileToActiveChunk(Tile tile){
        ActiveGridChunk.allTilesOnChunk.Add(tile);
    }

    public void RemoveTileFromActiveChunk(Tile tile){
        ActiveGridChunk.allTilesOnChunk.Remove(tile);
    }

    #endregion

    #region Switch Coords


    public Vector2Int SwitchToGridCoordinates(Vector3 worldCoordinates) {
        return ActiveGridChunk.SwitchToGridCoordinates(worldCoordinates);
        // Vector3 adjustedWorldCoordinates = worldCoordinates - gridManagerCenter;
        // Vector3 gridCoordinates = BuildingSystem.current.SnapCoordinateToGrid(adjustedWorldCoordinates);
        // return new Vector2Int((int)(gridCoordinates.x - .5f), (int)(gridCoordinates.z - .5f));
    }

    public Vector3 SwitchFromArrayToWorldCoordinates(Vector2Int arrayCoordinates) {
        return ActiveGridChunk.SwitchFromArrayToWorldCoordinates(arrayCoordinates);
        // int gridPosX = arrayCoordinates.x - (xLengthOfGrid / 2);
        // int gridPosY = arrayCoordinates.y - (yLengthOfGrid / 2);

        // Vector3 AsWorldCoordinates = new Vector3((gridPosX - .5f), 0f, gridPosY - .5f);
        // Vector3 adjustedWorldCoordinates = AsWorldCoordinates + gridManagerCenter;
        // return adjustedWorldCoordinates;
    }


    public Vector3 SwitchFromGridToWorldCoordinates(Vector2Int gridCoordinates) {
        return ActiveGridChunk.SwitchFromGridToWorldCoordinates(gridCoordinates);

        // Vector3 AsWorldCoordinates = new Vector3((gridCoordinates.x - .5f), 0f, gridCoordinates.y - .5f);
        // Vector3 adjustedWorldCoordinates = AsWorldCoordinates + gridManagerCenter;
        // return adjustedWorldCoordinates;
    }

    public Vector3 AdjustCoordinatesByGridCenter(Vector2 gridCoordinates) {
        return ActiveGridChunk.AdjustCoordinatesByGridCenter(gridCoordinates);

        // Vector3 adjustedWorldCoordinates = new Vector3(gridCoordinates.x, 0f, gridCoordinates.y) + gridManagerCenter;
        // return adjustedWorldCoordinates;
    }


    //Switches to array from grid coordinates
    private Vector2Int SwitchFromGridToArrayCoordinates(Vector2Int gridCoordinates){
        //return ActiveGridChunk.SwitchFromGridToArrayCoordinates(gridCoordinates);

        int posX = gridCoordinates.x + (xLengthOfGrid / 2);
        int posY = gridCoordinates.y + (yLengthOfGrid / 2);
        return new Vector2Int(posX, posY);
    }


    public void SwitchCenter(Vector3 newWorldCenter){
        gridManagerCenter = newWorldCenter;
    }
    
    public Vector3 GetCenter(){
        if(ActiveGridChunk != null){
            return ActiveGridChunk.chunkWorldCenter;
        } else{
            return new Vector3( 0f, 0f, 0f);
        }
        
    }

    #endregion


}





