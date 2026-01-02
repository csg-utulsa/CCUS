using UnityEngine;

public class RoadAndResidenceConnectionManager : MonoBehaviour
{
    public static RoadAndResidenceConnectionManager RARCM;

    void Start()
    {
        if(RARCM == null){
            RARCM = this;
        }else{
            Destroy(this);
        }
    }

    public bool AllResidencesAreConnected(){
        Tile[] allResidences = GridManager.GM.ResidenceTileTracker.GetAllTiles();
        bool allResidencesConnected = true;
        foreach(Tile tile in allResidences){
            if(tile is ResidentialBuilding residence){
                if(residence.IsActivated == false){
                    allResidencesConnected = false;
                }
            }
            
        }
        return allResidencesConnected;
    }
}
