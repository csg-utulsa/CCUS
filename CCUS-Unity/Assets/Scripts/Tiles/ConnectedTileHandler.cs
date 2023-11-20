using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ConnectedTileHandler : MonoBehaviour
{
    [Header("Model to be altered")]
    public GameObject TileModelGO;
    [Header("Connected Tile Scriptable Object")]
    public ConnectedTileScriptableObject baseModels;
    [Header("Current Model State")]
    public Mesh currentModel;
    public float rotation; //How much the base model should be rotated to fit the adjacency

    [Header("Current Adjacecy")]
    public adjacencyFlag hasNeightbors;
    public GameObject[] neighborGO = new GameObject[4];

    private (Mesh model, float rotation)[] modelList;

    [Flags] public enum adjacencyFlag
    {
        None = 0,
        North, XPlus = 1,
        East, ZPlus = 2,
        South, XMinus = 4,
        West, ZMinus = 8,
    }
    private void Awake()
    {
        (Mesh model, float rotation)[] modelList = {
            (baseModels.islandModel,        0 + baseModels.isleRot),        // [0]  Island Model,           facing Nothing
            (baseModels.endModel,           0 + baseModels.endRot),         // [1]  End Model,              facing North
            (baseModels.endModel,           90 + baseModels.endRot),        // [2]  End Model,              facing East
            (baseModels.cornerModel,        0 + baseModels.cornerRot),      // [3]  Corner Model,           facing North and East
            (baseModels.endModel,           180 + baseModels.endRot),       // [4]  End Model,              facing South
            (baseModels.straightModel,      0 + baseModels.straightRot),    // [5]  Straight Model.         facing North and South
            (baseModels.cornerModel,        90 + baseModels.cornerRot),     // [6]  Corner Model.           facing East and South
            (baseModels.tIntersectionModel, 90 + baseModels.tRot),          // [7]  T Intersection Model,   facing North, East, and South
            (baseModels.endModel,           -90 + baseModels.endRot),       // [8]  End Model,              facing West
            (baseModels.cornerModel,        -90 + baseModels.cornerRot),    // [9]  Corner Model,           facing North and West
            (baseModels.straightModel,      90 + baseModels.straightRot),   // [10] Straight Model,         facing East and West
            (baseModels.tIntersectionModel, 0 + baseModels.tRot),           // [11] T Intersection Model,   facing North, East, and West
            (baseModels.cornerModel,        180 + baseModels.cornerRot),    // [12] Corner Model,           facing South and West
            (baseModels.tIntersectionModel, -90 + baseModels.tRot),         // [13] T Intersection Model,   facing North, South, and West,
            (baseModels.tIntersectionModel, 180 + baseModels.tRot),         // [14] T Intersection Model,   facing East, South and West,
            (baseModels.xIntersectionModel, 0 + baseModels.xRot)            // [15] X intersection Model,   facing North, South, East and West,
        };

        
    }
    // Start is called before the first frame update
    void Start()
    {
        if (TileModelGO == null)
        {
            TileModelGO = this.transform.GetChild(0).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ChangeModelOnAdjacency();
        UpdateModel();
    }

    public void ChangeModelOnAdjacency()
    {   

    }//End UpdateModelOnAdjacency


    public void UpdateModel()
    {

    }

    public void AddNeighbor(adjacencyFlag direction, GameObject neighbor)
    {
        hasNeightbors |= direction;//adds direction to flag

        switch (direction) //adds neighbor GO to array
        {
            case adjacencyFlag.North:
                neighborGO[0] = neighbor;
                break;
            case adjacencyFlag.East:
                neighborGO[1] = neighbor;
                break;
            case adjacencyFlag.South:
                neighborGO[2] = neighbor;
                break;
            case adjacencyFlag.West:
                neighborGO[3] = neighbor;
                break;
        }//end switch (direction)
    }//end AddNeigbor

    public void RemoveNeightbor(adjacencyFlag direction, GameObject neighbor)
    {
        hasNeightbors &= ~direction;//removed direction from flag
        switch (direction) //removes neighbor GO to array
        {
            case adjacencyFlag.North:
                neighborGO[0] = null;
                break;
            case adjacencyFlag.East:
                neighborGO[1] = null;
                break;
            case adjacencyFlag.South:
                neighborGO[2] = null;
                break;
            case adjacencyFlag.West:
                neighborGO[3] = null;
                break;

        }//end switch(direction)
    }//end remve neighbor

}
