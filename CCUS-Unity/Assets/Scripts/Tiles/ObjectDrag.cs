using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    bool dragging = true;
    //public bool terrain; // for if we're displacing non-terrain, but a 2nd grid would be better
    public bool overRide;
    private GameObject replacement;

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
        if (overRide){
        Destroy(replacement);
        }
    }

    public void Pickup()
    {
        dragging = true;
    }

    public void OnCollisionEnter(Collision other)
    {   
        Debug.Log(this.gameObject.name+"hit"+other.gameObject.name);
        if (other.gameObject.tag == this.gameObject.tag)
        {
            overRide = true;
            replacement=other.gameObject;
        }
    }

    public void OnCollisionExit(Collision other)
    {
    if (other.gameObject.tag == this.gameObject.tag)
    {
            overRide = false;
            replacement = null;
    }
}
}
