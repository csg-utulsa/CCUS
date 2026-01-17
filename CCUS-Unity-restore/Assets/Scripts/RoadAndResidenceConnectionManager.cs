using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class RoadAndResidenceConnectionManager : MonoBehaviour
{

    public static RoadAndResidenceConnectionManager current;
    

    void Start()
    {
        if(current == null){
            current = this;
        }else{
            Destroy(this);
        }
    }

    public bool AllResidencesAreConnected(){
        Tile[] allResidences = TileTypeCounter.current.ResidenceTileTracker.GetAllTiles();
        bool allResidencesConnected = true;
        foreach(Tile tile in allResidences){
            if(tile is ResidentialBuilding residence){
                if(residence.IsActivated == false){
                    allResidencesConnected = false;
                }
            }
            
        }
        return allResidencesConnected;
    }

    private int NumberOfTilesChecked = 0;
    public void UpdateResidenceConnections(GameObject objectToCheck){

        if(objectToCheck != null && (objectToCheck.GetComponent<ActivatableTile>() != null)){
            List<ActivatableTile> connectedTiles = new List<ActivatableTile>();

            NumberOfTilesChecked = 0;
            bool connectedTwoTiles = GetTilesToActivateOrDeactivate(objectToCheck, connectedTiles);
            if(connectedTwoTiles){
                foreach(ActivatableTile connectedTile in connectedTiles){
                    connectedTile.ActivateBuilding();
                }
            }else{
                foreach(ActivatableTile connectedTile in connectedTiles){
                    connectedTile.DeactivateBuilding();
                }
            }
            // List<int> TilesCheckedAlready = new List<int>();
            // List<GameObject> ConnectedRoads = new List<GameObject>(); 
            // List<GameObject> ConnectedResidences = new List<GameObject>(); 
            // //Goes through each of roads connected to this road. Returns true if it's connected to an activatable building
            // bool connectedTwoResidences = RecursivelyCheckTileConnections(objectToCheck, ConnectedRoads, ConnectedResidences, TilesCheckedAlready);

            // //Activates/Deactivates the attached roads depending on if they connect two activatable buildings.
            // if(connectedTwoResidences){
            //     foreach(GameObject connectedRoad in ConnectedRoads){
            //         //Activate Road
            //         if(connectedRoad.GetComponent<RoadTile>() != null){
            //             //Debug.Log("ACTIVATED A ROAD");
            //             connectedRoad.GetComponent<RoadTile>().ActivateBuilding();
            //         }
            //     }
            //     foreach(GameObject connectedResidence in ConnectedResidences){
            //         //Activate activatable building
            //         if(connectedResidence.GetComponent<ActivatableBuilding>() != null){
            //             //Debug.Log("ACTIVATED A RESIDENCE");
            //             connectedResidence.GetComponent<ActivatableBuilding>().ActivateBuilding();
            //         }
            //     }
            // } else{
            //     //Deactivates roads and activatable buildings
            //     foreach(GameObject connectedRoad in ConnectedRoads){
            //         //Deactivate Road
            //         if(connectedRoad.GetComponent<RoadTile>() != null){
            //             //Debug.Log("ACTIVATED A ROAD");
            //             connectedRoad.GetComponent<RoadTile>().DeactivateBuilding();
            //         }
            //     }
            //     foreach(GameObject connectedResidence in ConnectedResidences){
            //         //Deactivate activatablel building
            //         if(connectedResidence.GetComponent<ActivatableBuilding>() != null){
            //             //Debug.Log("DEACTIVATED A RESIDENCE");
            //             connectedResidence.GetComponent<ActivatableBuilding>().DeactivateBuilding();
            //         }
            //     }
            // }

        }
    }

    //Used when loading chunks to activate building connections
    public void LoadResidenceConnections(GameObject objectToCheck){

        if(objectToCheck != null && (objectToCheck.GetComponent<RoadConnections>() != null || objectToCheck.GetComponent<ActivatableBuilding>() != null)){
            List<ActivatableTile> connectedTiles = new List<ActivatableTile>();
            bool connectedTwoTiles = GetTilesToActivateOrDeactivate(objectToCheck, connectedTiles);
            if(connectedTwoTiles){
                foreach(ActivatableTile connectedTile in connectedTiles){
                    connectedTile.LoadActivatedBuilding();
                }
            }else{
                foreach(ActivatableTile connectedTile in connectedTiles){
                    connectedTile.LoadDeactivatedBuilding();
                }
            }

        }
    }

    public bool GetTilesToActivateOrDeactivate(GameObject tile, List<ActivatableTile> ConnectedTiles){
        if(tile != null && (tile.GetComponent<ActivatableTile>())){
            List<int> TilesCheckedAlready = new List<int>();
            List<GameObject> ConnectedRoads = new List<GameObject>(); 
            List<GameObject> ConnectedBuildings = new List<GameObject>(); 
            
            //Gets all of the connected buildings and roads
            bool connectedTwoBuildings = RecursivelyCheckTileConnections(tile, ConnectedRoads, ConnectedBuildings, TilesCheckedAlready);
            
            //Saves all the connected tiles to the activatableTiles array
            List<GameObject> AllConnectedTiles = new List<GameObject>();
            AllConnectedTiles.AddRange(ConnectedRoads);
            AllConnectedTiles.AddRange(ConnectedBuildings);
            foreach(GameObject connectedTile in AllConnectedTiles){
                ActivatableTile activatableTile = connectedTile.GetComponent<ActivatableTile>();
                if(activatableTile != null){
                    ConnectedTiles.Add(activatableTile);
                }
            }

            return connectedTwoBuildings;
        } else{
            return false;
        }
    }

    

    // Recursive function that checks all of the roads connected to an object
    // Returns true if it's connected to another Activatable building
    private bool RecursivelyCheckTileConnections(GameObject nextObjectToCheck, List<GameObject> ConnectedRoads, List<GameObject> ConnectedResidences, List<int> TilesCheckedAlready){
        
        //Adds roads/residences to the ConnectedRoads and ConnectedResidences lists.
        if(!TilesCheckedAlready.Contains(nextObjectToCheck.GetInstanceID())){ //&& nextObjectToCheck.GetComponent<RoadConnections>() != null){
            TilesCheckedAlready.Add(nextObjectToCheck.GetInstanceID());
            if(nextObjectToCheck.GetComponent<RoadConnections>() != null){
                ConnectedRoads.Add(nextObjectToCheck);
            }else if(nextObjectToCheck.GetComponent<ActivatableBuilding>() != null){
                ConnectedResidences.Add(nextObjectToCheck);
            }
            
        }else{
            return false;
        }

        NumberOfTilesChecked++;
        
        GameObject[] neighboringTiles = RoadAndResidenceConnectionManager.current.GetRoadNeighbors(nextObjectToCheck);
        bool _ConnectedTwoResidences = false;
        for(int i = 0; i < neighboringTiles.Length; i++){
            //This if statement checks if the object isn't null
            if(neighboringTiles[i] != null){
                //Checks if the neighboring object is an activatable building that hasn't already been checked. It also prevents connecting two residences that are sitting next to each other w/o roads
                if(neighboringTiles[i].GetComponent<ActivatableBuilding>() != null && !ConnectedResidences.Contains(neighboringTiles[i]) && (nextObjectToCheck.GetComponent<ActivatableBuilding>() == null)){
                    ConnectedResidences.Add(neighboringTiles[i]);
                    if(ConnectedResidences.Count >= 2){
                        _ConnectedTwoResidences = true;
                    }
                    
                }

                //Tells next object to run a recursive check if the neighboring object is a road or residence, but prevents traveling through two residenes sitting next to each other
                if(neighboringTiles[i].GetComponent<RoadConnections>() != null || (neighboringTiles[i].GetComponent<ActivatableBuilding>() != null && nextObjectToCheck.GetComponent<ActivatableBuilding>() == null)){
                    //ConnectedRoads.Add(neighboringTiles[i]);
                    if(RecursivelyCheckTileConnections(neighboringTiles[i], ConnectedRoads, ConnectedResidences, TilesCheckedAlready)){
                        _ConnectedTwoResidences = true;
                    }
                }
            }
        }
        return _ConnectedTwoResidences;

    }

    public GameObject[] GetRoadNeighbors(GameObject _tile){
        return GetRoadNeighbors(_tile.transform.position);
    }

    //returns all activatabletile neighbors of input tile
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
                //Checks if a neighbor object is an activatable tile (like roads, residences, or factories), since those are the only things roads connect to
                if(obj.GetComponent<ActivatableTile>() != null){
                    tileNeighbors[i] = obj;
                }
            }
        }
        return tileNeighbors;
    }
}
