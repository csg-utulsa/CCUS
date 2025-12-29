using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResidentialBuilding : MonoBehaviour
{

    public bool IsConnectedToOtherResidences {get; set;} = false;
    List<int> GameObjectsCheckedSoFar = new List<int>();

    //FIXME -- This is temporary. DO NOT use the update function to update connections long term
    float timer = 0;
    void Update(){
        timer += Time.deltaTime;
        if(timer > 15f){
            timer = 0f;
            UpdateResidenceConnections();

        }
    }

    public void UpdateResidenceConnections(){
        GameObjectsCheckedSoFar.Clear();
        GameObject[] neighboringRoads = GridManager.GM.GetRoadNeighbors(gameObject);
        for(int i = 0; i < neighboringRoads.Length; i++){
            if(neighboringRoads[i] != null && neighboringRoads[i].GetComponent<RoadConnections>() != null){
                List<GameObject> ConnectedRoads = new List<GameObject>(); 
                List<GameObject> ConnectedResidences = new List<GameObject>(); 
                bool isConnected = RecursivelyCheckConnections(neighboringRoads[i], ConnectedRoads, ConnectedResidences);
                if(isConnected){
                    ConnectedResidences.Add(gameObject);

                    foreach(GameObject connectedRoad in ConnectedRoads){
                        if(connectedRoad.GetComponent<RoadConnections>() != null){
                            connectedRoad.GetComponent<RoadConnections>().activateConnectedRoad();
                            //Debug.Log("activatedroad");
                        }
                    }
                } else{
                    foreach(GameObject connectedRoad in ConnectedRoads){
                        if(connectedRoad.GetComponent<RoadConnections>() != null){
                            //Debug.Log("DEACTIVATED A ROAD. I'm so cool.");
                            connectedRoad.GetComponent<RoadConnections>().deactivateConnectedRoad();
                        }
                    }
                }
            }
        }


        

    }

    // Recursive function that checks all of the roads connected to an object
    // Returns true if it's connected to another residence
    private bool RecursivelyCheckConnections(GameObject nextObjectToCheck, List<GameObject> ConnectedRoads, List<GameObject> ConnectedResidences){
        
        //Makes sure the first road gets added
        if(!GameObjectsCheckedSoFar.Contains(nextObjectToCheck.GetInstanceID()) && nextObjectToCheck.GetComponent<RoadConnections>() != null){
            GameObjectsCheckedSoFar.Add(nextObjectToCheck.GetInstanceID());
            ConnectedRoads.Add(nextObjectToCheck);
        }
        
        GameObject[] neighboringTiles = GridManager.GM.GetRoadNeighbors(nextObjectToCheck);
        bool _ConnectedToResidence = false;
        for(int i = 0; i < neighboringTiles.Length; i++){
            //This if statement checks if the object isn't null, and if it hasn't already checked the object
            if(neighboringTiles[i] != null && !GameObjectsCheckedSoFar.Contains(neighboringTiles[i].GetInstanceID())){
                //If statement runs if the neighboring object is a residential building that isn't the one that this script is on
                if((neighboringTiles[i].GetComponent<ResidentialBuilding>() != null) && neighboringTiles[i].GetInstanceID() != gameObject.GetInstanceID()){
                    ConnectedResidences.Add(neighboringTiles[i]);
                    _ConnectedToResidence = true;
                }

                //If statement runs if the neighboring object is a road
                if(neighboringTiles[i].GetComponent<RoadConnections>() != null){
                    //ConnectedRoads.Add(neighboringTiles[i]);
                    if(RecursivelyCheckConnections(neighboringTiles[i], ConnectedRoads, ConnectedResidences)){
                        _ConnectedToResidence = true;
                    }
                }
            }
        }
        return _ConnectedToResidence;

    }
}
