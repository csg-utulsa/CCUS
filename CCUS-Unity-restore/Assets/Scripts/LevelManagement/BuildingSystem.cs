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

    [HideInInspector] public bool ClickAndDragHasBeenUsed {get; set;} = false;

    [HideInInspector]
    public GameObject activeObject;
    public Tile activeTile;

    public LayerMask groundLayer;

    private bool isInDragMode = false;
    private bool IsInDragMode{
        get{
            return isInDragMode;
        }
        set{
            //Calls event to begin drag placing tiles game state
            if(value && !isInDragMode){ //Begin drag placing
            dragTimer = 0f;
                GameEventManager.current.GetEvent(EventType.E.BeginDragPlaceTiles).Invoke();
            }else if(!value && isInDragMode){ //End drag placing
                GameEventManager.current.GetEvent(EventType.E.EndDragPlaceTiles).Invoke();
            }

            isInDragMode = value;
        }
    }

    private float dragTimer = 0f;
    public float maxTimeToBeInDragMode = .2f;

    //Prevents multiple tiles from being placed in the same square when the user holds down the left mouse button
    bool preventMultipleObjectPlacement = false;


    //Stores the last prefab that was placed so that another one can be made when the first one is placed
    GameObject previousPrefabToPlace;

    #region Unity methods

    private void Awake() 
    {
        current = this;
        grid = gridLayout.gameObject.GetComponent<Grid>();
        
    }

    private void Start(){
        TickManager.TM.EndOfMoneyAndPollutionTicks.AddListener(EndOfResourceTicks);
        GameEventManager.current.GetEvent(EventType.E.MoneyAmountUpdated).AddListener(MoneyAmountUpdated);
    }

    
    private void Update()
    {

        if (!activeObject)
        {
            return;
        }


        //Handles disabling drag mode after a certain amount of time
        if(Input.GetMouseButton(0)){
            dragTimer += Time.deltaTime;
            if(dragTimer > maxTimeToBeInDragMode){
                IsInDragMode = false;
            }
        }

        //The next lines disable the tile being dragged (activeObject) if the mouse isn't over the screen.
        if(!isMouseOverScreen()){
            activeObject.SetActive(false);
        }else{
            //the next line 
            activeObject.GetComponent<ObjectDrag>().Update();
            activeObject.SetActive(true);
        }

        //Player clicks down to place tile
        if(Input.GetMouseButtonDown(0) && !preventMultipleObjectPlacement){
            
            attemptToPlaceSelectedTile();
            
        }

        if (Input.GetMouseButtonUp(0))
        {
            preventMultipleObjectPlacement = false;
            IsInDragMode = false;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            deselectActiveObject();
        }
    }

    //Runs at the end of money and pollution ticks
    private void EndOfResourceTicks(){
        //Updates material validity of active tile
        UpdateActiveTileMaterialValididty();
    }

    //Updates tile material validity when the amount of money changes
    private void MoneyAmountUpdated(){
        UpdateActiveTileMaterialValididty();
    }

    private void UpdateActiveTileMaterialValididty(){
        if(activeObject != null && activeObject.GetComponent<ObjectDrag>() != null)
            activeObject.GetComponent<ObjectDrag>().updateTileMaterialValidity();
    }

    public void deselectActiveObject(){
        if (activeObject != null){

            //Makes the tile select panel visually deselect the tile button
            TileSelectPanel.TSP.deselectAllButtons();
            
            //Makes any roads that might be connected to the active game object update their visual connections
            if(activeObject.GetComponent<RoadConnections>() != null){
                activeObject.GetComponent<RoadConnections>().UpdateNeighborConnections();
            }

            Destroy(activeObject);

        }
    }

    //Called from ObjectDrag of activeObject and enables click and drag placing
    public void activeObjectMovedToNewTile(){
        if(activeTile != null && activeTile.tileScriptableObject.allowClickAndDrag && Input.GetMouseButton(0)){
            IsInDragMode = true;
            ClickAndDragHasBeenUsed = true;
            attemptToPlaceSelectedTile();
            preventMultipleObjectPlacement = false;
            
        }
    }


    private void attemptToPlaceSelectedTile(){

        if (CanBePlaced(true))
        {   
            Tile previousActiveTile = activeTile;
            
            placeSelectedTile();
            preventMultipleObjectPlacement = true;

            //previousActiveTile.CallSpecialPlacementEvent();

            //Calls event for when the player clicks to place a tile, without dragging it
            if(!IsInDragMode){
                GameEventManager.current.GetEvent(EventType.E.TilePlacedWithoutDrag).Invoke();
            }
            
        } 

    }



    //Places the currently selected tile
    private void placeSelectedTile(){

        activeObject.SetActive(true);

        activeObject.GetComponent<ObjectDrag>().Place();
        
        int cost = activeTile.tileScriptableObject.BuildCost;
        LevelManager.LM.AdjustMoney(-1 * cost);
        
        
        activeObject = null;
        activeTile = null;

        //Makes a new prefab to drag around, so now we don't have to repeatedly click
        //the button for the tile we want to place :)
        InitializeWithObject(previousPrefabToPlace);
    }



    #endregion

    #region Utils

    // Raycast to get world position of mouse hover input
    public static Vector3 GetMouseWorldPosition() 
    {
        int layer_mask = LayerMask.GetMask("RaycastHitter");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit raycastHit, 30f, layer_mask))
        {
            return raycastHit.point;
        }
        else 
        {
            return Vector3.zero;
        }
    }


    //Move this function to a separate class
    public static bool isMouseOverScreen(){

        //Checks if mouse is over a scrollable area, like the tile select panel
        if(ScrollingUI.mouseIsOverScrollableArea){
            return false;
        }

        //Checks if the mouse is over the tile delete button
        if(TrashButtonScript.TBS != null && TrashButtonScript.TBS.mouseIsOverTrashButton()){
            return false;
        }

        //Checks if the mouse is over the people panel
        if(PeoplePanel._peoplePanel != null && PeoplePanel._peoplePanel.isMouseOverPanel()){
            return false;
        }

        //Checks if mouse is over the new chunk purchase UI
        if(ActivateChunkPurchaseUI.current.IsMouseOverAreaPrice()){
            return false;
        }

        //Checks if mouse is within screen bounds
        if(MouseScreenPosition.MouseIsOverScreen()){
            return true;
        } else{
            return false;
        }


    }

    //Move to separate class
    //gets cell center's world position.
    public Vector3 SnapCoordinateToGrid(Vector3 position) 
    {
        Vector3Int cellPos = gridLayout.WorldToCell(position);
        position = grid.GetCellCenterWorld(cellPos);
        return position;
    }
  


    #endregion

    #region Building Placement

    public void InitializeWithObject(GameObject prefab)
    {
        //stores the prefab being placed in order to instantiate another one when this one is placed.
        previousPrefabToPlace = prefab;

        if(activeObject != null){
            Destroy(activeObject);
        }

        
        Vector3 position = SnapCoordinateToGrid(GetMouseWorldPosition());

        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        activeObject = obj;
        activeTile = obj.GetComponent<Tile>();

        
    }

    //Called By Tile Select Panel when the user deselects the current tile
     public void deselectCurrentObject(){
        Destroy(activeObject);
     }


    //This function raycasts straight down over a tile to find out what's on it
    public static bool isObjectOverVoid(){
        if(current.activeObject == null) return false;
        Vector3 activeObjectPositionOnGrid = current.SnapCoordinateToGrid(current.activeObject.transform.position);
        Vector3 raycastStartPosition = new Vector3(activeObjectPositionOnGrid.x, activeObjectPositionOnGrid.y + 5.0f, activeObjectPositionOnGrid.z);

        bool isOverVoid = true;
        isOverVoid = !(Physics.Raycast(raycastStartPosition, -Vector3.up, 100.0F, current.groundLayer));

        return isOverVoid;
    }

    //This function is the same as above, but depends on mouse position instead of activeObject.position
    public static bool isMouseOverVoid(){
        Vector3 positionOnGrid = current.SnapCoordinateToGrid(GetMouseWorldPosition());
        Vector3 raycastStartPosition = new Vector3(positionOnGrid.x, positionOnGrid.y + 5.0f, positionOnGrid.z);

        bool isOverVoid = true;
        isOverVoid = !(Physics.Raycast(raycastStartPosition, -Vector3.up, 100.0F, current.groundLayer));

        return isOverVoid;
    }

    public bool isActiveObjectOverlappingSameTileType(){
        if(activeObject == null) return false;

        //Checks if trying to place object over same object
        //Use the GridManager singleton instance
        foreach (GameObject obj in GridManager.GM.GetGameObjectsInGridCell(activeTile.gameObject))
        {
            Tile tile = obj.GetComponent<Tile>();
            if (tile != null && tile.tileScriptableObject == activeTile.tileScriptableObject)
            {
                return true;
            }
        }
        return false;

    }



    //Checks if active object's material should be in the valid state. Returns true if no active tile is selected.
    //Used to change placeability state of the highlighted tile graphic.
    public bool ShouldActiveTileMaterialBeValid(){
        if(activeObject == null) return true;
        if (isObjectOverVoid()){
            if(activeTile != null && activeTile.tileScriptableObject.isTerrain){
                return false;
            } else{
                return true;
            }
            
        } 
        if(!isMouseOverScreen()) return true;
        if(isActiveObjectOverlappingSameTileType()) return true;

        if (!activeTile.gameObject.GetComponent<ObjectDrag>().CanBePlacedOnOverlappingTile()){
            return false;
        } else{
            return true;
        }
        
    }

    public bool CanBePlaced(bool attemptingToPlaceTile)
    {
        //Guard against null Tile Reference
        if (activeTile == null) return false;

        //Checks if the mouse is over the screen. If not, it can't be placed
        if(!isMouseOverScreen()){
            return false;
        }

        //Checks if object is over void
        if (isObjectOverVoid()){
            return false;
        }
            
        //Checks if the active chunk is purchased
        if(!ChunkPurchaseManager.current.ActiveChunkIsPurchased){

            //displays "Must Purchase Area" if attempting to place tile
            if(attemptingToPlaceTile){
                unableToPlaceTileUI._unableToPlaceTileUI.MustPurchaseAreaError();
            }
            
            return false;
        }

        if(isActiveObjectOverlappingSameTileType()){
            return false;
        }


        //Checks if there's enough money and that the carbon isn't maxed out
        if(attemptingToPlaceTile){
            if(!activeTile.CheckIfTileIsPlaceable(true)){

                //Calls event for when the user fails to place a tile and isn't dragging.
                if(!IsInDragMode){
                    GameEventManager.current.GetEvent(EventType.E.FailedToPlaceTileWithoutDrag).Invoke();
                }
                
                return false;
            }
        }else{
            if(!activeTile.CheckIfTileIsPlaceable(false)){
                return false;
            }
        }



        //Decides if a tile can destroy the tile it is being placed on top of
        if (!activeTile.gameObject.GetComponent<ObjectDrag>().CanBePlacedOnOverlappingTile()){
            if(attemptingToPlaceTile && !TrashButtonScript.TBS.HasBeenSelectedAtLeastOnce){
                unableToPlaceTileUI._unableToPlaceTileUI.UseTrashButtonToRemoveTiles();
            }
            return false;
        }

        return true;
    }


    #endregion
}
