using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    bool dragging = true;
    //public bool terrain; // for if we're displacing non-terrain, but a 2nd grid would be better

    public void Update()
    {
        if (!dragging) return;
        Vector3 pos = BuildingSystem.GetMouseWorldPosition();
        transform.position = BuildingSystem.current.SnapCoordinateToGrid(pos);
        // ALT: transform.position = BuildingSystem.current.SnapCoordinateToGrid(pos, terrain);
    }

    public void Place()
    {
        dragging = false;
        Debug.Log("ObjectDrag.Place()");
    }

    public void Pickup()
    {
        dragging = true;
        Debug.Log("ObjectDrag.Pickup()");
    }
}
