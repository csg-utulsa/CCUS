using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GridManager : MonoBehaviour
{
    public GridCell[][] positionsOfCells;

    public static GridManager GM;

    private List<GameObject> allGridObjects = new List<GameObject>();

    public List<Tile> moneyProducingTiles = new List<Tile>();
    public List<Tile> carbonProducingTiles = new List<Tile>();


    public Tile[] GetMoneyProducingTiles(){
        Tile[] returnArray = new Tile[moneyProducingTiles.Count];
        for(int i = 0; i < moneyProducingTiles.Count; i++){
            returnArray[i] = moneyProducingTiles[i];
        }
        return returnArray;
    }

    public Tile[] GetCarbonProducingTiles(){
        Tile[] returnArray = new Tile[carbonProducingTiles.Count];
        for(int i = 0; i < carbonProducingTiles.Count; i++){
            returnArray[i] = carbonProducingTiles[i];
        }
        return returnArray;
    }

    public void AddToMoneyTileList(Tile tileToAdd){
        moneyProducingTiles.Add(tileToAdd);
    }    

    public void AddToCarbonTileList(Tile tileToAdd){
        carbonProducingTiles.Add(tileToAdd);
    }    

    public void RemoveFromMoneyTileList(Tile tileToAdd){
        moneyProducingTiles.Remove(tileToAdd);
    }    

    public void RemoveFromCarbonTileList(Tile tileToAdd){
        carbonProducingTiles.Remove(tileToAdd);
    }    

    public void AddGridObjectToList(GameObject objectToAdd){
        allGridObjects.Add(objectToAdd);
    }

    public void RemoveGridObjectFromList(GameObject objectToRemove){
        allGridObjects.Remove(objectToRemove);
    }


    
    void Awake(){
        GM = this;

        //Declares new 100 by 100 fragmented array of grid cells
        positionsOfCells = new GridCell[100][];
        for(int i = 0; i < 100; i++)
        {
            positionsOfCells[i] = new GridCell[100];
            for (int q = 0; q < 100; q++) {
                positionsOfCells[i][q] = new GridCell();
            }
        }


    }
    
    void Start(){
        GM = this;

        //Test
        // int[] testArray = { 1, 2, 3, 4, 5, 6, 7, 0, 0, 0, 0, 0, 0};
        // for(int i = 0; i < testArray.Length; i++) {
        //     if(3 == testArray[i])
        //     {
        //         testArray[i] = 0;
        //         for(int b=i; b < testArray.Length-1; b++) {
        //             testArray[b] = testArray[b + 1];
        //         }
        //         testArray[testArray.Length-1] = 0;
        //     }
        // }
        // Debug.Log("This is the test: ");
        // for(int i = 0; i < testArray.Length; i++){
        //     Debug.Log(testArray[i]);
        // }

        // for(int i = 0; i < testArray.Length; i++) {
        //     if(6 == testArray[i])
        //     {
        //         testArray[i] = 0;
        //         for(int b=i; b < testArray.Length-1; b++) {
        //             testArray[b] = testArray[b + 1];
        //         }
        //         testArray[testArray.Length-1] = 0;
        //     }
        // }
        // Debug.Log("This is the test: ");
        // for(int i = 0; i < testArray.Length; i++){
        //     Debug.Log(testArray[i]);
        // }
        
        
        
    }

    public GameObject[] GetRoadNeighbors(GameObject _tile){
        return GetRoadNeighbors(_tile.transform.position);
    }

    //returns all neighbors of input tile
    public GameObject[] GetRoadNeighbors(Vector3 tileLocation){

        BuildingSystem currentBuildingSystem = BuildingSystem.current;
        Vector3Int tileCell = currentBuildingSystem.gridLayout.WorldToCell(tileLocation);
        GameObject[] tileNeighbors = new GameObject[4];

        Vector3Int[] directions = new Vector3Int[]
        {
            new Vector3Int(0, 1, 0),  // North
            new Vector3Int(1, 0, 0),  // East
            new Vector3Int(0, -1, 0), // South
            new Vector3Int(-1, 0, 0)  // West
        };

        for (int i = 0; i < directions.Length; i++)
        {
            Vector3Int checkCell = tileCell + directions[i];
            Vector3 checkWorldPos = currentBuildingSystem.grid.GetCellCenterWorld(checkCell);

            foreach (GameObject obj in GridManager.GM.GetGameObjectsInGridCell(checkWorldPos))
            {
                if(obj.GetComponent<RoadConnections>() != null){
                    tileNeighbors[i] = obj;
                }
                // if (obj != placedTile)
                // {
                //     ConnectedTileHandler neighborHandler = obj.GetComponent<ConnectedTileHandler>();
                //     if (neighborHandler != null)
                //     {
                //         neighborHandler.UpdateModel();
                //     }
                // }
            }
        }
        return tileNeighbors;
    }
    
    public GameObject[] GetAllGridObjects(){
        GameObject[] allGridObjectsArray = new GameObject[allGridObjects.Count];
        for(int i = 0; i < allGridObjects.Count; i++){
            allGridObjectsArray[i] = allGridObjects[i];
        }
        return allGridObjectsArray;
    }

    //returns all the objects sitting in a cell
    public GameObject[] GetGameObjectsInGridCell(int x, int z){
        
        GridCell currentGridCell = GetGridCell(x, z);
        
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

    public GameObject[] GetGameObjectsInGridCell(Vector3 worldPositionOfGridCell){
        Vector3 gridPositionOfGridcell = switchToGridCoordinates(worldPositionOfGridCell);
        //Debug.Log("Well this ran");
        //Debug.Log("Positions: " + gridPositionOfGridcell.x + ", " + gridPositionOfGridcell.z);
        //GridCell myGridCell = GetGridCell((int)gridPositionOfGridcell.x, (int)gridPositionOfGridcell.z);
        //return myGridCell.GetObjectsInCell();
        //return GetGridCell((int)gridPositionOfGridcell.x , (int)gridPositionOfGridcell.z).GetObjectsInCell();
        
        return GetGameObjectsInGridCell((int)gridPositionOfGridcell.x , (int)gridPositionOfGridcell.z);
    }

    public GameObject[] GetGameObjectsInGridCell(GameObject gameObjectInGridCell){
        return GetGameObjectsInGridCell(gameObjectInGridCell.transform.position);
    }

    public Vector3 switchToGridCoordinates(Vector3 worldCoordinates) {
        Vector3 gridCoordinates = BuildingSystem.current.SnapCoordinateToGrid(worldCoordinates);
        //Debug.Log("Snapped given coordinates: " + gridCoordinates.x + ", " + gridCoordinates.z);
        return new Vector3(gridCoordinates.x - .5f, gridCoordinates.y - .01f, gridCoordinates.z - .5f);
    }

    //returns the Grid Cell Object for a given point
    public GridCell GetGridCell(int x, int z)
    {
        //return positionsOfCells[x ][z ];

        //Used to be
        return positionsOfCells[x + 50][z + 50 ];
    }

    //returns the Grid Cell Object for a given world point
    public GridCell GetGridCellFromWorldPoint(Vector3 worldPointPosition)
    {
        Vector3 positionInGrid = switchToGridCoordinates(new Vector3(worldPointPosition.x, worldPointPosition.y, 0f));
        return positionsOfCells[(int)positionInGrid.x + 50][(int)positionInGrid.y + 50];
    }
    
    //Adds a new object to the fragmented array positionsOfCells
    public void AddObject(GameObject objectToAdd, int posX, int posY)
    {
        //Vector3 positionInGrid = BuildingSystem.current.SnapCoordinateToGrid(new Vector3(worldPointPosition.x, worldPointPosition.y, 0f));
        positionsOfCells[posX + 50][posY + 50].AddObject(objectToAdd, posX, posY);
    }

    public void AddObject(GameObject objectToAdd)
    {
        Vector3 positionInGrid = switchToGridCoordinates(objectToAdd.transform.position);
        int posX = (int)positionInGrid.x + 50;
        int posY = (int)positionInGrid.z + 50;

        if(positionsOfCells == null)
            Debug.Log("The cell is null");
        positionsOfCells[posX][posY].AddObject(objectToAdd, posX, posY);
    }

    public Vector2 switchToGridIndexCoordinates(Vector3 positionOfObject){
        Vector3 positionInGrid = switchToGridCoordinates(positionOfObject);
        int posX = (int)positionInGrid.x + 50;
        int posY = (int)positionInGrid.z + 50;
        return new Vector2(posX, posY);
    }

    //Removes an object from the fragmented array positionsOfCells
    public void RemoveObject(GameObject objectToRemove, int posX, int posY)
    {
        positionsOfCells[posX + 50][posY + 50].RemoveObject(objectToRemove);
    }

    public void RemoveObject(GameObject objectToRemove)
    {
        Vector3 positionInGrid = switchToGridCoordinates(objectToRemove.transform.position);
        positionsOfCells[(int)positionInGrid.x + 50][(int)positionInGrid.z + 50].RemoveObject(objectToRemove);
    }

}


//Container to hold everything a single Cell on the grid could need to know
public class GridCell 
{

    GameObject[] objectsInCell = new GameObject[8];
    public int xLocation { get; set; }
    public int yLocation { get; set; }
    public int numberOfObjectsInCell = 0;
    public bool isOverGround = false;

    //returns what objects are sitting on the cell
    public GameObject[] GetObjectsInCell(){
        return objectsInCell;
    }

    //Adds an object to the array of objects that are sitting on the cell
    public void AddObject(GameObject objectToAdd, int x, int y) {
        if (objectsInCell.Length > numberOfObjectsInCell)
        {
            GridManager gm = GridManager.GM;
            gm.AddGridObjectToList(objectToAdd);
            if(objectToAdd.GetComponent<Tile>() != null && objectToAdd.GetComponent<Tile>().tileScriptableObject != null){
                Tile tileScript = objectToAdd.GetComponent<Tile>();
                if(tileScript.tileScriptableObject.AnnualIncome > 0){
                    gm.AddToMoneyTileList(tileScript);
                }
                if(tileScript.tileScriptableObject.AnnualCarbonAdded != 0){
                    gm.AddToCarbonTileList(tileScript);
                }
            }
            xLocation = x;
            yLocation = y;
            objectsInCell[numberOfObjectsInCell] = objectToAdd;
            // Debug.Log("objects in cell: ");
            // for(int i = 0; i < objectsInCell.Length; i++){
            //     Debug.Log(objectsInCell[i]);
            // }
            
            //Debug.Log("objects in cell: " + objectsInCell);
            numberOfObjectsInCell++;
        }
    }


    //Removes an object from the array of objects that are sitting on the cell
    public void RemoveObject(GameObject objectToRemove) {
        for(int i = 0; i < objectsInCell.Length; i++) {
            if(objectToRemove == objectsInCell[i])
            {
                GridManager gm = GridManager.GM;
                gm.RemoveGridObjectFromList(objectToRemove);
                if(objectToRemove.GetComponent<Tile>() != null && objectToRemove.GetComponent<Tile>().tileScriptableObject != null){
                    Tile tileScript = objectToRemove.GetComponent<Tile>();
                    if(tileScript.tileScriptableObject.AnnualIncome > 0){
                        gm.RemoveFromMoneyTileList(tileScript);
                    }
                    if(tileScript.tileScriptableObject.AnnualCarbonAdded != 0){
                        gm.RemoveFromCarbonTileList(tileScript);
                    }
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
