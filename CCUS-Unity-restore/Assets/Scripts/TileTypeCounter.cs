using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTypeCounter : MonoBehaviour
{
    public static TileTypeCounter current;

    public TileTracker[] TileTrackers { get; set; }
    public TileTracker AllTileTracker { get; set; }
    public TileTracker MoneyTileTracker { get; set; }
    public TileTracker CarbonTileTracker { get; set; }
    public TileTracker RoadTileTracker { get; set; }
    public TileTracker ResidenceTileTracker { get; set; }
    public TileTracker FactoryTileTracker { get; set; }
    public TileTracker CarbonCaptureTileTracker { get; set; }

    void Start()
    {
        if(current == null){
            current = this;
        }else{
            Destroy(this);
        }
        //Activates all the tile trackers, to keep track of every added tile of each type.
        TileTrackers = new TileTracker[7];

        AllTileTracker = new AllTileTracker();
        TileTrackers[0] = AllTileTracker;

        MoneyTileTracker = new MoneyTileTracker();
        TileTrackers[1] = MoneyTileTracker;

        CarbonTileTracker = new CarbonTileTracker();
        TileTrackers[2] = CarbonTileTracker;

        ResidenceTileTracker = new ResidenceTileTracker();
        TileTrackers[3] = ResidenceTileTracker;

        RoadTileTracker = new RoadTileTracker();
        TileTrackers[4] = RoadTileTracker;

        FactoryTileTracker = new FactoryTileTracker();
        TileTrackers[5] = FactoryTileTracker;

        CarbonCaptureTileTracker = new CarbonCaptureTileTracker();
        TileTrackers[6] = CarbonCaptureTileTracker;
    }

    public void CheckTileTrackersForRemoval(GameObject objectToCheck){
        foreach(TileTracker tileTracker in TileTrackers){
            tileTracker.CheckTileForRemoval(objectToCheck.GetComponent<Tile>());
        }
    }
    public void CheckTileTrackersForAddition(GameObject objectToCheck){
        foreach(TileTracker tileTracker in TileTrackers){
            tileTracker.CheckTileForAddition(objectToCheck.GetComponent<Tile>());
        }
    }
}

//Base class for each type of Tile Tracker. The Tile Trackers keep a list of each kind of tile.
public class TileTracker{

    public List<Tile> trackedTileList = new List<Tile>();
    protected void AddTile(Tile tile){
        trackedTileList.Add(tile);
    }
    
    protected void RemoveTile(Tile tile){
        trackedTileList.Remove(tile);
    }

    public virtual void CheckTileForRemoval(Tile tile){
        Debug.LogError("Base Tile Tracker Class has been accidentally implemented. You need to use one of its derived classes.");
    }

    public virtual void CheckTileForAddition(Tile tile){
        Debug.LogError("Base Tile Tracker Class has been accidentally implemented. You need to use one of its derived classes.");
    }

    public Tile[] GetAllTiles(){
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
