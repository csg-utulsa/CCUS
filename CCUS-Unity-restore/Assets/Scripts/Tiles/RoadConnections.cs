using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RoadConnections : MonoBehaviour
{
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

    // public void correctOrientation(int ){

    // }

    public void UpdateModelConnections(bool makeNeighborsCheckConnections){
        //Debug.Log("Updating model connections");
        GameObject[] neighborGameObjects = GridManager.GM.GetRoadNeighbors(gameObject);
        neighbors = neighborGameObjects;
        int currentAlignment = Array.IndexOf(neighborAlignments, neighborsAsBits(neighborGameObjects));
        int newOrientation = arrangementOrientationAngles[currentAlignment];
        Material newModel = modelList[arrangementModels[currentAlignment]];

        if(previousModel == null || previousOrientation != newOrientation || newModel != previousModel){
            //Debug.Log("Model Number: " + arrangementModels[currentAlignment]);
            GetComponentInChildren<Renderer>().material = newModel;//("Model Arrangement: " + arrangementModels[currentAlignment], newModel);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, (newOrientation * 90f) + correctModelRotations, transform.rotation.eulerAngles.z);
            
            if(makeNeighborsCheckConnections){
                foreach(GameObject _neighbor in neighborGameObjects){
                    if(_neighbor != null && _neighbor.GetComponent<RoadConnections>() != null){
                        _neighbor.GetComponent<RoadConnections>().UpdateModelConnections(false);
                    }    
                }
            }
            
        }

    }


    public int neighborsAsBits(GameObject[] _neighbors){
        int[] neighborAlignmentAsArray = new int[_neighbors.Length];
        for(int i = 0; i < _neighbors.Length; i++){
            if(_neighbors[i] != null){
                neighborAlignmentAsArray[i] = 1;
            }
            else{
                neighborAlignmentAsArray[i] = 0;
            }
        }
        return int.Parse(string.Join("", neighborAlignmentAsArray));
    }
}
