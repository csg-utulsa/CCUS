using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

//using UnityEngine.WSA;
[SelectionBase]
public class Tile : MonoBehaviour
{
    
    [SerializeField] TileType tileType;
    public TileScriptableObject tileScriptableObject;
    [SerializeField] PlaceableObject po;

    public TileState state = TileState.Uninitialized;
    bool menuOpen = false;
    LevelManager dm;
    private TileMaterialHandler tileMatHandler;

    public Vector3 tilePosition;

    public GridCell gridCell {get; set;}

    private int _currentTileNetMoney = 0;
    private int _currentTileNetCarbon = 0;

    public int currentTileNetMoney{
        get {
            return _currentTileNetMoney;
        }
        set {
            _currentTileNetMoney = value;
            //LevelManager.LM.UpdateNetCarbonAndMoney();
        }
    }

    public int currentTileNetCarbon{
        get {
            return _currentTileNetCarbon;
        }
        set {
            _currentTileNetCarbon = value;
            //LevelManager.LM.UpdateNetCarbonAndMoney();
        }
    }



    //private bool accountedForNetContribution = false;


    void Start(){
        //Saves the tile coordinates of this tile
        // Debug.Log("Script start tile called");
        // if(state == TileState.Static){
        //     tilePosition = BuildingSystem.current.SnapCoordinateToGrid(transform.position);
        //     //Adds this object to the GridManager's database of all current tiles
        //     Debug.Log("Added tile from Start of Tile Script");
        //     GridManager.GM.AddObject(this.gameObject);// "Tile Script of this tile: " + this.gameObject);

        //     //Adds annual income and annual carbon for tiles that are placed when the game starts
        //     setInitialIncomeAndCarbon();
            
            
        // }
    }

    public void setInitialIncomeAndCarbon(){
        if(tileScriptableObject != null){
            currentTileNetMoney =  tileScriptableObject.AnnualIncome;
            currentTileNetCarbon = tileScriptableObject.AnnualCarbonAdded; 
        }
        //LevelManager.LM.UpdateNetCarbonAndMoney();
    }






    public Vector3 getTileGridPosition(){
        tilePosition = BuildingSystem.current.SnapCoordinateToGrid(transform.position);
        return tilePosition;
    }



    public bool MouseIsOnTile(){
        Vector3 mouseWorldPosition = BuildingSystem.GetMouseWorldPosition();
        Vector3 mouseGridCoordinate = BuildingSystem.current.SnapCoordinateToGrid(mouseWorldPosition);
        Vector3 mouseSnappedWorldPosition = new Vector3(mouseGridCoordinate.x, 0f, mouseGridCoordinate.z);
        GameObject[] gameObjectsInCell =  GridManager.GM.GetGameObjectsInGridCell(mouseSnappedWorldPosition);
        if (gameObjectsInCell.Contains(gameObject)){
            return true;
        } else{
            return false;
        }
        // Vector3 mouseWorldPosition = BuildingSystem.GetMouseWorldPosition();
        // if(GridManager.GM.GetGridCellFromWorldPoint(mouseWorldPosition) == gridCell){
        //     return true;
            
        // }else{
        //     return false;
        // }
    }

    public virtual bool CheckIfTileIsPlaceable(bool displayErrorMessages){

        //Requires non-Terrain tiles to be placed on terrain
        // if(!tileScriptableObject.isTerrain){
        //     if(!GridManager.GM.TileIsOverGround(transform.position)){
        //         return false;
        //     }
        // }

        //Checks if there is too much carbon to place tile
        if(tooMuchCarbonToPlace()){
            if(displayErrorMessages){
                unableToPlaceTileUI._unableToPlaceTileUI.tooMuchCarbon();
            }
            
            return false;
        }

        //Checks if there is not enough money to place the tile
        if (notEnoughMoneyToPlace())
        {
            if(displayErrorMessages){
                unableToPlaceTileUI._unableToPlaceTileUI.notEnoughMoney();
            }
            return false;
        }

        // //Checks if there aren't enough people to place the factory
        // if (!EnoughEmployeesToPlace())
        // {
        //     if(displayErrorMessages){
        //         unableToPlaceTileUI._unableToPlaceTileUI.NotEnoughPeople();
        //     }
        //     return false;
        // }

        //If none of the other conditions return false, it returns true        
        return true;

        
    }

    public virtual void ThisTileJustPlaced(){
        

        setInitialIncomeAndCarbon();

        //Updates the carbon, money, and employees for tile if it's not an activatable tile
        if(!(this is ActivatableTile activatableTile)){
            LevelManager.LM.AdjustNetMoney(tileScriptableObject.AnnualIncome);
            LevelManager.LM.AdjustNetCarbon(tileScriptableObject.AnnualCarbonAdded);
            if(PeopleManager.current != null){
                PeopleManager.current.AdjustNumberOfEmployees(tileScriptableObject.RequiredEmployees);
            }
        }


        GameEventManager.current.TileJustPlaced.Invoke();

    }

