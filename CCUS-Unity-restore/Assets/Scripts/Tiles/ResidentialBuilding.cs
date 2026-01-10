using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResidentialBuilding : ActivatableBuilding
{

    public bool IsConnectedToOtherResidences {get; set;} = false;

    public override void ThisTileJustPlaced(){
        base.ThisTileJustPlaced();

        //Updates the cap on number of people
        if(PeopleManager.current != null){
            PeopleManager.current.UpdateMaxPeople();
        }

    }

    public override void ThisTileAboutToBeDestroyed(){
        base.ThisTileAboutToBeDestroyed();

        //Updates the cap on number of people
        if(PeopleManager.current != null){
            PeopleManager.current.UpdateMaxPeople();
        }

    }

    //public bool IsActivated {get; set;} = false;
    //List<int> GameObjectsCheckedSoFar = new List<int>();

    //public GameObject buildingActivatedGraphic;

    //FIXME -- This Update() function is temporary. DO NOT use the update function to update connections long term
    // float timer = 0;
    // void Update(){
    //     timer += Time.deltaTime;
    //     if(timer > 15f){
    //         timer = 0f;
    //         UpdateResidenceConnections();

    //     }
    // }

    //Allows for a residence to hold people, once it is connected to another residence by roads.
    // public void ActivateBuilding(){
    //     IsActivated = true;

    //     //Change a graphic to make it clear the house is activated.
    //     if(buildingActivatedGraphic != null){
    //       buildingActivatedGraphic.SetActive(true);  
    //     }
    // }

    // public void DeactivateResidence(){
    //     IsActivated = false;

    //     //Change a graphic to make it clear the house is NOT activated.
    //     if(buildingActivatedGraphic != null){
    //         buildingActivatedGraphic.SetActive(false);
    //     }
    // }

    //Duplicated in Grid Manager (Switch to using GridManager's version)
    // public void UpdateResidenceConnections(){
    //     GameObjectsCheckedSoFar.Clear();
    //     GameObject[] neighboringRoads = GridManager.GM.GetRoadNeighbors(gameObject);

    //     //Loops through each of the roads connected to this residence (4 max)
    //     for(int i = 0; i < neighboringRoads.Length; i++){
    //         if(neighboringRoads[i] != null && neighboringRoads[i].GetComponent<RoadConnections>() != null){
    //             List<GameObject> ConnectedRoads = new List<GameObject>(); 
    //             List<GameObject> ConnectedResidences = new List<GameObject>(); 
    //             //Goes through each of roads connected to this road. Returns true if it's connected to a residence
    //             bool isConnected = RecursivelyCheckConnections(neighboringRoads[i], ConnectedRoads, ConnectedResidences);

    //             //Activates/Deactivates the attached roads depending on if they connect two residences.
    //             if(isConnected){
    //                 ActivateResidence();
    //                 ConnectedResidences.Add(gameObject);
    //                 foreach(GameObject connectedRoad in ConnectedRoads){
    //                     if(connectedRoad.GetComponent<RoadConnections>() != null){
    //                         connectedRoad.GetComponent<RoadConnections>().activateConnectedRoad();
    //                         //Debug.Log("activatedroad");
    //                     }
    //                 }
    //             } else{
    //                 DeactivateResidence();
    //                 foreach(GameObject connectedRoad in ConnectedRoads){
    //                     if(connectedRoad.GetComponent<RoadConnections>() != null){
    //                         //Debug.Log("DEACTIVATED A ROAD. I'm so cool.");
    //                         connectedRoad.GetComponent<RoadConnections>().deactivateConnectedRoad();
    //                     }
    //                 }
    //             }
    //         }
    //     }
    // }

    //Duplicated in Grid Manager (Switch to using GridManager's version)
    // Recursive function that checks all of the roads connected to an object
    // Returns true if it's connected to another residence
    // private bool RecursivelyCheckConnections(GameObject nextObjectToCheck, List<GameObject> ConnectedRoads, List<GameObject> ConnectedResidences){
        
    //     //Makes sure the first road gets added
    //     if(!GameObjectsCheckedSoFar.Contains(nextObjectToCheck.GetInstanceID()) && nextObjectToCheck.GetComponent<RoadConnections>() != null){
    //         GameObjectsCheckedSoFar.Add(nextObjectToCheck.GetInstanceID());
    //         ConnectedRoads.Add(nextObjectToCheck);
    //     }
        
    //     GameObject[] neighboringTiles = GridManager.GM.GetRoadNeighbors(nextObjectToCheck);
    //     bool _ConnectedToResidence = false;
    //     for(int i = 0; i < neighboringTiles.Length; i++){
    //         //This if statement checks if the object isn't null, and if it hasn't already checked the object
    //         if(neighboringTiles[i] != null && !GameObjectsCheckedSoFar.Contains(neighboringTiles[i].GetInstanceID())){
    //             //If statement runs if the neighboring object is a residential building that isn't the one that this script is on
    //             if((neighboringTiles[i].GetComponent<ResidentialBuilding>() != null) && neighboringTiles[i].GetInstanceID() != gameObject.GetInstanceID()){
    //                 ConnectedResidences.Add(neighboringTiles[i]);
    //                 _ConnectedToResidence = true;
    //             }

    //             //If statement runs if the neighboring object is a road
    //             if(neighboringTiles[i].GetComponent<RoadConnections>() != null){
    //                 //ConnectedRoads.Add(neighboringTiles[i]);
    //                 if(RecursivelyCheckConnections(neighboringTiles[i], ConnectedRoads, ConnectedResidences)){
    //                     _ConnectedToResidence = true;
    //                 }
    //             }
    //         }
    //     }
    //     return _ConnectedToResidence;

    // }
}
