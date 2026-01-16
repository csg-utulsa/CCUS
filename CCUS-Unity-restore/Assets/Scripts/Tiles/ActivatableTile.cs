using UnityEngine;

public class ActivatableTile : Tile
{
    public GameObject buildingActivatedGraphic;
    public bool IsActivated {get; set;} = false;

    public override void ThisTileJustPlaced(){
        
        
        //Updates Activatable Tile Connections
        RoadAndResidenceConnectionManager.current.UpdateResidenceConnections(gameObject);

        base.ThisTileJustPlaced();

    }

    public override void ThisTileAboutToBeDestroyed(){
        base.ThisTileAboutToBeDestroyed();
        DeactivateBuilding();
    }

    public virtual void ActivateBuilding(){
        

        //Change a graphic to make it clear the house is activated.
        if(buildingActivatedGraphic != null){
          buildingActivatedGraphic.SetActive(true);  
        }

        //adjusts Level Manager money and carbon per hour
        if(!IsActivated){
            LevelManager.LM.AdjustNetCarbon(tileScriptableObject.AnnualCarbonAdded);
            LevelManager.LM.AdjustNetMoney(tileScriptableObject.AnnualIncome);
        }

        IsActivated = true;
        TileActivationSettingChanged();
    }

    public virtual void DeactivateBuilding(){


        //Change a graphic to make it clear the house is NOT activated.
        if(buildingActivatedGraphic != null){
            buildingActivatedGraphic.SetActive(false);
        }


        //Adjusts level manager money and carbon per hour
        if(IsActivated){
            LevelManager.LM.AdjustNetCarbon(-tileScriptableObject.AnnualCarbonAdded);
            LevelManager.LM.AdjustNetMoney(-tileScriptableObject.AnnualIncome);
        }

        IsActivated = false;
        TileActivationSettingChanged();
    }



    
    public virtual void TileActivationSettingChanged(){
        //LevelManager.LM.UpdateNetCarbonAndMoney();
    }

    public virtual void UpdateBuildingActivation(){
        if(IsActivated && buildingActivatedGraphic != null){
            buildingActivatedGraphic.SetActive(true);  
        }
        if(!IsActivated && buildingActivatedGraphic != null){
            buildingActivatedGraphic.SetActive(false);
        }
    }


    //Loading functions -- Update graphics when tiles load
    public virtual void LoadActivatedBuilding(){
        IsActivated = true;

        //Change a graphic to make it clear the building is activated.
        if(buildingActivatedGraphic != null){
          buildingActivatedGraphic.SetActive(true);  
        }
    }

    public virtual void LoadDeactivatedBuilding(){
        IsActivated = false;

        //Change a graphic to make it clear the building is NOT activated.
        if(buildingActivatedGraphic != null){
            buildingActivatedGraphic.SetActive(false);
        }
    }
}
