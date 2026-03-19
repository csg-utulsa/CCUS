using UnityEngine;

public class ActivatableTile : Tile
{
    public GameObject buildingActivatedGraphic;
    public bool IsActivated {get; set;} = false;
    public bool IsConnectedByRoads {get; set;} = false;
    private bool RequiresEmployees{
        get{
            return tileScriptableObject.RequiredEmployees > 0;
        }
    }
    private bool RequiresRoadConnections{
        get{
            return tileScriptableObject.MustBeConnectedByRoads;
        }
    }
    private bool HasEnoughEmployees = true;

    void Start(){

        //If this tile requires employees, it checks its activation state every time the number of people changes
        if(RequiresEmployees){
            GameEventManager.current.NumberOfPeopleChanged.AddListener(CheckTileForActivation);
        }
        
    }

    public override void ThisTileJustPlaced(){
        
        
        //Updates Activatable Tile Connections
        RoadAndResidenceConnectionManager.current.UpdateResidenceConnections(gameObject);

        base.ThisTileJustPlaced();

        //Alerts game event manager that an activatable tile was placed
        GameEventManager.current.ActivatableTileJustPlaced.Invoke();

    }

    public override void ThisTileAboutToBeDestroyed(){
        base.ThisTileAboutToBeDestroyed();
        DeactivateTile();

        //Updates Activatable Tile Connections
        
        //Updates residence/road connections
        if(GetComponent<ActivatableTile>() != null){

            foreach(GameObject neighboringTile in RoadAndResidenceConnectionManager.current.GetRoadNeighbors(gameObject)){
                RoadAndResidenceConnectionManager.current.UpdateResidenceConnections(neighboringTile);
            }

        }

        //Alerts the game manager that an activatable tile was just destroyed
        GameEventManager.current.ActivatableTileJustDestroyed.Invoke();
    }

    // public override SetIsEnoughResourcesToFunction(bool enoughResources){
    //     base.SetIsEnoughResourcesToFunction(enoughResources);

    //     SetTileVisuallyActive(enoughResources);
    // }

    // public override void UpdateIsTileVisuallyActive(){
    //     if(enoughResourcesToFunction && IsActivated){
    //         buildingActivatedGraphic.SetActive(true);
    //     } else{
    //         buildingActivatedGraphic.SetActive(false);
    //     }
    // }

    public virtual void SetRoadConnection(bool connectedByRoads){
        IsConnectedByRoads = connectedByRoads;
        CheckTileForActivation();
    }

    //Checks if the tile should be activated & updates its activation
    public virtual void CheckTileForActivation(){

        //Activates itself depending on if it needs employees/has employees and if it needs roads/has road connections
        if((!RequiresRoadConnections || IsConnectedByRoads) && (!RequiresEmployees || HasEnoughEmployees)){
            ActivateTile();
        } else{
            DeactivateTile();
        }

    }

    public virtual void ActivateTile(){
        
        //Updates the cap on number of people
        if(!IsActivated && PeopleManager.current != null){
            PeopleManager.current.AdjustNumberOfEmployees(tileScriptableObject.RequiredEmployees);
        }

        //Change a graphic to make it clear the house is activated.
        if(buildingActivatedGraphic != null){
          buildingActivatedGraphic.SetActive(true);  
        }

        //adjusts Level Manager money and carbon per hour
        if(!IsActivated){
            LevelManager.LM.AdjustNetCarbon(tileScriptableObject.AnnualCarbonAdded);
            LevelManager.LM.AdjustNetMoney(tileScriptableObject.AnnualIncome);
            TileActivationSettingChanged();
        }

        IsActivated = true;
        
    }

    public virtual void DeactivateTile(){

        //Updates the cap on number of people
        if(IsActivated && PeopleManager.current != null){
            PeopleManager.current.AdjustNumberOfEmployees(-tileScriptableObject.RequiredEmployees);
        }
        
        //Change a graphic to make it clear the house is NOT activated.
        if(buildingActivatedGraphic != null){
            buildingActivatedGraphic.SetActive(false);
        }


        //Adjusts level manager money and carbon per hour
        if(IsActivated){
            LevelManager.LM.AdjustNetCarbon(-tileScriptableObject.AnnualCarbonAdded);
            LevelManager.LM.AdjustNetMoney(-tileScriptableObject.AnnualIncome);
            TileActivationSettingChanged();
        }

        IsActivated = false;
        
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
