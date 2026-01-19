using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GridManager : MonoBehaviour
{

    public GridCell[][] positionsOfCells;

    public Vector3 gridManagerCenter = new Vector3(0f, 0f, 0f);

    public static GridManager GM;

    private int xLengthOfGrid = 100;
    private int yLengthOfGrid = 100;

    private List<Tile> allTilesOnActiveChunk = new List<Tile>();

    #region Unity Functions

    void Awake(){

        if(GM == null){
           GM = this; 
        }else{
            //Destroy(this);
        }
    }


    void Start(){

        //Declares new 100 by 100 fragmented array of grid cells
        //Sets up the Grid Manager
        positionsOfCells = new GridCell[xLengthOfGrid][];
        for(int i = 0; i < xLengthOfGrid; i++)
        {
            positionsOfCells[i] = new GridCell[yLengthOfGrid];
            for (int q = 0; q < yLengthOfGrid; q++) {
                positionsOfCells[i][q] = new GridCell();
                positionsOfCells[i][q].xArrayLocation = i; //i - (xLengthOfGrid / 2) + .5f
                positionsOfCells[i][q].yArrayLocation = q; //q - (yLengthOfGrid / 2) + .5f
            }
        }

        GameEventManager.current.SwitchedCurrentGroundChunk.AddListener(SwitchedGridChunks);

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

    

    void Update(){
        //Debug.Log("Grid center is " + gridManagerCenter);
    }

    #endregion


    #region Get Functions
    //Only used by grid visualizer utility
    public GridCell[] GetAllGridCells(){
        GridCell[] allGridCells = new GridCell[xLengthOfGrid * yLengthOfGrid];
        for(int i = 0; i < xLengthOfGrid; i++){
            for(int j = 0; j < yLengthOfGrid; j++){
                allGridCells[(xLengthOfGrid * i) + j] = positionsOfCells[i][j];
            }
        }
        return allGridCells;
    }

    public Tile[] GetAllTilesInRange(Vector2Int bottomLeftGridPoint, Vector2Int topRightGridPoint){
        List<Tile> allTiles = new List<Tile>();


        //Gets all grid cells in the given range
        int widthOfSelection = topRightGridPoint.x - bottomLeftGridPoint.x;
        int heightOfSelection = topRightGridPoint.y - bottomLeftGridPoint.y;
        GridCell[] allGridCells = new GridCell[widthOfSelection * heightOfSelection];
        for(int i = bottomLeftGridPoint.x; i < topRightGridPoint.y; i++){
            for(int j = bottomLeftGridPoint.y; j < topRightGridPoint.y; j++){
                int adjustedXArrayCoordinate = (widthOfSelection/2) + i;
                int adjustedYArrayCoordinate = (heightOfSelection/2) + j;

                

                allGridCells[(widthOfSelection * adjustedXArrayCoordinate) + adjustedYArrayCoordinate] = GetGridCell(new Vector2Int(i, j));
            }
        }

        //Gets all the Tiles on each cell
        foreach(GridCell gridCell in allGridCells){
            GameObject[] allObjectsInCell = GetGameObjectsInGridCell(gridCell);
            foreach(GameObject objectInCell in allObjectsInCell){
                Tile tile = objectInCell.GetComponent<Tile>();
                if(tile != null){
                    allTiles.Add(tile);
                }
            }
        }

        return allTiles.ToArray();

    }



    //returns all neighbors of input tile
    public GameObject[] GetTileNeighbors(Vector3 tilePosition, int[] neighborsToReturn){

        BuildingSystem currentBuildingSystem = BuildingSystem.current;
        Vector3Int tileCell = currentBuildingSystem.gridLayout.WorldToCell(tilePosition);
        List<GameObject> tileNeighbors = new List<GameObject>();

        Vector3Int[] directions = new Vector3Int[]
        {
            new Vector3Int(0, 1, 0),  // North
            new Vector3Int(1, 1, 0),  // North East
            new Vector3Int(1, 0, 0),  // East
            new Vector3Int(1, -1, 0), // South East
            new Vector3Int(0, -1, 0), // South
            new Vector3Int(-1, -1, 0), // South West
            new Vector3Int(-1, 0, 0),  // West
            new Vector3Int(-1, 1, 0),  // North West
        };

        for (int i = 0; i < neighborsToReturn.Length; i++)
        {
            Vector3Int checkCell = tileCell + directions[neighborsToReturn[i]];
            Vector3 checkWorldPos = currentBuildingSystem.grid.GetCellCenterWorld(checkCell);

            foreach (GameObject obj in GetGameObjectsInGridCell(checkWorldPos))
            {
                if(obj.GetComponent<Tile>() != null){
                    tileNeighbors.Add(obj);
                }
            }
        }

        GameObject[] returnArray = new GameObject[tileNeighbors.Count];
        for(int i = 0; i < returnArray.Length; i++){
            returnArray[i] = tileNeighbors[i];
        }
        return returnArray;
    }
    
    // public GameObject[] GetAllGridObjects(){
    //     GameObject[] allGridObjectsArray = new GameObject[allGridObjects.Count];
    //     for(int i = 0; i < allGridObjects.Count; i++){
    //         allGridObjectsArray[i] = allGridObjects[i];
    //     }
    //     return allGridObjectsArray;
    // }

    //returns all the objects sitting in a cell
    public GameObject[] GetGameObjectsInGridCell(GridCell currentGridCell){
        GameObject[] allObjectsInCell = currentGridCell.GetObjectsInCell();
        int numberOfObjectsInCell = 0;
        foreach(GameObject objectInCell in allObjectsInCell){
            if(objectInCell != null)
                numberOfObjectsInCell++;
        }
        GameObject[] notNullObjectsInCell = new GameObject[numberOfObjectsInCell];
        for(int i = 0; i < numberOfObjectsInCell; i++){
            notNullObjectsInCell[i] = allObjectsInCell[i];
        }
        return notNullObjectsInCell;
    }


    public GameObject[] GetGameObjectsInGridCell(Vector2Int gridPosition){
        GridCell currentGridCell = GetGridCell(gridPosition);
        return GetGameObjectsInGridCell(currentGridCell);
    }

    public GameObject[] GetGameObjectsInGridCell(Vector3 worldPosition){
        Vector2Int gridPosition = SwitchToGridCoordinates(worldPosition);
        return GetGameObjectsInGridCell(gridPosition);
    }

    public GameObject[] GetGameObjectsInGridCell(GameObject gameObjectInGridCell){
        return GetGameObjectsInGridCell(gameObjectInGridCell.transform.position);
    }

    //returns the Grid Cell Object for a given point
    public GridCell GetGridCell(Vector2Int gridPosition)
    {
        Vector2Int arrayPosition = SwitchFromGridToArrayCoordinates(gridPosition);
        return positionsOfCells[arrayPosition.x][arrayPosition.y];
        //return positionsOfCells[x + (xLengthOfGrid / 2)][z + (yLengthOfGrid / 2) ];
    }

    #endregion

    #region Edit GridCells

    public void AddObject(GameObject objectToAdd, bool isLoading)
    {
        Vector2Int gridPosition = SwitchToGridCoordinates(objectToAdd.transform.position);
        Vector2Int arrayPosition = SwitchFromGridToArrayCoordinates(gridPosition);


        GetGridCell(gridPosition).AddObject(objectToAdd, arrayPosition.x, arrayPosition.y, isLoading);


    }


    public void RemoveObject(GameObject objectToRemove, bool isUnloading)
    {
        Vector2Int positionInGrid = SwitchToGridCoordinates(objectToRemove.transform.position);
        Vector2Int arrayPosition = SwitchFromGridToArrayCoordinates(positionInGrid);

        positionsOfCells[arrayPosition.x][arrayPosition.y].RemoveObject(objectToRemove, isUnloading);
    }


    #endregion

    #region Chunk Functions

    private void SwitchedGridChunks(){
        ReCalculateAllTilesOnActiveChunk();
    }

    public void ReCalculateAllTilesOnActiveChunk(){

        
        //Gets all the tiles on top of the active grid chunk
        int halfOfGridChunkSize = GridDataLoader.current.gridChunkSize / 2;
        Vector2Int bottomLeftGridPoint = new Vector2Int(-halfOfGridChunkSize, -halfOfGridChunkSize);
        Vector2Int topRightGridPoint = new Vector2Int(halfOfGridChunkSize, halfOfGridChunkSize);

        //Adds all tiles on the active chunk to the list
        allTilesOnActiveChunk.Clear();
        allTilesOnActiveChunk.AddRange(GridManager.GM.GetAllTilesInRange(bottomLeftGridPoint, topRightGridPoint));

        //Clears tile counters and recalculates them
        TileTypeCounter.current.ClearTileTrackers();
        foreach(Tile tile in allTilesOnActiveChunk){
            TileTypeCounter.current.CheckTileTrackersForAddition(tile.gameObject);
        }
    }

    public Tile[] GetAllTilesOnActiveChunk(){
        return allTilesOnActiveChunk.ToArray();
    }

    #endregion

    #region Set Functions

    public void AddTileToActiveChunk(Tile tile){
        allTilesOnActiveChunk.Add(tile);
    }

    public void RemoveTileFromActiveChunk(Tile tile){
        allTilesOnActiveChunk.Remove(tile);
    }

    #endregion

    #region Switch Coords


    public Vector2Int SwitchToGridCoordinates(Vector3 worldCoordinates) {
        Vector3 adjustedWorldCoordinates = worldCoordinates - gridManagerCenter;
        Vector3 gridCoordinates = BuildingSystem.current.SnapCoordinateToGrid(adjustedWorldCoordinates);
        //Debug.Log("Snapped given coordinates: " + gridCoordinates.x + ", " + gridCoordinates.z);
        return new Vector2Int((int)(gridCoordinates.x - .5f), (int)(gridCoordinates.z - .5f));
    }

    public Vector3 SwitchFromArrayToWorldCoordinates(Vector2Int arrayCoordinates) {
        int gridPosX = arrayCoordinates.x - (xLengthOfGrid / 2);
        int gridPosY = arrayCoordinates.y - (yLengthOfGrid / 2);

        Vector3 AsWorldCoordinates = new Vector3((gridPosX - .5f), 0f, gridPosY - .5f);
        Vector3 adjustedWorldCoordinates = AsWorldCoordinates + gridManagerCenter;
        return adjustedWorldCoordinates;
    }

    public Vector3 SwitchFromGridToWorldCoordinates(Vector2Int gridCoordinates) {

        Vector3 AsWorldCoordinates = new Vector3((gridCoordinates.x - .5f), 0f, gridCoordinates.y - .5f);
        Vector3 adjustedWorldCoordinates = AsWorldCoordinates + gridManagerCenter;
        return adjustedWorldCoordinates;
    }

    //Switches to array from grid coordinates
    private Vector2Int SwitchFromGridToArrayCoordinates(Vector2Int gridCoordinates){
        int posX = gridCoordinates.x + (xLengthOfGrid / 2);
        int posY = gridCoordinates.y + (yLengthOfGrid / 2);
        return new Vector2Int(posX, posY);
    }

    public void SwitchCenter(Vector3 newWorldCenter){
        gridManagerCenter = newWorldCenter;
    }
    
    public Vector3 GetCenter(){
        return gridManagerCenter;
    }

    #endregion


}


#region GridCell Class

//Container to hold everything a single Cell on the grid could need to know
public class GridCell 
{

    GameObject[] objectsInCell = new GameObject[5];
    public float xArrayLocation { get; set; }
    public float yArrayLocation { get; set; }


    public int numberOfObjectsInCell = 0;
    public bool isOverGround = false;

    //returns what objects are sitting on the cell
    public GameObject[] GetObjectsInCell(){
        return objectsInCell;
    }

    //Adds an object to the array of objects that are sitting on the cell
    public void AddObject(GameObject objectToAdd, int x, int y, bool isLoading) {
        if (objectsInCell.Length > numberOfObjectsInCell)
        {
            
            if(!isLoading && objectToAdd.GetComponent<Tile>() != null && objectToAdd.GetComponent<Tile>().tileScriptableObject != null){

                //Keeps list of tiles on active chunk
                GridManager.GM.AddTileToActiveChunk(objectToAdd.GetComponent<Tile>());

                //Tracks different types of tiles
                TileTypeCounter.current.CheckTileTrackersForAddition(objectToAdd);
               

            }

            objectsInCell[numberOfObjectsInCell] = objectToAdd;
            
            Tile tile = objectToAdd.GetComponent<Tile>();
            if(tile != null)
                tile.gridCell = this;

            numberOfObjectsInCell++;
        }
    }


    //Removes an object from the array of objects that are sitting on the cell
    public void RemoveObject(GameObject objectToRemove, bool isUnloading) {
        for(int i = 0; i < objectsInCell.Length; i++) {
            if(objectToRemove == objectsInCell[i])
            {
                if(!isUnloading && objectToRemove.GetComponent<Tile>() != null && objectToRemove.GetComponent<Tile>().tileScriptableObject != null){
                    //Keeps list of tiles on active chunk
                    GridManager.GM.RemoveTileFromActiveChunk(objectToRemove.GetComponent<Tile>());
                    //Tracks different types of tiles
                    TileTypeCounter.current.CheckTileTrackersForRemoval(objectToRemove);
                }
                objectsInCell[i] = null;
                for(int b=i; b < objectsInCell.Length-1; b++) {
                    objectsInCell[b] = objectsInCell[b + 1];
                }
                objectsInCell[objectsInCell.Length-1] = null;
                numberOfObjectsInCell--;
            }
        }
    }


}

#endregion


