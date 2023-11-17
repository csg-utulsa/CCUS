using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectedTileHandler : MonoBehaviour
{
    [Header("Connected Tile Scriptable Object")]
    public ConnectedTileScriptableObject baseModels;
    [Header("Current Model State")]
    public Mesh currentModel;
    public float rotation; //How much the base model should be rotated to fit the adjacency

    [Header("Current Adjacecy")]
    public bool xPlusAdjacent; //Tile has a similair tile to the north
    public bool zMinusAdjacent; //Tile has a similair tile to the east
    public bool xMinusAdjacent; //Tile has a similair tile to the south
    public bool zPlusAdjacent; //Tile has a similair tile to the west

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeModelOnAdjacency()
    {   /**
         * Got ourselves a big old if-else tree here, choosing base model and rotation nased on how many similair tiles are adjacent to the current tile
         * If anyone has a more efficient way of checking this, be my guest
         * -Aidan Pohl
         */
        if (xPlusAdjacent)//Similair tile to the North
        {
            if (zPlusAdjacent)
            {//North (yes) West (yes)
                if (xMinusAdjacent)
                {//North (yes) West (yes) South (yes)
                    if (zMinusAdjacent)
                    {//North (yes) West (yes) South (Yes) East (Yes)
                        currentModel = baseModels.xIntersectionModel;//Get the Full X Intersection Model
                        rotation = 0f;
                    }else
                    {//North (yes) West (yes) South (Yes) East (No)
                        currentModel = baseModels.tIntersectionModel;//Get a T Intersection Facing to the West
                        rotation = -90f;
                    }
                }
                else
                {//North (yes) West (yes) South (No)
                    if(zMinusAdjacent)
                    {//North (yes) West(Yes) South (No) East(Yes)
                        currentModel = baseModels.tIntersectionModel;//Get a T Intersection Facing to the North
                        rotation = 0f;
                    }
                    else
                    {//North (yes) West (yes) South (No) East (No)
                        currentModel = baseModels.cornerModel;// Get a corner facing North and West
                        rotation = 0f;
                    }
                }
            }
            else
            {//North (yes) West (no)
                if (xMinusAdjacent)
                {//North (yes) West (no) South (yes)
                    if (zMinusAdjacent)
                    {//North (Yes) West (No) South (Yes) East (Yes)
                        currentModel = baseModels.tIntersectionModel;//Get a T Intersection Facing to the East
                        rotation = 90f;
                    } else
                    {//North (Yes) West (No) South (Yes) East (No)
                        currentModel = baseModels.straightModel; //Get a Straight Path running North and South
                        rotation = 0f;
                    }
                }
                else
                {//North (Yes) West (No) South (No)
                    if (zMinusAdjacent)
                    {//North (Yes) West (No) South (No) East (Yes)
                        currentModel = baseModels.cornerModel;//Get a Corner Path facing North and East
                        rotation = 90f;
                    }
                    else
                    {//North (Yes) West (No) South (No) East (No)
                        currentModel = baseModels.endModel;// Get an Dead End facing North
                        rotation = 0f;
                    }//end else
                }//end else
            }//end else
        }
        else
        { //North (No)
            if (zPlusAdjacent)
            {//North (No) West (yes)
                if (xMinusAdjacent)
                {//North (No) West (yes) South (yes)
                    if (zMinusAdjacent)
                    {//North (No) West (yes) South (Yes) East (Yes)
                        currentModel = baseModels.tIntersectionModel;//Get a T Intersection Facing to the South
                        rotation = 180f;
                    }
                    else
                    {//North (No) West (yes) South (Yes) East (No)
                        currentModel = baseModels.cornerModel;//Get a Corner Facing to the West and South
                        rotation = -90f;
                    }
                }
                else
                {//North (No) West (yes) South (No)
                    if (zMinusAdjacent)
                    {//North (No) West(Yes) South (No) East(Yes)
                        currentModel = baseModels.straightModel;//Get a Straight tile running East and West
                        rotation = 90f;
                    }
                    else
                    {//North (No) West (yes) South (No) East (No)
                        currentModel = baseModels.endModel;// Get a Dead End facing West
                        rotation = -90f;
                    }
                }
            }
            else
            {//North (no) West (no)
                if (xMinusAdjacent)
                {//North (no) West (no) South (yes)
                    if (zMinusAdjacent)
                    {//North (no) West (No) South (Yes) East (Yes)
                        currentModel = baseModels.cornerModel;//Get a Corner tile facing South and East
                        rotation = 180f;
                    }
                    else
                    {//North (no) West (No) South (Yes) East (No)
                        currentModel = baseModels.endModel; //Get a Dead End facing South
                        rotation = 180f;
                    }
                }
                else
                {//North (no) West (No) South (No)
                    if (zMinusAdjacent)
                    {//North (No) West (No) South (No) East (Yes)
                        currentModel = baseModels.endModel;//Get a Dead End facing East
                        rotation = 90f;
                    }
                    else
                    {//North (No) West (No) South (No) East (No)
                        currentModel = baseModels.islandModel;// Get a lonely Island
                        rotation = 0f;
                    }//end else
                }//end else
            }//end else


        }//End of the Big If-Else tree


    }

}
