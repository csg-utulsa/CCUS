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
        ResidentialBuilding[] allResidences = GridManager.GM.GetResidentialTiles();
        bool allResidencesConnected = true;
        foreach(ResidentialBuilding residence in allResidences){
            if(residence.IsActivated == false){
                allResidencesConnected = false;
            }
        }
        return allResidencesConnected;
    }
}
