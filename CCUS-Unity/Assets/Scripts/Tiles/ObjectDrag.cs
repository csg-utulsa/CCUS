using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    //private Vector3 offset;
    bool dragging = false;

    public void Drag()
    {
        dragging = true;
        //offset = transform.position - BuildingSystem.GetMouseWorldPosition();
    }

    public void Update()
    {
        if (!dragging) return;
        Vector3 pos = BuildingSystem.GetMouseWorldPosition();
        transform.position = BuildingSystem.current.SnapCoordinateToGrid(pos);
    }

    public void Place()
    {
        dragging = false;
    }

    public bool isDragging()
    {
        return dragging;
    }
}
