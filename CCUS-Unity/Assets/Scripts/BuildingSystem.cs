/** TODO: 
*   - Add method that spawns object under mouse when GUI is clicked 
*   - Have object follow mouse outside of world border
        -- Change click-to-drag to always follow w/ click to place
*   - Prevent multiple buildings to follow mouse (true/false)
**/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem current { get; private set; }

    public GridLayout gridLayout;
    public Grid grid;
    [SerializeField] private Tilemap TerrainTilemap;
    [SerializeField] private Tilemap PlaceablesTilemap;
    [SerializeField] private TileBase whiteTile;

    [SerializeField] GameObject[] prefabs;

    /*public GameObject prefab1;
    public GameObject prefab2;*/

    [HideInInspector]
    public GameObject activeObject;
    private PlaceableObject objectToPlace;
    private Tile activeTile;

    public LayerMask groundLayer;

    //Prevents multiple tiles from being placed in the same square when the user holds down the left mouse button
    bool preventMultipleObjectPlacement = false;

    //Drag placement mode: tracks the last cell where a tile was placed to avoid placing multiple tiles in the same cell while dragging
    private Vector3Int lastPlacedCell = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);
    private bool isDragPlacing = false;

    //Set by ObjectDrag Script on each tile
    //public bool mouseOverScreen = false;

    //public bool mouseOverScreen = false;


    //Stores the last prefab that was placed so that another one can be made when the first one is placed
    GameObject previousPrefabToPlace;

    #region Unity methods

    private void Awake() 
    {
        current = this;
        grid = gridLayout.gameObject.GetComponent<Grid>();
    }

    
    private void Update()
    {

        if (!activeObject)
        {
            return;
        }

        //The next lines disable the tile being dragged (activeObject) if the mouse isn't over the screen.
        if(!isMouseOverScreen()){
            activeObject.SetActive(false);
        }else{
            //the next line 
            activeObject.GetComponent<ObjectDrag>().Update();
            activeObject.SetActive(true);
        }

        // Start drag placement mode
        if (Input.GetMouseButtonDown(0))
        {
            isDragPlacing = true;
            lastPlacedCell = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);
        }

        // Handle continuous drag placement
        if (isDragPlacing && Input.GetMouseButton(0))
        {
            Vector3 mouseWorldPos = GetMouseWorldPosition();
            Vector3Int currentCell = gridLayout.WorldToCell(mouseWorldPos);

            // Only place if moved to a new cell
            if (currentCell != lastPlacedCell && isMouseOverScreen())
            {
                lastPlacedCell = currentCell;
                
                //Reset the active tile's neighbors before moving so it recalculates connections at new position
                ConnectedTileHandler activeTileHandler = activeObject.GetComponent<ConnectedTileHandler>();
                
                activeObject.transform.position = grid.GetCellCenterWorld(currentCell);

                // Try to place at new location
                if (CanBePlaced(objectToPlace))
                {
                    placeSelectedTile();
                }
            }
        }

        // End drag placement mode
        if (Input.GetMouseButtonUp(0))
        {
            isDragPlacing = false;
            preventMultipleObjectPlacement = false;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            //Vector3 tileGridPosition = activeObject.GetComponent<Tile>().tilePosition;

            //GridManager.GM.RemoveObject(activeObject);
            deselectActiveObject();
            

        }
    }

