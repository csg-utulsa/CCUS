using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private int _currentTileNetMoney = 0;
    private int _currentTileNetCarbon = 0;

    public int currentTileNetMoney{
        get {
            return _currentTileNetMoney;
        }
        set {
            _currentTileNetMoney = value;
            LevelManager.LM.UpdateNetCarbonAndMoney();
        }
    }

    public int currentTileNetCarbon{
        get {
            return _currentTileNetCarbon;
        }
        set {
            _currentTileNetCarbon = value;
            LevelManager.LM.UpdateNetCarbonAndMoney();
        }
    }



    //private bool accountedForNetContribution = false;


    void Start(){
        //Saves the tile coordinates of this tile
        if(state == TileState.Static){
            tilePosition = BuildingSystem.current.SnapCoordinateToGrid(transform.position);
            //Adds this object to the GridManager's database of all current tiles
            GridManager.GM.AddObject(this.gameObject);

            //Adds annual income and annual carbon for tiles that are placed when the game starts
            setInitialIncomeAndCarbon();
            
            
        }
    }

    public void setInitialIncomeAndCarbon(){
        if(tileScriptableObject != null){
            currentTileNetMoney =  tileScriptableObject.AnnualIncome;
            currentTileNetCarbon = tileScriptableObject.AnnualCarbonAdded; 
        }
        LevelManager.LM.UpdateNetCarbonAndMoney();
    }




    public Vector3 getTileGridPosition(){
        tilePosition = BuildingSystem.current.SnapCoordinateToGrid(transform.position);
        return tilePosition;
    }



    // This method is pretty bad but I needed to have it to test features. Can be reworked at a later date. ~Coleton
    private void CheckForInput()
    {

    }

    public void SetTileState(TileState ts)
    {
        state = ts;
    }

    void OnTick()
    {

        // I split this tick up into a separate pollution tick and money tick
        // if (state != TileState.Static) return;
        // if (tileScriptableObject.AnnualIncome != 0)
        //     dm.AdjustMoney(tileScriptableObject.AnnualIncome);
        // if (tileScriptableObject.AnnualCost != 0)
        //     dm.AdjustMoney(-1 * tileScriptableObject.AnnualCost);
        // if (tileScriptableObject.AnnualCarbonStored != 0)
        //     dm.AdjustStored(tileScriptableObject.AnnualCarbonStored);
        // if (tileScriptableObject.AnnualCarbonRemoved != 0)
        //     dm.AdjustCarbon(-1 * tileScriptableObject.AnnualCarbonRemoved);
        // if (tileScriptableObject.AnnualCarbonAdded != 0)
        //     dm.AdjustCarbon(tileScriptableObject.AnnualCarbonAdded);
    }

    void OnPollutionTick()
    {
        if (state != TileState.Static) return;
        if (tileScriptableObject.AnnualCarbonAdded != 0){
            //Moved to Level Manager
            //dm.AdjustCarbon(tileScriptableObject.AnnualCarbonAdded);
        }
            

    }

    void OnMoneyTick()
    {
        if (state != TileState.Static) return;
        if (tileScriptableObject.AnnualIncome != 0){
            dm.AdjustMoney(tileScriptableObject.AnnualIncome);
        }
             

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
        GridManager.GM.RemoveObject(gameObject);
        if(GetComponent<RoadConnections>() != null){ //Updates road connections of neighbors
            GetComponent<RoadConnections>().UpdateNeighborConnections();
        }
        Destroy(gameObject);
    }




    #region Unity Methods

    private void Awake()
    {
        TickManager.TM.Tick.AddListener(OnTick);
        TickManager.TM.PollutionTick.AddListener(OnPollutionTick);
        TickManager.TM.MoneyTick.AddListener(OnMoneyTick);
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



