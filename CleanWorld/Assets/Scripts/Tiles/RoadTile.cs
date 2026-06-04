using UnityEngine;

public class RoadTile : ActivatableTile
{
    public override void ThisTileJustPlaced(){
        
        
        //visually updates road connections
        if(GetComponent<RoadConnections>() != null){
            GetComponent<RoadConnections>().UpdateModelConnections(true);
        } else{
            UpdateTileNeighborConnections();
        }

        base.ThisTileJustPlaced();

    }

    public override void ThisTileAboutToBeDestroyed(){
        base.ThisTileAboutToBeDestroyed();

        //Updates visual road connections (not activations) of neighbors
        if(GetComponent<RoadConnections>() != null){
            GetComponent<RoadConnections>().UpdateNeighborConnections();
        }else{
            UpdateTileNeighborConnections();
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
