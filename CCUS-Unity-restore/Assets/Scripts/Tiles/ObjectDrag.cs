using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;
//using static UnityEditor.PlayerSettings;

public class ObjectDrag : MonoBehaviour
{
    public bool placed;
    bool dragging = true;
    public bool overRide;
    [SerializeField] GameObject overlapObject;
    [SerializeField] GameObject overlapTerrain;
    private string GOTag;//tag of the tile
    public TileMaterialHandler tileMaterialHandler;
    private static bool SoundCanBePlayed = false; //Should not call sound at beginning so we're not overwhelmed at startup
    public Vector2Int PreviousGridPosition {get; set;} = new Vector2Int(0, 0);
    private Vector3 previousWorldPosition = new Vector3(0f, 0f, 0f);

    //Used to turn off mesh
    private TileMeshLoader myMeshLoader;
    

    public void Awake()
    {
        GOTag = gameObject.tag;
        tileMaterialHandler = GetComponent<TileMaterialHandler>();

        myMeshLoader = GetComponent<TileMeshLoader>();


        previousWorldPosition = transform.position;

        
    }
    public void Start()
    {

        //Sets the previous position to the object's starting position
        Vector3 pos = BuildingSystem.GetMouseWorldPosition();
        PreviousGridPosition = GridManager.GM.SwitchToGridCoordinates(pos);
    }

    public void Update()
    {   
        //makes tile follow mouse if dragging == true;
        if (!dragging) return;

        //Disables the mesh when dragging, if in touch mode & finger not held down
        if(TouchModeHandler.current.IsInTouchMode && Input.touchCount == 0){
            if(myMeshLoader != null){
                myMeshLoader.UnloadTileMesh();
            }
        } else{
            if(myMeshLoader != null){
                myMeshLoader.LoadTileMesh();
            }
        }

        //NOTE: OnMoveTile call must go last. Otherwise, extra code gets called once OnMoveTileCompletes, which can be after another tile is placed.
        Vector3 pos = BuildingSystem.GetMouseWorldPosition();
        transform.position = BuildingSystem.current.SnapCoordinateToGrid(pos);
        Vector2Int currentGridPosition = GridManager.GM.SwitchToGridCoordinates(pos);
        if(PreviousGridPosition != currentGridPosition){
            //Debug.Log("Moving Tile From: " + PreviousGridPosition + " to " + currentGridPosition);
            OnMoveTile(previousWorldPosition);
            PreviousGridPosition = currentGridPosition;
            previousWorldPosition = transform.position;
        }

        
        
    }

    public void Place()
    {   

        //Puts object in the Grid Manager
        GridManager.GM.AddObject(gameObject, false);
        GetComponent<Tile>().tilePosition = BuildingSystem.current.SnapCoordinateToGrid(transform.position);//updates tile position of tile
        GetComponent<Tile>().SetTileState(TileState.Static);//Non functional (I think)

        dragging = false;//stops object from following mouse
        transform.position = BuildingSystem.current.SnapCoordinateToGrid(transform.position);//locks object in grid
        tileMaterialHandler.MaterialSet(TileMaterialHandler.matState.Placed);

    
        //Delete overlapping tiles (that can't overlap) on placement
        if(overlapObject != null && !AllowObjectOverlap(overlapObject)){
            overlapObject.GetComponent<Tile>().DeleteThisTile();
        }
        if(overlapTerrain != null && !AllowObjectOverlap(overlapTerrain)){
            overlapTerrain.GetComponent<Tile>().DeleteThisTile();
        }
        
        


        //Alerts the Tile script that this tile was just placed
        if(this.GetComponent<Tile>() != null) this.GetComponent<Tile>().ThisTileJustPlaced();

        //Makes sure the mesh is activated
        if(myMeshLoader != null){
           myMeshLoader.LoadTileMesh(); 
        }

        placed = true;


    }

    //Used to load data chunks
    // public void LoadedTile(){
    //     GetComponent<Tile>().SetTileState(TileState.Static);
    //     placed = true;
    //     dragging = false;
    //     tileMaterialHandler.MaterialSet(TileMaterialHandler.matState.Placed);
    // }



