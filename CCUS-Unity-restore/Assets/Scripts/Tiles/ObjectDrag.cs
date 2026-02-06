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
    public Vector2Int PreviousGridPosition {get; set;} = new Vector2Int(0, 0);
    private Vector3 previousWorldPosition = new Vector3(0f, 0f, 0f);
    
   // private Vector2 previousPosition = new Vector2(0, 0);

    public void Awake()
    {
        GOTag = gameObject.tag;
        tileMaterialHandler = GetComponent<TileMaterialHandler>();


        previousWorldPosition = transform.position;

        
    }
    public void Start()
    {

        //Sets the previous position to the object's starting position
        Vector3 pos = BuildingSystem.GetMouseWorldPosition();
        PreviousGridPosition = GridManager.GM.SwitchToGridCoordinates(pos);

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
        Vector2Int currentGridPosition = GridManager.GM.SwitchToGridCoordinates(pos);
        if(PreviousGridPosition != currentGridPosition){
            //Debug.Log("Moving Tile From: " + PreviousGridPosition + " to " + currentGridPosition);
            OnMoveTile(previousWorldPosition);
            PreviousGridPosition = currentGridPosition;
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
        //Puts object in the Grid Manager
        GridManager.GM.AddObject(gameObject, false);
        GetComponent<Tile>().tilePosition = BuildingSystem.current.SnapCoordinateToGrid(transform.position);//updates tile position of tile
        GetComponent<Tile>().SetTileState(TileState.Static);//Non functional

        //gameObject.GetComponent<Tile>().tilePosition = BuildingSystem.current.SnapCoordinateToGrid(transform.position);//updates tile position of tile
        dragging = false;//stops object from following mouse
        transform.position = BuildingSystem.current.SnapCoordinateToGrid(transform.position);//locks object in grid
        //this.GetComponent<Tile>().SetTileState(TileState.Static);//Non functional
        tileMaterialHandler.MaterialSet(TileMaterialHandler.matState.Placed);

        //FIXME - Update the system that plays the sound. FMOD is causing struggles
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
        
        


        //Alerts the Tile script that this tile was just placed
        if(this.GetComponent<Tile>() != null) this.GetComponent<Tile>().ThisTileJustPlaced();


    }

    //Used to load data chunks
    public void LoadedTile(){
        GetComponent<Tile>().SetTileState(TileState.Static);
        GetComponent<PlaceableObject>().placed = true;
        dragging = false;
        tileMaterialHandler.MaterialSet(TileMaterialHandler.matState.Placed);
    }

    public void DestroyTile(){

        //Alerts the Tile script that this tile is about to be Destroyed
        if(this.GetComponent<Tile>() != null) this.GetComponent<Tile>().ThisTileAboutToBeDestroyed();

        Destroy(gameObject);

        GameEventManager.current.TileJustDestroyed.Invoke();
    }

    //TODO Move to different script
    //Updates the road connection graphics of any surrounding roads.
    public void UpdateTileNeighborConnections(){
        GameObject[] neighborGameObjects = RoadAndResidenceConnectionManager.current.GetRoadNeighbors(gameObject);
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
                //If we ever want to have multiple objects (More than 1 object & 1 terrain) in a GridCell, this code is toast!
                //You'll have to make the overlapTerrain and overlapObject Lists.
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
                GameObject[] oldNeighbors = RoadAndResidenceConnectionManager.current.GetRoadNeighbors(previousPosition);
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
        //    if ((!BuildingSystem.current.CanBePlaced(GetComponent<PlaceableObject>(), false)))//changes material based on if it's somewhere it can be placed
        //     {
        //         tileMaterialHandler.MaterialSet(TileMaterialHandler.matState.HoveringValid);
        //     }

        //Turns object red if it cannot be placed in position
            //if(!CanBePlacedOnOverlappingTile()){
            if(!BuildingSystem.current.ShouldActiveTileMaterialBeValid()){
                tileMaterialHandler.MaterialSet(TileMaterialHandler.matState.HoveringInvalid);
            }else {
                tileMaterialHandler.MaterialSet(TileMaterialHandler.matState.HoveringValid);
            } 
        }

        
    }



    //Updated overlap function check
    public bool AllowObjectOverlap(GameObject otherTile){
        if(otherTile == null) return true;
        if(GetComponent<Tile>() != null && GetComponent<Tile>().tileScriptableObject != null && otherTile.GetComponent<Tile>() != null){
            
            //Overlap is valid if the overlapping object is the same type as this object
            if(GetComponent<Tile>().tileScriptableObject == otherTile.GetComponent<Tile>().tileScriptableObject){
                return true;
            }

            //Checks this tile's overlap list to see if it's a valid overlap
            TileScriptableObject[] validOverlaps = GetComponent<Tile>().tileScriptableObject.AllowOverlapList;
            TileScriptableObject otherTileScriptableObject = otherTile.GetComponent<Tile>().tileScriptableObject;
            foreach(TileScriptableObject validOverlap in validOverlaps){
                if(otherTileScriptableObject == validOverlap){
                    return true;
                }
            }

            //Checks overlapping tile's overlap list to see if this is a valid overlap
            TileScriptableObject[] otherTileValidOverlaps = otherTile.GetComponent<Tile>().tileScriptableObject.AllowOverlapList;
            TileScriptableObject tileScriptableObject = GetComponent<Tile>().tileScriptableObject;
            foreach(TileScriptableObject validOverlap in otherTileValidOverlaps){
                if(tileScriptableObject == validOverlap){
                    return true;
                }
            }
        }
        //Debug.Log("Denying overlap");
        return false;
    }


    //Decides when a tile can destroy a tile it will be placed on top of
    public bool CanBePlacedOnOverlappingTile(GameObject otherTile)
    {   
        if(otherTile == null){
            return true;
        }

        //Terrain can be placed onto and destroy other terrain, but nothing else can destroy each other
        Tile thisTile = GetComponent<Tile>();
        Tile otherTileScript = otherTile.GetComponent<Tile>();
        if(!thisTile.tileScriptableObject.isTerrain){ // If this tile is a placeable object, can't destroy other tiles
            return false;

        }else{ //If this tile is a terrain tile, it can destroy other terrain tiles
            if(otherTile != null){

                
                if(otherTileScript.tileScriptableObject.isTerrain){

                    //Terrain that removes carbon can't destroy terrain that doesn't remove carbon
                    if(otherTileScript.tileScriptableObject.AnnualCarbonAdded >= 0 && thisTile.tileScriptableObject.AnnualCarbonAdded < 0){
                        return false;
                    }else{
                        return true;
                    }
                    
                }
            }
        }

        return false;



        // if(GOTag == "UI") { return true; }
        // if(GOTag == "Ground") { return true; }//dont care FOR NOW if this is a terrain tile, or otherTile is a object, the object above gets deleted anyways
        // if(otherTile == null) { return true;}//if object, check if there is a ground tile its overlapping
        // if( otherTile.tag == "Object"){return true;}
        // else
        // {
            


        //     // if (otherTile == null) return false;//if nothing to overlap, then object shouldnt be placed


        //     // TileScriptableObject thisTSO = this.gameObject.GetComponent<Tile>().tileScriptableObject;
        //     // TileScriptableObject otherTSO = otherTile.GetComponent<Tile>().tileScriptableObject;

        //     // if (thisTSO.OverlapWhiteList.Length > 0)
        //     // {
        //     //     if (Array.IndexOf(thisTSO.OverlapWhiteList, otherTSO.name) >= 0) { return true; }//if this has a whitelist and the overlap is on said whitelist, return true
                
        //     //     else { return false; }
        //     // }
            
        //     // if (Array.IndexOf(thisTSO.OverlapBlackList, otherTSO.name) >= 0)
        //     // { //if the tile is in the blacklist, not valid
        //     //     return false;
        //     // }
        //     // else { return true; }//if otherTile is not in the blacklist, return true
        // }
    }

    public bool IsValidOverlap()
    {
        
        

        if (AllowObjectOverlap(overlapTerrain) && AllowObjectOverlap(overlapObject))//if BOTH terrain and object is valid, it's valid
        {
            return true;
        }
        return false;
 
    }
    

    //Checks if this tile can be placed on the overlapping tiles, even if the overlap is invalid
    //Will destroy invalid overlapping tiles
    public bool CanBePlacedOnOverlappingTile()
    {
        if(IsValidOverlap()){
            return true;
        }

        if (CanBePlacedOnOverlappingTile(overlapTerrain) && CanBePlacedOnOverlappingTile(overlapObject))
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
