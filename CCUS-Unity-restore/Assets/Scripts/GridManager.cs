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
    public List<ResidentialBuilding> residenceTiles = new List<ResidentialBuilding>();
    public List<RoadConnections> roadTiles = new List<RoadConnections>();



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

    public RoadConnections[] GetRoadTiles(){
        RoadConnections[] returnArray = new RoadConnections[roadTiles.Count];
        for(int i = 0; i < roadTiles.Count; i++){
            returnArray[i] = roadTiles[i];
        }
        return returnArray;
    }

    public ResidentialBuilding[] GetResidentialTiles(){
        ResidentialBuilding[] returnArray = new ResidentialBuilding[residenceTiles.Count];
        for(int i = 0; i < residenceTiles.Count; i++){
            returnArray[i] = residenceTiles[i];
        }
        return returnArray;
    }

    public void AddToMoneyTileList(Tile tileToAdd){
        moneyProducingTiles.Add(tileToAdd);
    }    

    public void AddToCarbonTileList(Tile tileToAdd){
        carbonProducingTiles.Add(tileToAdd);
    } 

    public void AddToResidenceTileList(ResidentialBuilding residenceToAdd){
        residenceTiles.Add(residenceToAdd);
    }    

    public void AddToRoadTileList(RoadConnections tileToAdd){
        roadTiles.Add(tileToAdd);
    }

    public void RemoveFromMoneyTileList(Tile tileToAdd){
        moneyProducingTiles.Remove(tileToAdd);
    }    

    public void RemoveFromCarbonTileList(Tile tileToAdd){
        carbonProducingTiles.Remove(tileToAdd);
    }    

    public void RemoveFromResidenceTileList(ResidentialBuilding residenceToAdd){
        residenceTiles.Remove(residenceToAdd);
    }

    public void RemoveFromRoadTileList(RoadConnections tileToRemove){
        roadTiles.Add(tileToRemove);
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
        if(GM == null){
           GM = this; 
        }else{
            Destroy(this);
        }
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
                //Checks if a neighbor object is either a road or a residential buildings, since those are the only things roads connect to
                if(obj.GetComponent<RoadConnections>() != null || obj.GetComponent<ResidentialBuilding>()){
                    tileNeighbors[i] = obj;
                }
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

    public void UpdateResidenceConnections(GameObject objectToCheck){

        if(objectToCheck != null && (objectToCheck.GetComponent<RoadConnections>() != null || objectToCheck.GetComponent<ResidentialBuilding>() != null)){
            List<int> TilesCheckedAlready = new List<int>();
            List<GameObject> ConnectedRoads = new List<GameObject>(); 
            List<GameObject> ConnectedResidences = new List<GameObject>(); 
            //Goes through each of roads connected to this road. Returns true if it's connected to a residence
            bool connectedTwoResidences = RecursivelyCheckTileConnections(objectToCheck, ConnectedRoads, ConnectedResidences, TilesCheckedAlready);

            //Activates/Deactivates the attached roads depending on if they connect two residences.
            if(connectedTwoResidences){
                foreach(GameObject connectedRoad in ConnectedRoads){
                    //Activate Road
                    if(connectedRoad.GetComponent<RoadConnections>() != null){
                        //Debug.Log("ACTIVATED A ROAD");
                        connectedRoad.GetComponent<RoadConnections>().activateConnectedRoad();
                    }
                }
                foreach(GameObject connectedResidence in ConnectedResidences){
                    //Activate Residence
                    if(connectedResidence.GetComponent<ResidentialBuilding>() != null){
                        //Debug.Log("ACTIVATED A RESIDENCE");
                        connectedResidence.GetComponent<ResidentialBuilding>().ActivateResidence();
                    }
                }
            } else{
                //Deactivates roads and residences
                foreach(GameObject connectedRoad in ConnectedRoads){
                    //Deactivate Road
                    if(connectedRoad.GetComponent<RoadConnections>() != null){
                        //Debug.Log("DEACTIVATED A ROAD");
                        connectedRoad.GetComponent<RoadConnections>().deactivateConnectedRoad();
                    }
                }
                foreach(GameObject connectedResidence in ConnectedResidences){
                    //Deactivate Residence
                    if(connectedResidence.GetComponent<ResidentialBuilding>() != null){
                        //Debug.Log("DEACTIVATED A RESIDENCE");
                        connectedResidence.GetComponent<ResidentialBuilding>().DeactivateResidence();
                    }
                }
            }

        }
    }

    // Recursive function that checks all of the roads connected to an object
    // Returns true if it's connected to another residence
    private bool RecursivelyCheckTileConnections(GameObject nextObjectToCheck, List<GameObject> ConnectedRoads, List<GameObject> ConnectedResidences, List<int> TilesCheckedAlready){
        
        //Adds roads/residences to the ConnectedRoads and ConnectedResidences lists.
        if(!TilesCheckedAlready.Contains(nextObjectToCheck.GetInstanceID())){ //&& nextObjectToCheck.GetComponent<RoadConnections>() != null){
            TilesCheckedAlready.Add(nextObjectToCheck.GetInstanceID());
            if(nextObjectToCheck.GetComponent<RoadConnections>() != null){
                ConnectedRoads.Add(nextObjectToCheck);
            }else if(nextObjectToCheck.GetComponent<ResidentialBuilding>() != null){
                ConnectedResidences.Add(nextObjectToCheck);
            }
            
        }
        
        GameObject[] neighboringTiles = GM.GetRoadNeighbors(nextObjectToCheck);
        bool _ConnectedTwoResidences = false;
        for(int i = 0; i < neighboringTiles.Length; i++){
            //This if statement checks if the object isn't null, and if it hasn't already checked the object
            if(neighboringTiles[i] != null && !TilesCheckedAlready.Contains(neighboringTiles[i].GetInstanceID())){
                //Checks if the neighboring object is a residential building that hasn't already been checked. It also prevents connecting two residences that are sitting next to each other w/o roads
                if(neighboringTiles[i].GetComponent<ResidentialBuilding>() != null && !ConnectedResidences.Contains(neighboringTiles[i]) && (nextObjectToCheck.GetComponent<ResidentialBuilding>() == null)){
                    ConnectedResidences.Add(neighboringTiles[i]);
                    if(ConnectedResidences.Count >= 2){
                        _ConnectedTwoResidences = true;
                    }
                    
                }

                //Tells next object to run a recursive check if the neighboring object is a road or residence, but prevents traveling through two residenes sitting next to each other
                if(neighboringTiles[i].GetComponent<RoadConnections>() != null || (neighboringTiles[i].GetComponent<ResidentialBuilding>() != null && nextObjectToCheck.GetComponent<ResidentialBuilding>() == null)){
                    //ConnectedRoads.Add(neighboringTiles[i]);
                    if(RecursivelyCheckTileConnections(neighboringTiles[i], ConnectedRoads, ConnectedResidences, TilesCheckedAlready)){
                        _ConnectedTwoResidences = true;
                    }
                }
            }
        }
        return _ConnectedTwoResidences;

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
                if(objectToAdd.GetComponent<ResidentialBuilding>() != null){
                    gm.AddToResidenceTileList(objectToAdd.GetComponent<ResidentialBuilding>());
                } else if(objectToAdd.GetComponent<RoadConnections>() != null){
                    gm.AddToRoadTileList(objectToAdd.GetComponent<RoadConnections>());
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
                    if(objectToRemove.GetComponent<ResidentialBuilding>() != null){
                        gm.RemoveFromResidenceTileList(objectToRemove.GetComponent<ResidentialBuilding>());
                    } else if(objectToRemove.GetComponent<RoadConnections>() != null){
                        gm.RemoveFromRoadTileList(objectToRemove.GetComponent<RoadConnections>());
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
