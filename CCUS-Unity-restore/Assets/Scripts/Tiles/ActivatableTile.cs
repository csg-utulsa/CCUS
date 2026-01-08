using UnityEngine;

public class ActivatableTile : Tile
{
    public GameObject buildingActivatedGraphic;
    public bool IsActivated {get; set;} = false;

    public virtual void ActivateBuilding(){
        IsActivated = true;

        //Change a graphic to make it clear the house is activated.
        if(buildingActivatedGraphic != null){
          buildingActivatedGraphic.SetActive(true);  
        }
    }

    public virtual void DeactivateBuilding(){
        IsActivated = false;

        //Change a graphic to make it clear the house is NOT activated.
        if(buildingActivatedGraphic != null){
            buildingActivatedGraphic.SetActive(false);
        }
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
