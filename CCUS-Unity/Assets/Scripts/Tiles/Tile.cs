using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] ObjectDrag od;
    TileDecorator td;
    bool menuOpen = false;

 
    public void SetTileDecorator(TileDecorator td)
    {
        this.td = td;
    }

    // This method is pretty bad but I needed to have it to test features. Can be reworked at a later date.
    private void CheckForInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (od.isDragging())
                EndDrag();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (od.isDragging())
                EndDrag();
            else
                CloseMenu();
        }

        // TEMPORARY BUTTON TO ENABLE DRAGGING
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (!od.isDragging())
                BeginDrag();
        }
    }

    #region Unity Methods
    private void Update()
    {
        if (menuOpen) CheckForInput();
    }

    private void OnMouseDown()
    {
        if (!menuOpen)
            OpenMenu();
        else
            CloseMenu();
    }

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

        od.Drag();
    }

    private void EndDrag()
    {
        od.Place();
    }

    #endregion
}
