using UnityEngine;

public class FactoryTile : ActivatableBuilding
{
    public override void ThisTileJustPlaced(){
        
        base.ThisTileJustPlaced();
    }

    public override void ThisTileAboutToBeDestroyed(){
        base.ThisTileAboutToBeDestroyed();
    }

    // public override bool CheckIfTileIsPlaceable(bool displayErrorMessages){
    //     if(!base.CheckIfTileIsPlaceable(displayErrorMessages)){
    //         return false;
    //     }

    //     if(!EnoughEmployeesToPlace()){
    //         if(displayErrorMessages){
    //             unableToPlaceTileUI._unableToPlaceTileUI.NotEnoughPeople();
    //         }
    //         return false;
    //     }

    //     return true;
    // }

    // public override void ActivateBuilding(){
    //     //Updates the cap on number of people
    //     if(!IsActivated && PeopleManager.current != null){
    //         PeopleManager.current.AdjustNumberOfEmployees(tileScriptableObject.RequiredEmployees);
    //     }
    //     base.ActivateBuilding();
    // }

    // public override void DeactivateBuilding(){
    //     //Updates the cap on number of people
    //     if(IsActivated && PeopleManager.current != null){
    //         PeopleManager.current.AdjustNumberOfEmployees(-tileScriptableObject.RequiredEmployees);
    //     }
    //     base.DeactivateBuilding();
    // }

    // public override bool CheckIfTileIsPlaceable(bool displayErrorMessages){
    //     if(!base.CheckIfTileIsPlaceable(displayErrorMessages)){
    //         return false;
    //     }
    //     //Checks if there aren't enough people to place the factory
    //     if (!EnoughEmployeesToPlace())
    //     {
    //         if(displayErrorMessages){
    //             unableToPlaceTileUI._unableToPlaceTileUI.NotEnoughPeople();
    //         }
    //         return false;
    //     }

    //     return true;
    // }

    public bool EnoughEmployeesToPlace(){
        int numberOfAvailablePeople = PeopleManager.current.NumberOfPeople - PeopleManager.current.NumberOfEmployees;
        if(tileScriptableObject.RequiredEmployees <= numberOfAvailablePeople){
            return true;
        } else{
            return false;
        }
    }
}
