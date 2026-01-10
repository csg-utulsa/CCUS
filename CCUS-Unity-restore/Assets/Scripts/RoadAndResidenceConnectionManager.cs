using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoadAndResidenceConnectionManager : MonoBehaviour
{
    public static RoadAndResidenceConnectionManager RARCM;
    

    void Start()
    {
        if(RARCM == null){
            RARCM = this;
        }else{
            Destroy(this);
        }
    }

    public bool AllResidencesAreConnected(){
        Tile[] allResidences = TileTypeCounter.current.ResidenceTileTracker.GetAllTiles();
        bool allResidencesConnected = true;
        foreach(Tile tile in allResidences){
            if(tile is ActivatableBuilding residence){
                if(residence.IsActivated == false){
                    allResidencesConnected = false;
                }
            }
            
        }
        return allResidencesConnected;
    }

    public void UpdateResidenceConnections(GameObject objectToCheck){

        if(objectToCheck != null && (objectToCheck.GetComponent<RoadConnections>() != null || objectToCheck.GetComponent<ActivatableBuilding>() != null)){
            List<int> TilesCheckedAlready = new List<int>();
            List<GameObject> ConnectedRoads = new List<GameObject>(); 
            List<GameObject> ConnectedResidences = new List<GameObject>(); 
            //Goes through each of roads connected to this road. Returns true if it's connected to an activatable building
            bool connectedTwoResidences = RecursivelyCheckTileConnections(objectToCheck, ConnectedRoads, ConnectedResidences, TilesCheckedAlready);

            //Activates/Deactivates the attached roads depending on if they connect two activatable buildings.
            if(connectedTwoResidences){
                foreach(GameObject connectedRoad in ConnectedRoads){
                    //Activate Road
                    if(connectedRoad.GetComponent<RoadTile>() != null){
                        //Debug.Log("ACTIVATED A ROAD");
                        connectedRoad.GetComponent<RoadTile>().ActivateBuilding();
                    }
                }
                foreach(GameObject connectedResidence in ConnectedResidences){
                    //Activate activatable building
                    if(connectedResidence.GetComponent<ActivatableBuilding>() != null){
                        //Debug.Log("ACTIVATED A RESIDENCE");
                        connectedResidence.GetComponent<ActivatableBuilding>().ActivateBuilding();
                    }
                }
            } else{
                //Deactivates roads and activatable buildings
                foreach(GameObject connectedRoad in ConnectedRoads){
                    //Deactivate Road
                    if(connectedRoad.GetComponent<RoadTile>() != null){
                        //Debug.Log("ACTIVATED A ROAD");
                        connectedRoad.GetComponent<RoadTile>().DeactivateBuilding();
                    }
                }
                foreach(GameObject connectedResidence in ConnectedResidences){
                    //Deactivate activatablel building
                    if(connectedResidence.GetComponent<ActivatableBuilding>() != null){
                        //Debug.Log("DEACTIVATED A RESIDENCE");
                        connectedResidence.GetComponent<ActivatableBuilding>().DeactivateBuilding();
                    }
                }
            }

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
            
        }
        
        GameObject[] neighboringTiles = GridManager.GM.GetRoadNeighbors(nextObjectToCheck);
        bool _ConnectedTwoResidences = false;
        for(int i = 0; i < neighboringTiles.Length; i++){
            //This if statement checks if the object isn't null, and if it hasn't already checked the object
            if(neighboringTiles[i] != null && !TilesCheckedAlready.Contains(neighboringTiles[i].GetInstanceID())){
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
}
