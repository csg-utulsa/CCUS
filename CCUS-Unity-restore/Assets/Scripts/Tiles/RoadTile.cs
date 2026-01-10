using UnityEngine;

public class RoadTile : ActivatableTile
{
    public override void ThisTileJustPlaced(){
        base.ThisTileJustPlaced();
        
        //visually updates road connections
        if(GetComponent<RoadConnections>() != null){
            GetComponent<RoadConnections>().UpdateModelConnections(true);
        } else{
            UpdateTileNeighborConnections();
        }

    }

    public override void ThisTileAboutToBeDestroyed(){
        base.ThisTileAboutToBeDestroyed();

        //Updates road connections of neighbors
        if(GetComponent<RoadConnections>() != null){
            GetComponent<RoadConnections>().UpdateNeighborConnections();
        }else{
            UpdateTileNeighborConnections();
        }

        //Updates residence/road connections
        if(GetComponent<RoadConnections>() != null || GetComponent<ResidentialBuilding>() != null){

            // //Deactivates all road connection graphics. They will be reenabled by the residence connection updates, if they're still connected to two houses.
            // foreach(RoadConnections road in GridManager.GM.GetRoadTiles()){
            //     road.deactivateConnectedRoad();
            // }

            // //Updates Connections of Each Residence
            // foreach(ResidentialBuilding residence in GridManager.GM.GetResidentialTiles()){
            //     residence.UpdateResidenceConnections();
            // }

            foreach(GameObject neighboringTile in GridManager.GM.GetRoadNeighbors(gameObject)){
                RoadAndResidenceConnectionManager.RARCM.UpdateResidenceConnections(neighboringTile);
            }

            

        }
    }

    // //This method displays if this road is connecting two or more residences
    // public override void ActivateBuilding(){
    //     if(base.buildingActivatedGraphic != null)
    //         base.buildingActivatedGraphic.SetActive(true);
    // }
    // //This method displays if this road is NOT connecting two or more residences
    // public override void DeactivateBuilding(){
    //     if(activationGraphic != null)
    //         base.buildingActivatedGraphic.SetActive(false);
    // }
}
