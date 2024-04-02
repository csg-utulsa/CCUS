using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tile", menuName = "Asset/Tile")]
public class TileScriptableObject : ScriptableObject
{
    public string Name;
    public int BuildCost;
    public int AnnualCost;
    public int AnnualCarbonRemoved;
    public int AnnualCarbonAdded;
    public int AnnualCarbonStored;
    public int AnnualIncome;
    public enum TileClass
    {
        Grass, Dirt, Pavement, Tree, Water, Machine, Building
    }

    public TileClass thisTileClass;

    public string[] OverlapBlackList;

    public string[] OverlapWhiteList;

    public string FlavorText;
}