    private void OnMoveTile(Vector3 previousPosition){

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




    public void updateTileMaterialValidity(){
        if(GetComponent<ObjectDrag>().placed){
            tileMaterialHandler.MaterialSet(TileMaterialHandler.matState.Placed);
        }else{


        //Turns object red if it cannot be placed in position
            if(!BuildingSystem.current.ShouldActiveTileMaterialBeValid()){
                tileMaterialHandler.MaterialSet(TileMaterialHandler.matState.HoveringInvalid);
            }else {
                tileMaterialHandler.MaterialSet(TileMaterialHandler.matState.HoveringValid);
            } 
        }

        
    }



    //Updated overlap function check
    public bool AllowObjectOverlap(GameObject otherTileObject){
        if(otherTileObject == null){ return true; }
        //return true;

        Tile myTile = GetComponent<Tile>();
        Tile otherTile = otherTileObject.GetComponent<Tile>();


        if(myTile != null && myTile.tileScriptableObject != null && otherTile != null && otherTile.tileScriptableObject != null){

            TileScriptableObject myScriptable = myTile.tileScriptableObject;
            TileScriptableObject otherScriptable = otherTile.tileScriptableObject;

            // if(myScriptable.isTerrain && otherScriptable.isTerrain){
            //     return false;
            // }

            //Allows placeable objects (not terrain) to overlap with grass
            //This tile isn't terrain
            if(!myScriptable.isTerrain){

                //Other tile is grass
                if(otherScriptable.isTerrain && otherScriptable.AnnualCarbonAdded < 0){
                    return true;
                }                
                
            }
            //This tile is grass
            if(myScriptable.isTerrain && myScriptable.AnnualCarbonAdded < 0){

                //Other tile isn't terrain
                if(!otherScriptable.isTerrain){
                    return true;
                }
            }
            
            // //Overlap is valid if the overlapping object is the same type as this object
            // if(GetComponent<Tile>().tileScriptableObject == otherTile.GetComponent<Tile>().tileScriptableObject){
            //     return true;
            // }

            // //Checks this tile's overlap list to see if it's a valid overlap
            // TileScriptableObject[] validOverlaps = GetComponent<Tile>().tileScriptableObject.AllowOverlapList;
            // TileScriptableObject otherTileScriptableObject = otherTile.GetComponent<Tile>().tileScriptableObject;
            // foreach(TileScriptableObject validOverlap in validOverlaps){
            //     if(otherTileScriptableObject == validOverlap){
            //         return true;
            //     }
            // }

            // //Checks overlapping tile's overlap list to see if this is a valid overlap
            // TileScriptableObject[] otherTileValidOverlaps = otherTile.GetComponent<Tile>().tileScriptableObject.AllowOverlapList;
            // TileScriptableObject tileScriptableObject = GetComponent<Tile>().tileScriptableObject;
            // foreach(TileScriptableObject validOverlap in otherTileValidOverlaps){
            //     if(tileScriptableObject == validOverlap){
            //         return true;
            //     }
            // }
        }
        //Debug.Log("Denying overlap");
        return false;
    }


    //Decides when a tile can destroy a tile it will be placed on top of
    public bool CanDestroyOverlappingTile(GameObject otherTile)
    {   
        if(otherTile == null){
            return true;
        }

        //Roads can destroy grass, and trees can destroy saplings, but nothing else can destroy each other
        Tile thisTile = GetComponent<Tile>();
        Tile otherTileScript = otherTile.GetComponent<Tile>();
        if(!thisTile.tileScriptableObject.isTerrain){ // If this tile is a placeable object, can't destroy other tiles
            //Trees can destroy each other.
            if(thisTile.tileScriptableObject.isTree && otherTileScript.tileScriptableObject.isTree){
                return true;
            }
            

        }else{ //If this tile is a terrain tile, it can destroy other terrain tiles
            if(otherTile != null){

                
                if(otherTileScript.tileScriptableObject.isTerrain){

                    //Terrain that removes carbon can't destroy terrain that doesn't remove carbon
                    // IOW, roads can destroy grass, grass can't destroy roads
                    if(otherTileScript.tileScriptableObject.AnnualCarbonAdded >= 0 && thisTile.tileScriptableObject.AnnualCarbonAdded < 0){
                        return false;
                    }else{
                        return true;
                    }
                    
                }
            }
        }

        return false;

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
    //Invalid overlapping tiles will be destroyed once Place() is called
    public bool CanBePlacedOnOverlappingTile()
    {
        if(IsValidOverlap()){
            return true;
        }

        if ((CanDestroyOverlappingTile(overlapTerrain) || AllowObjectOverlap(overlapTerrain)) && (CanDestroyOverlappingTile(overlapObject) || AllowObjectOverlap(overlapObject)))
        {
            if(AllowObjectOverlap(overlapObject) && overlapObject != null){
                Debug.Log(GetComponent<Tile>().tileScriptableObject.Name + " can overlap with " + overlapObject.GetComponent<Tile>().tileScriptableObject.Name);
            }
            return true;
        }
        return false;
 
    }
}
