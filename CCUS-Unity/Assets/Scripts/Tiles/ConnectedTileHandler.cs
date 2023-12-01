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


    [Header("Current Adjacecy")]
    public AdjacencyFlag hasNeighbors = AdjacencyFlag.None;//handles current neighbors
    public GameObject[] neighborGO = new GameObject[4];

    private (AdjacencyFlag direction, GameObject neighbor) tempNeighbor;//to handle floating neighbors
    
    private (Mesh model, float rotation)[] modelList;
    private void Awake()
    {
        modelList = new(Mesh model, float rotation)[]  {
            
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
            (baseModels.xIntersectionModel, 0 + baseModels.xRot),           // [15] X intersection Model,   facing North, South, East and West,
        };


        if (currentModel == null)
        {
            currentModel = modelList[0].model;
        }
        if (TileModelGO == null)
            {
                TileModelGO = transform.Find("Model").gameObject;
            }
        
        
    }
    // Start is called before the first frame update
    void Update()
    {
        //Debug.Log(tempNeighbor.neighbor);
        //Debug.Log(neighborGO[0]+" "+neighborGO[1]+" "+neighborGO[2]+" "+neighborGO[3]);
        if ((tempNeighbor.neighbor != null) && (tempNeighbor.neighbor.GetComponent<PlaceableObject>().placed))
        {
            
            AddNeighbor(tempNeighbor.direction, tempNeighbor.neighbor);
            RemoveTempNeighbor();
        }
        
        UpdateModel();
    }
    public void AddTempNeighbor(AdjacencyFlag tempDir, GameObject tempGO)
    {
        Debug.Log("Adding Temp Neighbor: "+tempGO.name);
        tempNeighbor.direction = tempDir;
        tempNeighbor.neighbor = tempGO;
        Debug.Log("Temp Direction ="+tempNeighbor.direction);
    }
    public void AddNeighbor(AdjacencyFlag direction, GameObject neighbor)
    {
        if (!neighbor.GetComponent<PlaceableObject>().placed)
        {
            AddTempNeighbor(direction, neighbor);
        }
        else
        {
            Debug.Log("Adding neighbor");
            hasNeighbors |= direction;//adds direction to flag

            switch (direction) //adds neighbor GO to array
            {
                case AdjacencyFlag.North:
                    neighborGO[0] = neighbor;
                    break;
                case AdjacencyFlag.East:
                    neighborGO[1] = neighbor;
                    break;
                case AdjacencyFlag.South:
                    neighborGO[2] = neighbor;
                    break;
                case AdjacencyFlag.West:
                    neighborGO[3] = neighbor;
                    break;
            }//end switch (direction)
        }
        UpdateModel();
    }//end AddNeigbor

    public void RemoveTempNeighbor()
    {
        Debug.Log("Removing Temp Neightbor");
        tempNeighbor.direction = AdjacencyFlag.None;
        tempNeighbor.neighbor = null;
    }

    public void RemoveNeighbor(AdjacencyFlag direction, GameObject neighbor)
    {
        if (neighbor == tempNeighbor.neighbor)
        {
            RemoveTempNeighbor();
        }
        else
        {
            Debug.Log("Removing Neighbor");
            hasNeighbors &= ~direction;//removed direction from flag
            switch (direction) //removes neighbor GO to array
            {
                case AdjacencyFlag.North:
                    neighborGO[0] = null;
                    break;
                case AdjacencyFlag.East:
                    neighborGO[1] = null;
                    break;
                case AdjacencyFlag.South:
                    neighborGO[2] = null;
                    break;
                case AdjacencyFlag.West:
                    neighborGO[3] = null;
                    break;
            }//end switch(direction)
            
        }

        UpdateModel();
    }//end remve neighbor

    public void UpdateModel()
    {
        AdjacencyFlag currentNeighbors = hasNeighbors | tempNeighbor.direction;
        //Debug.Log(currentNeighbors);
        currentModel = modelList[(int)currentNeighbors].model;
        TileModelGO.GetComponent<MeshFilter>().mesh = currentModel;
        TileModelGO.transform.localEulerAngles = new Vector3(-90,modelList[(int)currentNeighbors].rotation,0);

    }
}

[Flags] public enum AdjacencyFlag{
    None = 0,
    North = 1,
    East = 2,
    South = 4,
    West = 8,
    DONOTTOUCH = 99,
}

