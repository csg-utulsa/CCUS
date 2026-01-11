using UnityEngine;

public class ActivatableTile : Tile
{
    public GameObject buildingActivatedGraphic;
    public bool IsActivated {get; set;} = false;

    public override void ThisTileJustPlaced(){
        base.ThisTileJustPlaced();
        
        //Updates Activatable Tile Connections
        RoadAndResidenceConnectionManager.RARCM.UpdateResidenceConnections(gameObject);

        PeopleManager.current.UpdateMaxPeople();

    }

    public override void ThisTileAboutToBeDestroyed(){
        base.ThisTileAboutToBeDestroyed();
    }

    public virtual void ActivateBuilding(){
        IsActivated = true;

        //Change a graphic to make it clear the house is activated.
        if(buildingActivatedGraphic != null){
          buildingActivatedGraphic.SetActive(true);  
        }
        TileActivationSettingChanged();
    }

    public virtual void DeactivateBuilding(){
        IsActivated = false;

        //Change a graphic to make it clear the house is NOT activated.
        if(buildingActivatedGraphic != null){
            buildingActivatedGraphic.SetActive(false);
        }
        TileActivationSettingChanged();
    }
    
    public virtual void TileActivationSettingChanged(){
        LevelManager.LM.UpdateNetCarbonAndMoney();
    }

    public virtual void UpdateBuildingActivation(){
        if(IsActivated && buildingActivatedGraphic != null){
            buildingActivatedGraphic.SetActive(true);  
        }
        if(!IsActivated && buildingActivatedGraphic != null){
            buildingActivatedGraphic.SetActive(false);
        }
    }
}
