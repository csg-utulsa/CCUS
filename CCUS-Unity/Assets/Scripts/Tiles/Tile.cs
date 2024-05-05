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

        //Debug.Log(this.name);
        if (state != TileState.Static) return;
        if (tileScriptableObject.AnnualIncome != 0)
            dm.AdjustMoney(tileScriptableObject.AnnualIncome);
        if (tileScriptableObject.AnnualCost != 0)
            dm.AdjustMoney(-1 * tileScriptableObject.AnnualCost);
        if (tileScriptableObject.AnnualCarbonStored != 0)
            dm.AdjustStored(tileScriptableObject.AnnualCarbonStored);
        if (tileScriptableObject.AnnualCarbonRemoved != 0)
            dm.AdjustCarbon(-1 * tileScriptableObject.AnnualCarbonRemoved);
        if (tileScriptableObject.AnnualCarbonAdded != 0)
            dm.AdjustCarbon(tileScriptableObject.AnnualCarbonAdded);
    }




    #region Unity Methods

    private void Awake()
    {
        TickManager.TM.Tick.AddListener(OnTick);
        tileMatHandler = gameObject.GetComponent<TileMaterialHandler>();
        dm = LevelManager.LM;
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



