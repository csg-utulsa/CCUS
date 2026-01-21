using UnityEngine;

public class ActivatableBuilding : ActivatableTile
{
    // public GameObject buildingActivatedGraphic;
    // public bool IsActivated {get; set;} = false;

    // public override void ActivateBuilding(){
    //     base.ActivateBuilding();
    //     if(!IsActivated){
    //        GameEventManager.current.BuildingActivationStateChanged.Invoke(); 
    //     }
        
    // }

    // public override void DeactivateBuilding(){
    //     base.DeactivateBuilding();
    //     if(IsActivated){
            
    //     }
        
    // }

    public override void TileActivationSettingChanged(){
        GameEventManager.current.BuildingActivationStateChanged.Invoke();
    }

    // public virtual void ActivateBuilding(){
    //     IsActivated = true;

    //     //Change a graphic to make it clear the house is activated.
    //     if(buildingActivatedGraphic != null){
    //       buildingActivatedGraphic.SetActive(true);  
    //     }
    // }

    // public virtual void DeactivateBuilding(){
    //     IsActivated = false;

    //     //Change a graphic to make it clear the house is NOT activated.
    //     if(buildingActivatedGraphic != null){
    //         buildingActivatedGraphic.SetActive(false);
    //     }
    // }

    // public virtual void UpdateBuildingActivation(){
    //     if(IsActivated && buildingActivatedGraphic != null){
    //         buildingActivatedGraphic.SetActive(true);  
    //     }
    //     if(!IsActivated && buildingActivatedGraphic != null){
    //         buildingActivatedGraphic.SetActive(false);
    //     }
    // }
}
