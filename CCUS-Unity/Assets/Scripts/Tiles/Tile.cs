using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] PlaceableObject po;
    TileState state = TileState.Uninitialized;
    bool menuOpen = false;


    // Decorator system is a work in progress ~Coleton
    TileDecorator td;
    public void SetTileDecorator(TileDecorator td)
    {
        this.td = td;
    }

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
        Debug.Log(gameObject.name + ": Tick received");
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
}

public enum TileState
{
    Uninitialized, Static, Moveable
}
