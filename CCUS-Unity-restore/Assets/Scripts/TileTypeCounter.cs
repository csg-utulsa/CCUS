//Keeps track of each tile of a certain type

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTypeCounter : MonoBehaviour
{
    public static TileTypeCounter current;


    //Only returns all of the tiles for a given type within the active chunk
    public TileTracker[] TileTrackers { get; set; }
    public TileTracker AllTileTracker { get; set; }
    public TileTracker MoneyTileTracker { get; set; }
    public TileTracker CarbonTileTracker { get; set; }
    public TileTracker RoadTileTracker { get; set; }
    public TileTracker ResidenceTileTracker { get; set; }
    public TileTracker FactoryTileTracker { get; set; }
    public TileTracker CarbonCaptureTileTracker { get; set; }
    public TileTracker ActivatableBuildingTileTracker { get; set; }
    public TileTracker ActivatableTileTracker { get; set; }
    
    // public int[] TileTrackers { get; set; }
    // public int AllTileTracker { get; set; }
    // public int MoneyTileTracker { get; set; }
    // public int CarbonTileTracker { get; set; }
    // public int RoadTileTracker { get; set; }
    // public int ResidenceTileTracker { get; set; }
    // public int FactoryTileTracker { get; set; }
    // public int CarbonCaptureTileTracker { get; set; }

    //Test of tile trackers
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
        //Activates all the tile trackers, to keep track of the number of every tile type.
        TileTrackers = new TileTracker[7];

        // AllTileTracker = new AllTileTracker();
        // TileTrackers[0] = AllTileTracker;

        // MoneyTileTracker = new MoneyTileTracker();
        // TileTrackers[1] = MoneyTileTracker;

        CarbonTileTracker = new CarbonTileTracker();
        TileTrackers[0] = CarbonTileTracker;

        ResidenceTileTracker = new ResidenceTileTracker();
        TileTrackers[1] = ResidenceTileTracker;

        RoadTileTracker = new RoadTileTracker();
        TileTrackers[2] = RoadTileTracker;

        FactoryTileTracker = new FactoryTileTracker();
        TileTrackers[3] = FactoryTileTracker;

        CarbonCaptureTileTracker = new CarbonCaptureTileTracker();
        TileTrackers[4] = CarbonCaptureTileTracker;
        
        ActivatableBuildingTileTracker = new ActivatableBuildingTileTracker();
        TileTrackers[5] = ActivatableBuildingTileTracker;

        ActivatableTileTracker = new ActivatableTileTracker();
        TileTrackers[6] = ActivatableTileTracker;



        
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

    public List<Tile> trackedTileList = new List<Tile>();
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

