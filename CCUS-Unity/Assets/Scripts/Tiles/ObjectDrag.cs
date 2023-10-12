using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    bool dragging = true;
    //public bool terrain; // for if we're displacing non-terrain, but a 2nd grid would be better
    public bool override;
    privat GameObject replacement;

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
        if (override){
        Destroy(replacement);
        }
    }

    public void Pickup()
    {
        dragging = true;
    }

    public OnCollisionEnter(Collider other)
    {
        if (other.GameObject.tag == this.GameObject.tag)
        {
            override = true;
            replacement=other.GameObject;
        }
    }

    public OnCollisionExit(Collider other)
    {
    if (other.GameObject.tag == this.GameObject.tag)
    {
            override = false;
            replacement = null;
    }
}
}
