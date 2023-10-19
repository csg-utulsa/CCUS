using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    bool dragging = true;
    //public bool terrain; // for if we're displacing non-terrain, but a 2nd grid would be better
    public bool overRide;
    private GameObject replacement;
    DataManager dm = DataManager.DM;

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
        TileScriptableObject tileData = this.gameObject.GetComponent<Tile>().tileScriptableObject;
        DataManager.DM.AdjustYearlyCarbon(tileData.AnnualCarbonAdded - tileData.AnnualCarbonRemoved);
        DataManager.DM.AdjustStorageSize(tileData.AnnualCarbonStored);
        if (overRide){
            TileScriptableObject tileData2 = replacement.GetComponent<Tile>().tileScriptableObject;
            DataManager.DM.AdjustYearlyCarbon(-(tileData2.AnnualCarbonAdded - tileData2.AnnualCarbonRemoved));
            DataManager.DM.AdjustStorageSize(-tileData2.AnnualCarbonStored);
            Destroy(replacement);
        }
    }

    public void Pickup()
    {
        dragging = true;
    }

    public void OnTriggerEnter(Collider other)
    {   
        Debug.Log(this.gameObject.name+"hit"+other.gameObject.name);
        if (other.gameObject.tag == this.gameObject.tag)
        {
            overRide = true;
            replacement=other.gameObject;
        }
    }

    public void OnTriggerExit(Collider other)
    {
    if (other.gameObject.tag == this.gameObject.tag)
    {
            overRide = false;
            replacement = null;
    }
}
}
