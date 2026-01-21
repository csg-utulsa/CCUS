using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovementPathMap : MonoBehaviour
{
    private int widthOfGrid;
    public enum MapTileType
    {
        Empty,
        Road,
        Building
    }

    public MapTileType[][] movementPathMap;

    void Start(){

        //Fills 2D array with map tiles of type empty
        widthOfGrid = GridDataLoader.current.gridChunkSize;

        movementPathMap = new MapTileType[widthOfGrid][];
        for(int i = 0; i < widthOfGrid; i++){
            movementPathMap[i] = new MapTileType[widthOfGrid];
            for(int j = 0; j < widthOfGrid; j++){
                movementPathMap[i][j] = MapTileType.Empty;
            }
        }

        //Adds listeners for whenever the arrangement of Activatable Tiles changes
        GameEventManager.current.ActivatableTileJustPlaced.AddListener(ActivatableTileMapChanged);
        GameEventManager.current.ActivatableTileJustDestroyed.AddListener(ActivatableTileMapChanged);
        GameEventManager.current.BeginSwitchingCurrentGroundChunk.AddListener(SwitchedGroundChunk);
    }

    private void SetPathMapToEmpty(){
        for(int i = 0; i < movementPathMap.Length; i++){
            for(int j = 0; j < movementPathMap[i].Length; j++){
                movementPathMap[i][j] = MapTileType.Empty;
            }
        }
    }


    //Delays updating the tile map after a grid chunk switch to give the Tile Counter time to update
    private void SwitchedGroundChunk(){
        ActivatableTileMapChanged();
    }

    //Every time an activatable tile is placed or the grid chunk switches, it updates its tile map
    public void ActivatableTileMapChanged(){

        //Clears out the path map
        SetPathMapToEmpty();

        //Adds the activated road tiles to the path map
        Tile[] allRoadTiles = TileTypeCounter.current.RoadTileTracker.GetAllTiles();
        foreach(Tile roadTile in allRoadTiles){
                if(roadTile is ActivatableTile activatableRoad){
                    //Checks that the road is activated
                if(roadTile != null && activatableRoad != null && activatableRoad.IsActivated){
                    Vector2Int roadArrayCoordinates = SwitchToPathMapArrayCoordinates(roadTile.transform.position);
                    //Stores that there's a road at this position

                    movementPathMap[roadArrayCoordinates.x][roadArrayCoordinates.y] = MapTileType.Road;
                }
            }
        }

        //Adds the activated building tiles to the path map
        Tile[] allBuildingTiles = TileTypeCounter.current.ActivatableBuildingTileTracker.GetAllTiles();
        foreach(Tile building in allBuildingTiles){
            if(building is ActivatableTile activatableBuilding){
                //Checks that the building is activated
                if(building != null && activatableBuilding != null && activatableBuilding.IsActivated){
                    Vector2Int buildingArrayCoordinates = SwitchToPathMapArrayCoordinates(building.transform.position);
                    //Stores that there's a building at this position
                    movementPathMap[buildingArrayCoordinates.x][buildingArrayCoordinates.y] = MapTileType.Building;
                } 
            }
            
        }

    }

    public MapTileType GetTileForPoint(Vector3 tileWorldPosition){
        //switches world position to array coordinates
        Vector2Int arrayCoordinates = SwitchToPathMapArrayCoordinates(tileWorldPosition);

        //Returns tile type at array coordinates
        return GetTileForPoint(arrayCoordinates);
    }

    private MapTileType GetTileForPoint(Vector2Int arrayCoordinates){
        //returns the tile type at the given point, unless it's outside the array's bounds
        if(arrayCoordinates.x < movementPathMap.Length && arrayCoordinates.x >= 0){
            if(arrayCoordinates.y < movementPathMap[arrayCoordinates.x].Length && arrayCoordinates.y >= 0){
                return movementPathMap[arrayCoordinates.x][arrayCoordinates.y];
            }
        }
        
        //If the coordinates are outside the array's bounds, it returns that the cell is empty
        return MapTileType.Empty;
    }

    //Index 0 = up, Index 1 = right, Index 2 = down, Index 3 = left
    public MapTileType[] GetNeighborsForPoint(Vector3 tileWorldPosition){

        //Gets array coordinates for input world position
        Vector2Int arrayCoordinates = SwitchToPathMapArrayCoordinates(tileWorldPosition);

        Vector2Int[] directions = new Vector2Int[]{
            new Vector2Int(0, 1), //North / up
            new Vector2Int(1, 0), //East / right
            new Vector2Int(0, -1), //South / down
            new Vector2Int(-1, 0), //West / left
        };

        MapTileType[] neighbors = new MapTileType[directions.Length];

        //Stores the tile type of the neighbor in each direction in the neighbors[] array
        for(int i = 0; i < directions.Length; i++){
            neighbors[i] = GetTileForPoint(arrayCoordinates + directions[i]);
        }

        return neighbors;

    }

    //This is for a test only
    void Update(){
        if(Input.GetKeyDown(KeyCode.B)){
            Vector3 mouseWorldPosition = BuildingSystem.GetMouseWorldPosition();
            Vector2Int arrayCoordinates = SwitchToPathMapArrayCoordinates(mouseWorldPosition);


        }
    }

    //Switches to movementPathMap array coordinates from world coordinates
    private Vector2Int SwitchToPathMapArrayCoordinates(Vector3 worldCoordinates){
        Vector2Int gridCoords = GridManager.GM.SwitchToGridCoordinates(worldCoordinates);

        return SwitchToPathMapArrayCoordinates(gridCoords);
    }

    //Switches to movementPathMap array coordinates from grid coordinates
    private Vector2Int SwitchToPathMapArrayCoordinates(Vector2Int gridCoordinates){
        Vector2Int arrayCoords = new Vector2Int(gridCoordinates.x + (widthOfGrid / 2), gridCoordinates.y + (widthOfGrid / 2));
        return arrayCoords;
    }
}
