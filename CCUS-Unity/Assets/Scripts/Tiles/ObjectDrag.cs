using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;

public class ObjectDrag : MonoBehaviour
{
    bool dragging = true;
    public bool overRide;
    [SerializeField] GameObject overlapObject;
    [SerializeField] GameObject overlapTerrain;
    private string GOTag;//tag of the tile
    private TileMaterialHandler tileMaterialHandler;
    

    public void Awake()
    {
        GOTag = gameObject.tag;
        tileMaterialHandler = GetComponent<TileMaterialHandler>();
    }

    public void Update()
    {
        if (!dragging) return;
        Vector3 pos = BuildingSystem.GetMouseWorldPosition();
        transform.position = BuildingSystem.current.SnapCoordinateToGrid(pos);
        // ALT: transform.position = BuildingSystem.current.SnapCoordinateToGrid(pos, terrain);
    }

    public void Place()
    {
        dragging = false;
        this.GetComponent<Tile>().SetTileState(TileState.Static);
        tileMaterialHandler.MaterialSet(TileMaterialHandler.matState.Placed);

        if(overlapObject != null) {Destroy(overlapObject);}//the overlapping object is always destroyed
        if (GOTag == "Ground")
                Destroy(overlapTerrain);//terrain is only destroyed when placing terrain
    }

    public void Pickup()
    {
        dragging = true;
    }

    public void OnTriggerEnter(Collider other)
    {   
        //Debug.Log(this.gameObject.name+"hit"+other.gameObject.name);//Collison Debug (DO NOT FORGET RIGIDBODIES -AP


        if (dragging)
        {
            string otherTag = other.gameObject.tag;

            if (otherTag.Equals("Ground")) { overlapTerrain = other.gameObject; }//checks if a Terrain tile is already where this is
            if (otherTag.Equals("Object")) { overlapObject = other.gameObject; }//checks if a Object tile is already where this is
            if (other.gameObject.tag == this.gameObject.tag) { overRide = true; }//checks if this tile will replace a tile that already exists

            if (IsValidOverlap())//changes material based on if tile is overlapping a tile it can or not
            {
                tileMaterialHandler.MaterialSet(TileMaterialHandler.matState.HoveringValid);
            }
            else
            {
                tileMaterialHandler.MaterialSet(TileMaterialHandler.matState.HoveringInvalid);
            }
        }
            
    }

    public void OnTriggerExit(Collider other)
    {
            overRide = false; 
            overlapTerrain = null; 
            overlapObject = null;
        if (dragging)
        {
            tileMaterialHandler.MaterialSet(TileMaterialHandler.matState.HoveringValid);
        }
    }
    
    /*
     * Ensures that the tile is not being placed over an invalid tile
     */
    public bool IsValidOverlap(GameObject otherTile)
    {
        if(GOTag == "Ground") { return true; }//dont care FOR NOW if this is a terrain tile, or otherTile is a object, the object above gets deleted anyways
        if(otherTile == null) { return true;}//if object, check if there is a ground tile its overlapping
        if( otherTile.tag =="Object"){return true;}
        else
        {
            if (otherTile == null) return false;//if nothing to overlap, then object shouldnt be placed


            TileScriptableObject thisTSO = this.gameObject.GetComponent<Tile>().tileScriptableObject;
            TileScriptableObject otherTSO = otherTile.GetComponent<Tile>().tileScriptableObject;

            if (otherTSO.OverlapWhiteList.Length > 0 && Array.IndexOf(otherTSO.OverlapWhiteList, otherTSO.name) >= 0) { return true; }//if this has a whitelist and the overlap is on said whitelist, return true

            if (Array.IndexOf(thisTSO.OverlapBlackList, otherTSO.name) >= 0)
            { //if the tile is in the blacklist, not valid
                return false;
            }
            else { return true; }//if otherTile is not in the blacklist, return true
        }
    }

    public bool IsValidOverlap()
    {
        if (IsValidOverlap(overlapTerrain) && IsValidOverlap(overlapObject))//if BOTH terrain and object is valid, its valid
        {
            Debug.Log("Valid placement for " + this.gameObject.name);
            return true;
        }
        Debug.Log("Invalid placement for " + this.gameObject.name);
        return false;
 
    }
}