    public virtual void ThisTileAboutToBeDestroyed(){

        //Updates the carbon, money, and employees for tile if it's not an activatable tile
        if(!(this is ActivatableTile activatableTile)){
            LevelManager.LM.AdjustNetMoney(-tileScriptableObject.AnnualIncome);
            LevelManager.LM.AdjustNetCarbon(-tileScriptableObject.AnnualCarbonAdded);
            if(PeopleManager.current != null){
                PeopleManager.current.AdjustNumberOfEmployees(-tileScriptableObject.RequiredEmployees);
            }
        }
        // Removes object from GridManager, 
        // so when the roads use the GridManager to update their connections, they will ignore this tile
        GridManager.GM.RemoveObject(gameObject, false);

    }




    // This method is pretty bad but I needed to have it to test features. Can be reworked at a later date. ~Coleton
    private void CheckForInput()
    {

    }

    public void SetTileState(TileState ts)
    {
        state = ts;
    }


    public bool tooMuchCarbonToPlace(){
        if(tileScriptableObject.AnnualCarbonAdded <= 0){
            return false;
        }
        else if(LevelManager.overMaxCarbon()){
            return true;
        }
        else {
            return false;
        }
    }

    public bool notEnoughMoneyToPlace(){
        if(tileScriptableObject.BuildCost > LevelManager.LM.GetMoney()){
            return true;
        } else {
            return false;
        }
    }

    //Deletes this tile
    public void DeleteThisTile(){
        //Refunds cost of tile when tile deleted
        LevelManager.LM.AdjustMoney(tileScriptableObject.BuildCost);
        

        //Updates neighboring road connections and deletes the gameObject
        GetComponent<ObjectDrag>().DestroyTile();

        //moved these to DestroyTile() function of ObjectDrag
        // //Updates road connections of neighbors
        // if(GetComponent<RoadConnections>() != null){
        //     GetComponent<RoadConnections>().UpdateNeighborConnections();
        // }else{
        //     UpdateTileNeighborConnections();
        // }

        // Destroy(gameObject);
    }

    //Updates the road connection graphics of any surrounding roads
    public void UpdateTileNeighborConnections(){
        GameObject[] neighborGameObjects = RoadAndResidenceConnectionManager.current.GetRoadNeighbors(gameObject);
        for(int i = 0; i < neighborGameObjects.Length; i++){
            GameObject _neighbor = neighborGameObjects[i];
            if(_neighbor != null && _neighbor.GetComponent<RoadConnections>() != null){
                _neighbor.GetComponent<RoadConnections>().UpdateModelConnections(false);
            }    
        }
    }

    public bool EnoughEmployeesToPlace(){
        if(PeopleManager.current != null){
            int numberOfAvailablePeople = PeopleManager.current.NumberOfPeople - PeopleManager.current.NumberOfEmployees;
            if(tileScriptableObject.RequiredEmployees <= numberOfAvailablePeople){
                return true;
            } else{
                return false;
            }
        } else{
            return true;
        }
        
    }




    #region Unity Methods

    private void Awake()
    {
        tileMatHandler = gameObject.GetComponent<TileMaterialHandler>();
        dm = LevelManager.LM;




        
        
        if(tileScriptableObject != null){
            //Lets Level Manager know if this tile has the biggest income, carbon, or carbon removed (has obscure use in setting size of the bubble pop ups over objects)
            if(dm.getCurrentMaxTileIncome() < tileScriptableObject.AnnualIncome){
               dm.setCurrentMaxTileIncome(tileScriptableObject.AnnualIncome); 
            }
            if(dm.getCurrentMaxTileCarbon() < tileScriptableObject.AnnualCarbonAdded){
               dm.setCurrentMaxTileCarbon(tileScriptableObject.AnnualCarbonAdded); 
            }
            if(dm.getCurrentMinTileCarbon() > tileScriptableObject.AnnualCarbonAdded){
               dm.setCurrentMinTileCarbon(tileScriptableObject.AnnualCarbonAdded); 
            }

        }
    }

    private void Update()
    {
        if (menuOpen) CheckForInput();
    }

    // I disabled moving objects after the fact ~Coleton
    /*private void OnMouseDown()
    {
        if (!menuOpen)
            OpenMenu();
        else
            CloseMenu();
    }*/

    #endregion


    #region Menu Methods

    private void OpenMenu()
    {
        // We eventually want to hook this up to the a UI
        menuOpen = true;
    }

    private void CloseMenu()
    {
        menuOpen = false;
    }

    private void BeginDrag()
    {

        po.Pickup();
    }

    private void EndDrag()
    {
        po.Place();
    }

    #endregion

public TileType GetTileType()
    {
        return tileType;
    }
}


public enum TileType
{
    Terrain, Placeable, Static
}

public enum TileState
{
    Uninitialized, Static, Moveable
}



