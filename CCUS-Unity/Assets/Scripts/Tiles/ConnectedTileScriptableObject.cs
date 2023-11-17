using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Created By: Aidan Pohl
 * Created: Nov 16,2023
 * 
 * Decription: Scriptible object to store the base assets for a type of connected Tile
 */
[CreateAssetMenu(fileName = "ConnectedModels", menuName = "Asset/Connected Models")]
public class ConnectedTileScriptableObject : ScriptableObject
{
    [Header("Base Models")]
    public Mesh straightModel;//default should be X directional
    public Mesh cornerModel;//Default Should be X+ and Z+ openings
    public Mesh tIntersectionModel; //Default should be X+, Z+ and Z- Opening
    public Mesh xIntersectionModel; //Default shouldnt matter really
    public Mesh endModel; //Default should have X+ opening
    public Mesh islandModel; //An Isolated Tile
}


