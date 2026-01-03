//TODO/FIXME: This script and PlaceableObject are awfully similar. We should probably merge them eventually to simplify things

using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;
//using static UnityEditor.PlayerSettings;

public class ObjectDrag : MonoBehaviour
{
    bool dragging = true;
    public bool overRide;
    [SerializeField] GameObject overlapObject;
    [SerializeField] GameObject overlapTerrain;
    private string GOTag;//tag of the tile
    public TileMaterialHandler tileMaterialHandler;
    private static bool SoundCanBePlayed = false; //Should not call sound at beginning so we're not overwhelmed at startup
    private Vector3 previousGridPosition = new Vector3(0f, 0f, 0f);
    private Vector3 previousWorldPosition = new Vector3(0f, 0f, 0f);
    
   // private Vector2 previousPosition = new Vector2(0, 0);

    public void Awake()
    {
        GOTag = gameObject.tag;
        tileMaterialHandler = GetComponent<TileMaterialHandler>();
    }
    public void Start()
    {
        //Sets the previous position to the object's starting position
        Vector3 pos = BuildingSystem.GetMouseWorldPosition();
        Vector2 previousGridPosition = GridManager.GM.switchToGridIndexCoordinates(pos);
        previousWorldPosition = transform.position;


        Invoke("EnableSound", 1f);
    }
    public void OnDestroy()
    {
        SoundCanBePlayed = false;
    }

    public void Update()
    {   
        //makes tile follow mouse if dragging == true;
        if (!dragging) return;

        
        Vector3 pos = BuildingSystem.GetMouseWorldPosition();
        transform.position = BuildingSystem.current.SnapCoordinateToGrid(pos);
        Vector3 currentGridPosition = GridManager.GM.switchToGridIndexCoordinates(pos);
        if(previousGridPosition != currentGridPosition){
            //Debug.Log("moving tile");
            OnMoveTile(previousWorldPosition);
            previousGridPosition = currentGridPosition;
            previousWorldPosition = transform.position;
        }
        

        // Vector2 newSnappedPosition = GridManager.GM.switchToGridIndexCoordinates(pos);
        
        // if((previousPosition.x != newSnappedPosition.x) || (previousPosition.y != newSnappedPosition.y)){
        //     Debug.Log("Previous Position: " + previousPosition.x + ", " + previousPosition.y);
        //     Debug.Log("Current Position: " + newSnappedPosition.x + ", " + newSnappedPosition.y);
        //     Debug.Log("\n");
        //     transform.position = BuildingSystem.current.SnapCoordinateToGrid(pos);
        //     BuildingSystem.current.activeObjectMovedToNewTile(newSnappedPosition);
        //     previousPosition = new Vector2(newSnappedPosition.x, newSnappedPosition.y);
        // }
        
        // ALT: transform.position = BuildingSystem.current.SnapCoordinateToGrid(pos, terrain);
    }

    public void Place()
    {   
        gameObject.GetComponent<Tile>().tilePosition = BuildingSystem.current.SnapCoordinateToGrid(transform.position);//updates tile position of tile
        
        dragging = false;//stops object from following mouse
        transform.position = BuildingSystem.current.SnapCoordinateToGrid(transform.position);//locks object in grid
        this.GetComponent<Tile>().SetTileState(TileState.Static);//Non functional
        tileMaterialHandler.MaterialSet(TileMaterialHandler.matState.Placed);

        //FIXME - Update the system that plays the sound. FMOD is a struggle bus :(
        if (SoundCanBePlayed) { FMODUnity.RuntimeManager.PlayOneShot("event:/Tile" + this.GetComponent<Tile>().tileScriptableObject.thisTileClass); } //Gets Tileclass and plays corresponding FMOD event
        
        //Delete overlapping tiles (that can't overlap) on placement
        if(overlapObject != null && !AllowObjectOverlap(overlapObject)){
            overlapObject.GetComponent<Tile>().DeleteThisTile();
        }
        if(overlapTerrain != null && !AllowObjectOverlap(overlapTerrain)){
            overlapTerrain.GetComponent<Tile>().DeleteThisTile();
        }

        //Replaced code below with the two above if statements
        // if (overlapObject != null) {

        //     //remove object that this tile is overlapping.
        //     GridManager.GM.RemoveObject(overlapObject);
        //     Destroy(overlapObject);
        // }//the overlapping object is always destroyed
        // if (GOTag == "Ground")
        // {
        //     //remove overlap terrain from the tile gridmanager
        //     if(overlapTerrain != null){
        //         GridManager.GM.RemoveObject(overlapTerrain);
        //     }

        //     Destroy(overlapTerrain);//terrain is only destroyed when placing terrain
        // }
        //LevelManager.tileConnectionReset.Invoke();
        
        gameObject.GetComponent<Tile>().setInitialIncomeAndCarbon(); //Updates the initial net carbon and net income of tile.

        //visually updates road connections
        if(GetComponent<RoadConnections>() != null){
            GetComponent<RoadConnections>().UpdateModelConnections(true);
        } else{
            UpdateTileNeighborConnections();
        }

        //Updates residence connections
        if(GetComponent<RoadConnections>() != null || GetComponent<ResidentialBuilding>() != null){
            GridManager.GM.UpdateResidenceConnections(gameObject);

            
            // foreach(ResidentialBuilding residence in GridManager.GM.GetResidentialTiles()){

            //     //residence.UpdateResidenceConnections();
            // }
        }
        

        //Updates the cap on number of people
        if(TemporaryPeopleManager.TPM != null){
            TemporaryPeopleManager.TPM.UpdateMaxPeople();
        }


    }

