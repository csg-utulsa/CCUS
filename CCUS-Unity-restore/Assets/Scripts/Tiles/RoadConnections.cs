using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RoadConnections : MonoBehaviour
{

    //When enabled, it makes the roads also connect to houses, apartments, etc.
    public bool visuallyConnectToResidences = false;

    //public GameObject activationGraphic;

    public Material[] modelList;

    public Material previousModel;
    public int previousOrientation;

    public GameObject[] neighbors;

    public int correctModelRotations = 90;


    //Lists all possible arrangements of neighbors around a tile, with a 1 representing a neighbors
    //the first one is the top, the next one is the right, the next one is the bottom, and the last is the left
    public int[] neighborAlignments = new int[]{
        0000,
        0001,
        0010,
        0011,
        0100,
        0101,
        0110,
        0111,
        1000,
        1001,
        1010,
        1011,
        1100,
        1101,
        1110,
        1111
    };

    //Corresponding array that lists the directional orienation a tile should have for a given arrangement of neighbors (from neighborAlignments)
    //0 is up, 1 is right, 2 is down, 3 is left
    public int[] arrangementOrientationAngles = new int[]{
        1,
        3,
        2,
        2,
        1,
        1,
        1,
        2,
        0,
        3,
        2,
        3,
        0,
        0,
        1,
        0
    };
    
    //Another corresponding array that lists which model a tile should have for a given tile arrangement
    //0 is the blank model, 1 is the straight model, 2 is the corner, 3 is the T, and 4 is the all ways tile
    // By the way, the models must be oriented like this:     Straight: |      T-Shape: ⊥        Corner: L 
    public int[] arrangementModels = new int[]{
        0,
        1,
        1,
        2,
        1,
        1,
        2,
        3,
        1,
        2,
        1,
        3,
        2,
        3,
        3,
        4
    };
    

    void Start(){
        //Updates this tile's visual road connections on creation
        UpdateModelConnections(false);
    }

    // //This method displays if this road is connecting two or more residences
    // public void activateConnectedRoad(){
    //     if(activationGraphic != null)
    //         activationGraphic.SetActive(true);
    // }
    // //This method displays if this road is NOT connecting two or more residences
    // public void deactivateConnectedRoad(){
    //     if(activationGraphic != null)
    //         activationGraphic.SetActive(false);
    // }

    //This method switches side "i" of a tile, which is either side 0, 1, 2, or 3, to the opposite side.
    public int flipTileSide(int initialSide){
        if(initialSide < 2){
            return initialSide + 2;
        } else{
            return initialSide - 2;
        }
    }

    


    //This method updates road's visual connections. 
    //If makeNeighborsCheckConnections = true, it also forces its neighbors to update their connections.
    public void UpdateModelConnections(bool makeNeighborsCheckConnections){
        GameObject[] neighborGameObjects = GridManager.GM.GetRoadNeighbors(gameObject);

        neighbors = neighborGameObjects;
        int currentAlignment = Array.IndexOf(neighborAlignments, neighborsAsBits(neighborGameObjects));
        int newOrientation = arrangementOrientationAngles[currentAlignment];
        Material newModel = modelList[arrangementModels[currentAlignment]];

        if(previousModel == null || previousOrientation != newOrientation || newModel != previousModel){
            GetComponentInChildren<Renderer>().material = newModel;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, (newOrientation * 90f) + correctModelRotations, transform.rotation.eulerAngles.z);
            
            if(makeNeighborsCheckConnections){
                for(int i = 0; i < neighborGameObjects.Length; i++){
                    GameObject _neighbor = neighborGameObjects[i];
                    if(_neighbor != null && _neighbor.GetComponent<RoadConnections>() != null){
                        //Flip which side the tile is on. That way, _neighbor knows which side to add a new neighbor (this road) on.
                        _neighbor.GetComponent<RoadConnections>().UpdateConnectionsWithExtraNeighbor(flipTileSide(i), gameObject);
                        //_neighbor.GetComponent<RoadConnections>().UpdateModelConnections(false);
                    }    
                }
            }
            
        }
    }

    //Overload method that passes an extra neighbor that isn't listed in the Grid Manager.
    //Allows roads to connect to floating objects that the user hasn't placed yet.
    public void UpdateConnectionsWithExtraNeighbor(int whichNeighborToAdd, GameObject extraNeighbor){
        GameObject[] neighborGameObjects = GridManager.GM.GetRoadNeighbors(gameObject);
        neighborGameObjects[whichNeighborToAdd] = extraNeighbor;
        neighbors = neighborGameObjects;
        int currentAlignment = Array.IndexOf(neighborAlignments, neighborsAsBits(neighborGameObjects));
        int newOrientation = arrangementOrientationAngles[currentAlignment];
        Material newModel = modelList[arrangementModels[currentAlignment]];

        if(previousModel == null || previousOrientation != newOrientation || newModel != previousModel){
            GetComponentInChildren<Renderer>().material = newModel;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, (newOrientation * 90f) + correctModelRotations, transform.rotation.eulerAngles.z);
            
            // if(makeNeighborsCheckConnections){
            //     //This is mostly a duplicate of code from UpdateNeighborConnections, but partially rewriting it here
            //     //is more efficient for the system, since we don't have to call GridManager's GetRoadNeighbors() again.
            //     foreach(GameObject _neighbor in neighborGameObjects){
            //         if(_neighbor != null && _neighbor.GetComponent<RoadConnections>() != null){
            //             _neighbor.GetComponent<RoadConnections>().UpdateModelConnections(false);
            //         }    
            //     }
            // }
            
        }

    }

    public void UpdateNeighborConnections(){
        GameObject[] neighborGameObjects = GridManager.GM.GetRoadNeighbors(gameObject);
        neighbors = neighborGameObjects;
        foreach(GameObject _neighbor in neighborGameObjects){

            //Updates road connections for each neighbor
            if(_neighbor != null && _neighbor.GetComponent<RoadConnections>() != null){
                //The "false" being input here just tells it not to tell each of its neighbors to update their connections
                _neighbor.GetComponent<RoadConnections>().UpdateModelConnections(false);
            }   
        }
    }

    //Converts the arrangement of neighbors into a 4 digit code of 1s and 0s
    public int neighborsAsBits(GameObject[] _neighbors){
        int[] neighborAlignmentAsArray = new int[_neighbors.Length];
        for(int i = 0; i < _neighbors.Length; i++){
            //Only includes neighbors that aren't residential buildings, so it doesn't connect to them.
            if( (_neighbors[i] != null) && (visuallyConnectToResidences || !(_neighbors[i].GetComponent<ActivatableBuilding>() != null) ) ){
                neighborAlignmentAsArray[i] = 1;
            }
            else{
                neighborAlignmentAsArray[i] = 0;
            }
        }
        return int.Parse(string.Join("", neighborAlignmentAsArray));
    }
}
