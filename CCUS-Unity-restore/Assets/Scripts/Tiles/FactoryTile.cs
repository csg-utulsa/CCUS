using UnityEngine;

public class FactoryTile : ActivatableBuilding
{
    public override void ThisTileJustPlaced(){
        
        base.ThisTileJustPlaced();
    }

    public override void ThisTileAboutToBeDestroyed(){
        base.ThisTileAboutToBeDestroyed();
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