    public void DestroyTile(){

        // Removes object from GridManager, 
        // so when the roads use the GridManager to update their connections, they will ignore this tile
        GridManager.GM.RemoveObject(gameObject);

        //Updates road connections of neighbors
        if(GetComponent<RoadConnections>() != null){
            GetComponent<RoadConnections>().UpdateNeighborConnections();
        }else{
            UpdateTileNeighborConnections();
        }

        //Updates residence/road connections
        if(GetComponent<RoadConnections>() != null || GetComponent<ResidentialBuilding>() != null){

            // //Deactivates all road connection graphics. They will be reenabled by the residence connection updates, if they're still connected to two houses.
            // foreach(RoadConnections road in GridManager.GM.GetRoadTiles()){
            //     road.deactivateConnectedRoad();
            // }

            // //Updates Connections of Each Residence
            // foreach(ResidentialBuilding residence in GridManager.GM.GetResidentialTiles()){
            //     residence.UpdateResidenceConnections();
            // }

            foreach(GameObject neighboringTile in GridManager.GM.GetRoadNeighbors(gameObject)){
                GridManager.GM.UpdateResidenceConnections(neighboringTile);
            }

            

        }
        
        
        //Updates the cap on number of people
        if(TemporaryPeopleManager.TPM != null){
            
            TemporaryPeopleManager.TPM.UpdateMaxPeople();
        }

        Destroy(gameObject);
    }

    //Updates the road connection graphics of any surrounding roads
    public void UpdateTileNeighborConnections(){
        GameObject[] neighborGameObjects = GridManager.GM.GetRoadNeighbors(gameObject);
        for(int i = 0; i < neighborGameObjects.Length; i++){
            GameObject _neighbor = neighborGameObjects[i];
            if(_neighbor != null && _neighbor.GetComponent<RoadConnections>() != null){
                _neighbor.GetComponent<RoadConnections>().UpdateModelConnections(false);
            }    
        }
    }

    public void Pickup()
    {
        dragging = true;
    }

    public void OnMoveTile(Vector3 previousPosition){

        //Resets variables when moved
        overRide = false; 
        overlapTerrain = null; 
        overlapObject = null;

        if (dragging)
        {
            

            GameObject[] otherObjectsInCell = GridManager.GM.GetGameObjectsInGridCell(this.gameObject);
            foreach(GameObject otherObject in otherObjectsInCell){
                //FIX ME / TODO / NOT DONE!!!!!!!!!
                //If we ever want to have multiple objects (More than 1 object & 1 terrain) in a GridCell, this code is toast!
                //You'll have to make the overlapTerrain and overlapObject variable arrays.
                string otherTag = otherObject.gameObject.tag;
                if (otherTag.Equals("Ground")) { overlapTerrain = otherObject.gameObject; }//checks if a Terrain tile is already where this is
                if (otherTag.Equals("Object")) { overlapObject = otherObject.gameObject; }//checks if a Object tile is already where this is
                if (otherObject.gameObject.tag == this.gameObject.tag) { overRide = true; }//checks if this tile will replace a tile that already exists
            }

            //visually updates road connections when dragging
            if(GetComponent<RoadConnections>() != null){
                //Updates connections of new surrounding road tiles
                GetComponent<RoadConnections>().UpdateModelConnections(true);

                //Updates connections of the surrounding road tiles just moved away from
                GameObject[] oldNeighbors = GridManager.GM.GetRoadNeighbors(previousPosition);
                foreach(GameObject oldNeighbor in oldNeighbors){
                    if(oldNeighbor != null && oldNeighbor.GetComponent<RoadConnections>() != null){
                        oldNeighbor.GetComponent<RoadConnections>().UpdateModelConnections(false);
                    }
                }
            }

            //Lets Building system know this object moved to a new tile
            BuildingSystem.current.activeObjectMovedToNewTile();


        }

        if (dragging) { updateTileMaterialValidity(); } //Checks if it can be placed at this location and updates material accordingly

        

    }


