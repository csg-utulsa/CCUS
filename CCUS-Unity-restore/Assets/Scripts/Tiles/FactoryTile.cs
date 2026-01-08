using UnityEngine;

public class FactoryTile : ActivatableBuilding
{
    public override bool CheckIfTileIsPlaceable(bool displayErrorMessages){
        if(!base.CheckIfTileIsPlaceable(displayErrorMessages)){
            return false;
        }
        //Checks if there aren't enough people to place the factory
        if (!EnoughEmployeesToPlace())
        {
            if(displayErrorMessages){
                unableToPlaceTileUI._unableToPlaceTileUI.NotEnoughPeople();
            }
            return false;
        }

        return true;
    }
    public bool EnoughEmployeesToPlace(){
        int numberOfAvailablePeople = PeopleManager.current.NumberOfPeople - PeopleManager.current.NumberOfEmployees;
        if(tileScriptableObject.RequiredEmployees <= numberOfAvailablePeople){
            return true;
        } else{
            return false;
        }
    }
}
