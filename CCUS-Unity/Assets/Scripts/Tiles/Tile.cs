using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] TileType tileType;
    public TileScriptableObject tileScriptableObject;
    [SerializeField] PlaceableObject po;

    public TileState state = TileState.Uninitialized;
    bool menuOpen = false;


    // Decorator system is a work in progress ~Coleton
    /*TileDecorator td;
    public void SetTileDecorator(TileDecorator td)
    {
        this.td = td;
    }*/

    // This method is pretty bad but I needed to have it to test features. Can be reworked at a later date. ~Coleton
    private void CheckForInput()
    {
        /*if (Input.GetMouseButtonDown(0))
        {
            if (po.Placed)
                EndDrag();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (po.isDragging())
                EndDrag();
            else
                CloseMenu();
        }*/

        // TEMPORARY BUTTON TO ENABLE DRAGGING ~Coleton
        /*if (Input.GetKeyDown(KeyCode.D))
        {
            if (!od.isDragging())
                BeginDrag();
        }*/
    }

    public void SetTileState(TileState ts)
    {
        state = ts;
    }

    void OnTick()
    {
        if (state != TileState.Static) return;
        if (tileScriptableObject.AnnualIncome != 0)
            DataManager.DM.AdjustMoney(tileScriptableObject.AnnualIncome);
        if (tileScriptableObject.AnnualCost != 0)
            DataManager.DM.AdjustMoney(-1 * tileScriptableObject.AnnualCost);
        if (tileScriptableObject.AnnualCarbonStored != 0)
            DataManager.DM.AdjustStored(tileScriptableObject.AnnualCarbonStored);
        if (tileScriptableObject.AnnualCarbonRemoved != 0)
            DataManager.DM.AdjustCarbon(-1 * tileScriptableObject.AnnualCarbonRemoved);
        if (tileScriptableObject.AnnualCarbonAdded != 0)
            DataManager.DM.AdjustCarbon(tileScriptableObject.AnnualCarbonAdded);
    }

    #region Unity Methods

    private void Awake()
    {
        TickManager.TM.Tick.AddListener(OnTick);
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

public enum TileState
{
    Uninitialized, Static, Moveable
}