    //Deprecated Code replaced by the OnMoveTile() function.
    /*
    public void OnTriggerEnter(Collider other)
    {   
        //Debug.Log(this.gameObject.name+"hit"+other.gameObject.name);//Collison Debug (DO NOT FORGET RIGIDBODIES -AP


        if (dragging)
        {
            string otherTag = other.gameObject.tag;

            if (otherTag.Equals("Ground")) { overlapTerrain = other.gameObject; }//checks if a Terrain tile is already where this is
            if (otherTag.Equals("Object")) { overlapObject = other.gameObject; }//checks if a Object tile is already where this is
            if (other.gameObject.tag == this.gameObject.tag) { overRide = true; }//checks if this tile will replace a tile that already exists

            // if (IsValidOverlap() && BuildingSystem.current.CanBePlaced(GetComponent<PlaceableObject>()))//changes material based on if tile is overlapping a tile it can or not // also makes sure that it can be placed with Building SYstem Function
            // {
            //     tileMaterialHandler.MaterialSet(TileMaterialHandler.matState.HoveringValid);
            // }
            // else
            // {
            //     tileMaterialHandler.MaterialSet(TileMaterialHandler.matState.HoveringInvalid);
            // }

            updateTileMaterialValidity();
        }
            
    }

    public void OnTriggerExit(Collider other)
    {   
        //clear variables for temporary placement
            overRide = false; 
            overlapTerrain = null; 
            overlapObject = null;
        if (dragging)
        {
            updateTileMaterialValidity();
            // if (BuildingSystem.isObjectOverVoid()){
            //     tileMaterialHandler.MaterialSet(TileMaterialHandler.matState.HoveringInvalid); // Checks if object is over void
            // } else if(IsValidOverlap() && BuildingSystem.current.CanBePlaced(GetComponent<PlaceableObject>())){
                
            //     tileMaterialHandler.MaterialSet(TileMaterialHandler.matState.HoveringValid);
            // }
        }
    }
    */


    public void updateTileMaterialValidity(){
        if(GetComponent<PlaceableObject>().placed){
            tileMaterialHandler.MaterialSet(TileMaterialHandler.matState.Placed);
        }else{
           if ((!BuildingSystem.current.CanBePlaced(GetComponent<PlaceableObject>(), false)) && dragging)//changes material based on if it's somewhere it can be placed
            {
                tileMaterialHandler.MaterialSet(TileMaterialHandler.matState.HoveringInvalid);
            }
            else
            {
                tileMaterialHandler.MaterialSet(TileMaterialHandler.matState.HoveringValid);
            } 
        }
        
        
    }



    //Updated overlap function check
    public bool AllowObjectOverlap(GameObject otherTile){
        if(GetComponent<Tile>() != null && GetComponent<Tile>().tileScriptableObject != null && otherTile.GetComponent<Tile>() != null){
            
            TileScriptableObject[] validOverlaps = GetComponent<Tile>().tileScriptableObject.AllowOverlapList;
            TileScriptableObject otherTileScriptableObject = otherTile.GetComponent<Tile>().tileScriptableObject;
            foreach(TileScriptableObject validOverlap in validOverlaps){
                //Debug.Log("Other SCriptable= " + otherTileScriptableObject);
                //Debug.Log("My scriptable=  " + validOverlap);
                //Debug.Log("There are some valid overlaps");
                if(otherTileScriptableObject == validOverlap){
                    //Debug.Log("Allowing overlap");
                    return true;
                }
            }

            TileScriptableObject[] otherTileValidOverlaps = otherTile.GetComponent<Tile>().tileScriptableObject.AllowOverlapList;
            TileScriptableObject tileScriptableObject = GetComponent<Tile>().tileScriptableObject;
            foreach(TileScriptableObject validOverlap in otherTileValidOverlaps){
                if(tileScriptableObject == validOverlap){
                    //Debug.Log("Allowing overlap");
                    return true;
                }
            }
        }
        //Debug.Log("Denying overlap");
        return false;
    }


    /*
     * Ensures that the tile is not being placed over an invalid tile
     */
    public bool IsValidOverlap(GameObject otherTile)
    {   
        if(GOTag == "UI") { return true; }
        if(GOTag == "Ground") { return true; }//dont care FOR NOW if this is a terrain tile, or otherTile is a object, the object above gets deleted anyways
        if(otherTile == null) { return true;}//if object, check if there is a ground tile its overlapping
        if( otherTile.tag =="Object"){return true;}
        else
        {
            


            if (otherTile == null) return false;//if nothing to overlap, then object shouldnt be placed


            TileScriptableObject thisTSO = this.gameObject.GetComponent<Tile>().tileScriptableObject;
            TileScriptableObject otherTSO = otherTile.GetComponent<Tile>().tileScriptableObject;

            if (thisTSO.OverlapWhiteList.Length > 0)
            {
                if (Array.IndexOf(thisTSO.OverlapWhiteList, otherTSO.name) >= 0) { return true; }//if this has a whitelist and the overlap is on said whitelist, return true
                
                else { return false; }
            }
            
            if (Array.IndexOf(thisTSO.OverlapBlackList, otherTSO.name) >= 0)
            { //if the tile is in the blacklist, not valid
                return false;
            }
            else { return true; }//if otherTile is not in the blacklist, return true
        }
    }

    public bool IsValidOverlap()
    {
        if (IsValidOverlap(overlapTerrain) && IsValidOverlap(overlapObject))//if BOTH terrain and object is valid, it's valid
        {
            return true;
        }
        return false;
 
    }
    void EnableSound()
    {
        SoundCanBePlayed = true;
    }
}
