/* 
*   Description: Keeps track of each tile of a certain type.
*
*   How it works: It relies on a class called "TileTracker" (Attached to bottom of file).
*       For each type of tile it wants to track, you can create a new class that inherits from TileTracker.
*       Every time an object is placed, it runs CheckTileTrackersForRemoval("me!") and that runs the checks
*       in each inherited Tile Tracker Class to see if it should add it to its count. You can retrieve all
*       the tiles a Tile Tracker has counted by running ExampleTileTracker.GetAllTiles()
*
*   TO CREATE A NEW TILE TRACKER:
*       1) Declare a new one, like this: public TileTracker ExampleTileTracker { get; set; }
*       2) Create an instance of it in Start() & Add it to the TileTrackers array, like this:
*               ExampleTileTracker = new ExampleTileTracker();
*               TileTrackers.Add(ExampleTileTracker);
*       3) At the bottom of this file, add a new class for your tile tracker that inherits from TileTracker.
*           Then, add override methods for CheckTileForAddition() and CheckTileForRemoval() that define which tiles
*           should be added to your new tile tracker. Every time a new tile is placed, it will be checked with
*           those methods to determine if it should be added to your tracker.
*
*   NOTE: Try as best as you can to have as few of these tile trackers as possible because each of these adds
*       extra operations every single time a tile is placed. However, compared to the other war crimes on
*       efficiency my code is probably commiting, extra TileTrackers are likely not super significant.
*
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTypeCounter : MonoBehaviour
{
    public static TileTypeCounter current;


    //Only returns all of the tiles for a given type within the active chunk
    public List<TileTracker> TileTrackers { get; set; }
    public TileTracker AllTileTracker { get; set; }
    public TileTracker MoneyTileTracker { get; set; }
    public TileTracker CarbonTileTracker { get; set; }
    public TileTracker RoadTileTracker { get; set; }
    public TileTracker ResidenceTileTracker { get; set; }
    public TileTracker FactoryTileTracker { get; set; }
    public TileTracker WorkplaceTileTracker { get; set; }
    public TileTracker CarbonCaptureTileTracker { get; set; }
    public TileTracker ActivatableBuildingTileTracker { get; set; }
    public TileTracker ActivatableTileTracker { get; set; }


    //Test of certain tile trackers. KEEP IN CASE YOU NEED TO TEST NEW TILE TRACKERS
    void Update(){
        // if(Input.GetKeyDown(KeyCode.E)){
        //     int numberOfTiles = TileTrackers[1].GetAllTiles().Length; //GridManager.GM.GetAllTilesOnActiveChunk().Length;
        //     Debug.Log("Number of Residences: " + numberOfTiles);
        // }
        // if(Input.GetKeyDown(KeyCode.Y)){
        //     int numberOfTiles = TileTrackers[2].GetAllTiles().Length; //GridManager.GM.GetAllTilesOnActiveChunk().Length;
        //     Debug.Log("Number of Road: " + numberOfTiles);
        // }
        // if(Input.GetKeyDown(KeyCode.U)){
        //     int numberOfTiles = TileTrackers[3].GetAllTiles().Length; //GridManager.GM.GetAllTilesOnActiveChunk().Length;
        //     Debug.Log("Number of Factory: " + numberOfTiles);
        // }
        // if(Input.GetKeyDown(KeyCode.I)){
        //     int numberOfTiles = TileTrackers[4].GetAllTiles().Length; //GridManager.GM.GetAllTilesOnActiveChunk().Length;
        //     Debug.Log("Number of Carbon Capturerers: " + numberOfTiles);
        // }
        // if(Input.GetKeyDown(KeyCode.R)){
        //     int numberOfTiles = TileTrackers[0].GetAllTiles().Length; //GridManager.GM.GetAllTilesOnActiveChunk().Length;
        //     Debug.Log("Number of CarbonTiles: " + numberOfTiles);
        // }
    }

    void Awake()
    {
        if(current == null){
            current = this;
        }else{
            Destroy(this);
        }
    }

    void Start(){

        //Activates all the tile trackers, to keep track of the number of every tile type.
        TileTrackers = new List<TileTracker>();


        CarbonTileTracker = new CarbonTileTracker();
        TileTrackers.Add(CarbonTileTracker);

        ResidenceTileTracker = new ResidenceTileTracker();
        TileTrackers.Add(ResidenceTileTracker);

        RoadTileTracker = new RoadTileTracker();
        TileTrackers.Add(RoadTileTracker);

        FactoryTileTracker = new FactoryTileTracker();
        TileTrackers.Add(FactoryTileTracker);

        CarbonCaptureTileTracker = new CarbonCaptureTileTracker();
        TileTrackers.Add(CarbonCaptureTileTracker);
        
        ActivatableBuildingTileTracker = new ActivatableBuildingTileTracker();
        TileTrackers.Add(ActivatableBuildingTileTracker);

        ActivatableTileTracker = new ActivatableTileTracker();
        TileTrackers.Add(ActivatableTileTracker);

        WorkplaceTileTracker = new WorkplaceTileTracker();
        TileTrackers.Add(WorkplaceTileTracker);



        
    }

    public void CheckTileTrackersForRemoval(GameObject objectToCheck){
        foreach(TileTracker tileTracker in TileTrackers){
            if(tileTracker != null && objectToCheck != null && objectToCheck.GetComponent<Tile>() != null){
                tileTracker.CheckTileForRemoval(objectToCheck.GetComponent<Tile>());
            }
        }
    }

    public void CheckTileTrackersForAddition(GameObject objectToCheck){
        foreach(TileTracker tileTracker in TileTrackers){
            if(tileTracker != null && objectToCheck != null && objectToCheck.GetComponent<Tile>() != null){
                tileTracker.CheckTileForAddition(objectToCheck.GetComponent<Tile>());
            }
            
        }
    }

    public void ClearTileTrackers(){
        //Clears the previous list of tile trackers
        foreach(TileTracker tileTracker in TileTrackers){
            tileTracker.ClearAllTiles();
        }
    }

    public Tile[] GetAllActivatedBuildings(){
        Tile[] activatableBuildings = TileTypeCounter.current.ActivatableBuildingTileTracker.GetAllTiles();
        //Tracks all the buildings that are activated
        List<Tile> activatedBuildings = new List<Tile>();
        foreach(Tile activatableBuilding in activatableBuildings){
            //Checks if the tile is activated
            if(activatableBuilding is ActivatableTile activatableTile && activatableTile.IsActivated){
                activatedBuildings.Add(activatableBuilding);
            }
        }
        return activatedBuildings.ToArray();
    }

    public Tile[] GetAllActivatedRoads(){
        //Gets a list of all roads
        Tile[] roads = TileTypeCounter.current.RoadTileTracker.GetAllTiles();

        //Tracks all the roads that are activaed
        List<Tile> activatedRoads = new List<Tile>();
        foreach(Tile road in roads){
            //Checks if the tile is activated
            if(road is ActivatableTile activatableTile && activatableTile.IsActivated){
                activatedRoads.Add(road);
            }
        }
        return activatedRoads.ToArray();
    }


    // public void SwitchedGroundChunk(){

    //     //Clears the previous list of tile trackers
    //     foreach(TileTracker tileTracker in tileTrackers){
    //         tileTracker.Clear();
    //     }
        
    //     //Loads the new tile trackers
    //     TileTracker newChunkTileTrackers = GridDataLoader.currentGridChunk.tileTrackers;
    //     for(int i = 0; i < newChunkTileTrackers.Length; i++){
    //         tileTrackers.Add(newChunkTileTrackers[i]);
    //     }

    // }

}

//Base class for each type of Tile Tracker. The Tile Trackers keep the number of each kind of tile in the current Grid Chunk
public class TileTracker{

    // Override this bool to false for any tile trackers you don't want to be chunk-specific.
    // Whenever it's set to true, the given tile tracker will track the tiles for the active chunk only.
    public virtual bool resetThisTrackerOnChunkSwitch{
        get{
            return true;
        }
    }

    //Holds a list of all the tiles that the tracker should hold. 
    public List<Tile> trackedTileList = new List<Tile>();

    //Tile Tracker Constructor
    public TileTracker(){
        //Reset the tile tracker every time the player switches the current chunk
        if(resetThisTrackerOnChunkSwitch){
            GameEventManager.current.BeginSwitchingCurrentGroundChunk.AddListener(ResetTileTracker);
        }
    }


    protected void ResetTileTracker(){
        // Removes all tiles on chunk
        ClearAllTiles();

        // Goes through all the tiles on active chunk and checks them for addition to the tracker
        Tile[] allTilesOnActiveChunk = GridManager.GM.GetAllTilesOnActiveChunk();
        foreach(Tile tile in allTilesOnActiveChunk){
            if(tile != null)
                CheckTileForAddition(tile);
        }

    }

    protected void AddTile(Tile tile){
        trackedTileList.Add(tile);
    }
    
    protected void RemoveTile(Tile tile){
        trackedTileList.Remove(tile);
    }

    public void ClearAllTiles(){
        trackedTileList.Clear();
    }

    public virtual void CheckTileForRemoval(Tile tile){
        Debug.LogError("Base Tile Tracker Class has been accidentally implemented. You need to use one of its derived classes.");
    }

    public virtual void CheckTileForAddition(Tile tile){
        Debug.LogError("Base Tile Tracker Class has been accidentally implemented. You need to use one of its derived classes.");
    }

    public virtual Tile[] GetAllTiles(){
        Tile[] returnArray = new Tile[trackedTileList.Count];
        for(int i = 0; i < trackedTileList.Count; i++){
            returnArray[i] = trackedTileList[i];
        }
        return returnArray;
    }
}

public class AllTileTracker : TileTracker{
    public override void CheckTileForAddition(Tile tile){
        base.AddTile(tile);
    }
    public override void CheckTileForRemoval(Tile tile){
        base.RemoveTile(tile);
    }
}

public class CarbonTileTracker : TileTracker{
    public override void CheckTileForAddition(Tile tile){
        if(tile.tileScriptableObject.AnnualCarbonAdded != 0){
            base.AddTile(tile);
        }
    }
    public override void CheckTileForRemoval(Tile tile){
        if(tile.tileScriptableObject.AnnualCarbonAdded != 0){
            base.RemoveTile(tile);
        }
    }
}

public class MoneyTileTracker : TileTracker{
    public override void CheckTileForAddition(Tile tile){
        if(tile.tileScriptableObject.AnnualIncome > 0){
            base.AddTile(tile);
        }
    }
    public override void CheckTileForRemoval(Tile tile){
        if(tile.tileScriptableObject.AnnualIncome > 0){
            base.RemoveTile(tile);
        }
    }
}


public class RoadTileTracker : TileTracker{
    public override void CheckTileForAddition(Tile tile){
        if(tile is RoadTile road){
            base.AddTile(road);
        }
    }
    public override void CheckTileForRemoval(Tile tile){
        if(tile is RoadTile road){
            base.RemoveTile(road);
        }
    }
}

public class ResidenceTileTracker : TileTracker{
    public override void CheckTileForAddition(Tile tile){
        if(tile is ResidentialBuilding residence){
            base.AddTile(residence);
        }
    }
    public override void CheckTileForRemoval(Tile tile){
        if(tile is ResidentialBuilding residence){
            base.RemoveTile(residence);
        }
    }
}

public class FactoryTileTracker : TileTracker{
    public override void CheckTileForAddition(Tile tile){
        if(tile is FactoryTile factory){
            base.AddTile(factory);
        }
    }
    public override void CheckTileForRemoval(Tile tile){
        if(tile is FactoryTile factory){
            base.RemoveTile(factory);
        }
    }
}

public class WorkplaceTileTracker : TileTracker{

    //Keeps track of all the workplaces, not just the ones on the active chunk
    public override bool resetThisTrackerOnChunkSwitch{
        get{
            return false;
        }
    }

    public override void CheckTileForAddition(Tile tile){
        if(tile.tileScriptableObject != null && tile.tileScriptableObject.RequiredEmployees > 0){
            base.AddTile(tile);
        }
    }
    public override void CheckTileForRemoval(Tile tile){
        if(tile.tileScriptableObject != null && tile.tileScriptableObject.RequiredEmployees > 0){
            base.RemoveTile(tile);
        }
    }
}

public class CarbonCaptureTileTracker : TileTracker{
    public override void CheckTileForAddition(Tile tile){
        if(tile is CarbonCaptureTile carbonCapturer){
            base.AddTile(carbonCapturer);
        }
    }
    public override void CheckTileForRemoval(Tile tile){
        if(tile is CarbonCaptureTile carbonCapturer){
            base.RemoveTile(carbonCapturer);
        }
    }
}

public class ActivatableBuildingTileTracker : TileTracker{
    public override void CheckTileForAddition(Tile tile){
        if(tile is ActivatableBuilding activatableBuilding){
            base.AddTile(activatableBuilding);
        }
    }
    public override void CheckTileForRemoval(Tile tile){
        if(tile is ActivatableBuilding activatableBuilding){
            base.RemoveTile(activatableBuilding);
        }
    }
}

public class ActivatableTileTracker : TileTracker{
    public override void CheckTileForAddition(Tile tile){
        if(tile is ActivatableTile activatableTile){
            base.AddTile(activatableTile);
        }
    }
    public override void CheckTileForRemoval(Tile tile){
        if(tile is ActivatableTile activatableTile){
            base.RemoveTile(activatableTile);
        }
    }
}