<<<<<<< Updated upstream
    public void activeObjectMovedToNewTile(Vector3 newSnappedPosition){
        preventMultipleObjectPlacement = false;
=======
    public void deselectActiveObject(){
        if (activeObject != null){
            TileSelectPanel.TSP.deselectAllButtons();
            Destroy(activeObject);
        }
    }

    //Called from ObjectDrag of activeObject and enables click and drag placing
    public void activeObjectMovedToNewTile(){
        if(activeTile != null && activeTile.tileScriptableObject.allowClickAndDrag && Input.GetMouseButton(0)){
            StartCoroutine(DelayActionOneFrame(attemptToPlaceSelectedTile));
            //attemptToPlaceSelectedTile();
        }
    }

    //This Coroutine delays the placement of a new tile by one frame when placing with click and drag
    //This gives the ConnectedTileHandler time to create a "tempNeighbor" road tile and assign it the 
    //correct road model(end road, left, right, corner, etc)
    private IEnumerator DelayActionOneFrame(Action delayedAction){
        for (int i = 0; i < 1; i++)
        {
            yield return 0;
        }
        //yield return new WaitForSeconds(.1f);
        delayedAction();
    }

    public void attemptToPlaceSelectedTile(){

        if (CanBePlaced(objectToPlace))
        {
            placeSelectedTile();
        } 
        else if(objectToPlace.GetComponent<Tile>().tooMuchCarbonToPlace() && isMouseOverScreen()) {
            unableToPlaceTileUI._unableToPlaceTileUI.tooMuchCarbon();
        } else if(objectToPlace.GetComponent<Tile>().notEnoughMoneyToPlace() && isMouseOverScreen()) {
            unableToPlaceTileUI._unableToPlaceTileUI.notEnoughMoney();
        }

>>>>>>> Stashed changes
    }

    //Places the currently selected tile
    public void placeSelectedTile(){
        GridManager.GM.AddObject(objectToPlace.gameObject);   
        objectToPlace.Place();
        Vector3Int start = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
        TakeArea(start, objectToPlace.Size);
        int cost = activeTile.tileScriptableObject.BuildCost;
        LevelManager.LM.AdjustMoney(-1 * cost);
        
        //Force physics update so ConnectedTileHandler triggers fire immediately
        Physics.SyncTransforms();
        
        //Notify adjacent tiles to recalculate their connections
        NotifyAdjacentTilesToRecalculate(objectToPlace.gameObject);
        
        activeObject = null;
        objectToPlace = null;
        activeTile = null;

        //Makes a new prefab to drag around, so now we don't have to repeatedly click
        //the button for the tile we want to place :)
        InitializeWithObject(previousPrefabToPlace);
    }

    //Tell all adjacent connected tiles to update their models based on new neighbors
    private void NotifyAdjacentTilesToRecalculate(GameObject placedTile)
    {
        Vector3Int tileCell = gridLayout.WorldToCell(placedTile.transform.position);
        
        Vector3Int[] directions = new Vector3Int[]
        {
            new Vector3Int(0, 1, 0),  // North
            new Vector3Int(1, 0, 0),  // East
            new Vector3Int(0, -1, 0), // South
            new Vector3Int(-1, 0, 0)  // West
        };
        
        for (int i = 0; i < directions.Length; i++)
        {
            Vector3Int checkCell = tileCell + directions[i];
            Vector3 checkWorldPos = grid.GetCellCenterWorld(checkCell);
            
            foreach (GameObject obj in GridManager.GM.GetGameObjectsInGridCell(checkWorldPos))
            {
                if (obj != placedTile)
                {
                    ConnectedTileHandler neighborHandler = obj.GetComponent<ConnectedTileHandler>();
                    if (neighborHandler != null)
                    {
                        neighborHandler.UpdateModel();
                    }
                }
            }
        }
    }


    //Sets the material of the active object to the invalid (red) state or not
    // public void setActiveObjectMaterialValidity(){
    //         if(activeObject != null){
    //             if(CanBePlaced(objectToPlace)){
    //             activeObject.GetComponent<ObjectDrag>().tileMaterialHandler.MaterialSet(TileMaterialHandler.matState.HoveringValid);
    //         } else {
    //             activeObject.GetComponent<ObjectDrag>().tileMaterialHandler.MaterialSet(TileMaterialHandler.matState.HoveringInvalid);
    //         }
    //     }
        
    // }

    #endregion

    #region Utils

    // Raycast to get world position of mouse hover input
    public static Vector3 GetMouseWorldPosition() 
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            //BuildingSystem.current.mouseOverScreen = true;
            return raycastHit.point;
        }
        else 
        {
            //BuildingSystem.current.mouseOverScreen = false;
            return Vector3.zero;
        }
    }


    public static bool isMouseOverScreen(){
        if(ScrollingUI.mouseIsOverScrollableArea){
            return false;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            //BuildingSystem.current.mouseOverScreen = true;
            return true;
        }
        else 
        {
            //BuildingSystem.current.mouseOverScreen = false;
            return false;
        }
    }

    //gets cell center's world position
    public Vector3 SnapCoordinateToGrid(Vector3 position) 
    {
        //position = position + GetMouseWorldPosition();
        Vector3Int cellPos = gridLayout.WorldToCell(position);
        position = grid.GetCellCenterWorld(cellPos);
        return position;
    }

    /**
    // NOTE: Could work for objects that placed on top of terrain tiles BUT a separate grid might be better

    public Vector3 SnapCoordinateToGrid(Vector3 position, bool terrain) 
    {
        //position = position + GetMouseWorldPosition();
        Vector3Int cellPos = gridLayout.WorldToCell(position);
        position = grid.GetCellCenterWorld(cellPos);
        if (!terrain) position.y = position.y + 1f;
        return position;
    }
    **/
    
    

    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;

        foreach (var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }

        return array;
    }

    #endregion

    #region Building Placement

    public void InitializeWithObject(GameObject prefab)
    {
        //stores the prefab being placed in order to instantiate another one when this one is placed.
        previousPrefabToPlace = prefab;

        if(activeObject != null){
            //Vector3 tileGridPosition = activeObject.GetComponent<Tile>().tilePosition;
            //GridManager.GM.RemoveObject(activeObject);
            Destroy(activeObject);
        }
        
        //if (objectToPlace != null) return;
        Vector3 position = SnapCoordinateToGrid(GetMouseWorldPosition());

        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        activeObject = obj;
        objectToPlace = obj.GetComponent<PlaceableObject>();
        activeTile = obj.GetComponent<Tile>();
    }

    //Called By Tile Select Panel when the user deselects the current tile
     public void deselectCurrentObject(){
        Destroy(activeObject);
     }


    public bool MoveObject(GameObject obj)
    {
        if (activeObject != null) return false;
        activeObject = obj;
        objectToPlace = activeObject.GetComponent<PlaceableObject>();
        return true;
    }

    //This function raycasts straight down over a tile to find out what's on it
    public static bool isObjectOverVoid(){
        Vector3 activeObjectPositionOnGrid = current.SnapCoordinateToGrid(current.activeObject.transform.position);
        Vector3 raycastStartPosition = new Vector3(activeObjectPositionOnGrid.x, activeObjectPositionOnGrid.y + 5.0f, activeObjectPositionOnGrid.z);

        bool isOverVoid = true;
        isOverVoid = !(Physics.Raycast(raycastStartPosition, -Vector3.up, 100.0F, current.groundLayer));

        return isOverVoid;
    }

    public bool CanBePlaced(PlaceableObject placeableObject)
    {
        //Checks if the mouse is over the screen. If not, it can't be placed
        if(!isMouseOverScreen()){
            return false;
        }

        BoundsInt area = new BoundsInt();
        area.position = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
        area.size = new Vector3Int(area.size.x + 1, area.size.y + 1, area.size.z);

        TileBase[] baseArray = GetTilesBlock(area, TerrainTilemap);

        //Checks if object is over void
        if (isObjectOverVoid()==true)
            return false;

        //Checks if there is too much carbon to place tile
        if(placeableObject.GetComponent<Tile>().tooMuchCarbonToPlace()){
            return false;
        }

        //Checks if there is not enough money to place the tile
        if (placeableObject.GetComponent<Tile>().notEnoughMoneyToPlace())
        {
            return false;
        }

        //Checks if trying to place object over same object
        //Guard against a missing Tile reference and use the GridManager singleton instance
        if (activeTile == null) return false;
        foreach (GameObject obj in GridManager.GM.GetGameObjectsInGridCell(activeTile.gameObject))
        {
            Tile tile = obj.GetComponent<Tile>();
            if (tile != null && tile.tileScriptableObject == activeTile.tileScriptableObject)
            {
                return false;
            }
        }


        foreach (var b in baseArray)
        {
            if (b == whiteTile)
            {
                return false;
            }
        }

        

        int cost = activeTile.tileScriptableObject.BuildCost;
        if (cost != 0 && activeTile.state == TileState.Uninitialized)
        {
            if (LevelManager.LM.GetMoney() < cost)
            {
                return false;
            }
        }

        if (!activeTile.gameObject.GetComponent<ObjectDrag>().IsValidOverlap()) { return false; }//makes sure tile is not placed with invalid tiles

        return true;
    }

    public void TakeArea(Vector3Int start, Vector3Int size)
    {
        switch (activeObject.GetComponent<Tile>().GetTileType())
        {
            case TileType.Terrain:
                TerrainTilemap.BoxFill(start, whiteTile, start.x, start.y,
                            start.x + size.x, start.y + size.y);
                break;
            case TileType.Placeable:
                PlaceablesTilemap.BoxFill(start, whiteTile, start.x, start.y,
                            start.x + size.x, start.y + size.y);
                break;
        }
    }

    #endregion
}
