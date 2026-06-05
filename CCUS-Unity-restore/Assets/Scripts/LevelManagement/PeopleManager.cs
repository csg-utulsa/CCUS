using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PeopleManager : MonoBehaviour
{
    public static PeopleManager current;

    
    public int numberOfPeople = 0;
    public int NumberOfPeople{
        set{

            int previousPeople = numberOfPeople;
            numberOfPeople = value;
            LevelManager.LM.AdjustNetMoney((numberOfPeople-previousPeople)*incomeOfPerson);
            PeoplePanel._peoplePanel.NumberOfPeople = numberOfPeople;
            GameEventManager.current.GetEvent(EventType.E.NumberOfPeopleChanged).Invoke();
        }
        get{
            return numberOfPeople;
        }
    }

    public int numberOfEmployees = 0;
    public int NumberOfEmployees{
        set{
            numberOfEmployees = value;
            PeoplePanel._peoplePanel.NumberOfEmployees = numberOfEmployees;
        }
        get{
            return numberOfEmployees;
        }
    }

    public float PercentFilled{
        get{
            return (float)NumberOfPeople / (float)maxPeople;
        }
    }
    

    public int incomeOfPerson = 2;
    public int maxPeople {get; set;} = 0;
    public int NetPeopleIncome {get; set;} = 0;
    
    public PeoplePanel peoplePanel;
    

    void Start(){
        TickManager.TM.MoneyTick.AddListener(OnMoneyTick);
        if(current == null){
            current = this;
        }

        //Redistributes employees every time a workplace tile is placed or destroyed & every time a building's activation state changes
        GameEventManager.current.GetEvent(EventType.E.NumOfWorkPlaceTilesChanged).AddListener(RedistributeEmployees);
        GameEventManager.current.GetEvent(EventType.E.ActivatableTileJustPlaced).AddListener(RedistributeEmployees);
    }

    void OnMoneyTick(){
        //LevelManager.LM.AdjustMoney(NumberOfPeople * incomeOfPerson);
    }
    

    private void AddAPerson(){
        NumberOfPeople++;
        NetPeopleIncome = NumberOfPeople * incomeOfPerson;
        //LevelManager.LM.UpdateNetCarbonAndMoney();
        GameEventManager.current.GetEvent(EventType.E.PersonJustAdded).Invoke();

        //Distributes employees to all of the workplaces
        RedistributeEmployees();
        
    }

    //Called from Tile when a residential tile is activated or deactivated. It updates the max number of people.
    public void AdjustMaxPeople(int _maxPeopleIncrement){

        // int _maxPeople = 0;
        // Tile[] residentialTiles = TileTypeCounter.current.ResidenceTileTracker.GetAllTiles();
        // foreach(Tile tile in residentialTiles){
            
        //     if(tile is ResidentialBuilding residence){
        //         if(residence.IsActivated){
        //             _maxPeople += residence.gameObject.GetComponent<Tile>().tileScriptableObject.MaxPeople;
        //         }
                
        //     }
        // }
        maxPeople += _maxPeopleIncrement;

        if(NumberOfPeople > maxPeople){
            NumberOfPeople = maxPeople;
        }

        peoplePanel.MaxNumberOfPeople = maxPeople;


        //Distributes employees to all of the workplaces
        RedistributeEmployees();

        //UpdateNumberOfEmployees();
        
    }

    public void AdjustNumberOfEmployees(int _adjustNumOfEmployees){


        NumberOfEmployees += _adjustNumOfEmployees;

        

        
    }

    //Redistributes number of employees every time workplace & residential tiles are placed/destroyed
    public void RedistributeEmployees(){

        //Gets an array of all tiles that require employees (workplace tiles)
        Tile[] workplaceTiles = TileTypeCounter.current.WorkplaceTileTracker.GetAllTiles();

        int employeesRemaining = NumberOfPeople;

        List<ActivatableTile> workplacesWithoutRoads = new List<ActivatableTile>();

        //Loops through each workplace tile
        foreach(Tile workplace in workplaceTiles){

            if(workplace is ActivatableTile activatableWorkplace){

                int neededEmployees = activatableWorkplace.tileScriptableObject.RequiredEmployees;
                bool isConnectedByRoads = activatableWorkplace.IsConnectedByRoads;

                //Only gives employees to workplaces if they're connected by roads and there are enough employees
                if((isConnectedByRoads || !workplace.tileScriptableObject.MustBeConnectedByRoads) && (neededEmployees <= employeesRemaining)){

                    //Gives it the required number of employees
                    activatableWorkplace.CurrentEmployees = neededEmployees;
                    employeesRemaining -= neededEmployees;
                    

                }else{ //If there's not enough employees for it, gives it 0

                    //If the reason the workplace didn't get employees was because it wasn't connected by roads,
                    //then it gets a second chance at getting employees after all workplaces connected by roads
                    //get their employees. The ones without roads just have lowest priority.
                    if(!isConnectedByRoads){
                        workplacesWithoutRoads.Add(activatableWorkplace);
                    }

                    activatableWorkplace.CurrentEmployees = 0;
                }

                //Tells each tile to update its activation status
                activatableWorkplace.CheckTileForActivation();

            }

        }

        //Loops through each workplace tile that isn't connected by roads.
        //This means they have lower priority & only get whatever employees are left over.
        foreach(ActivatableTile workplace in workplacesWithoutRoads){
            int neededEmployees = workplace.tileScriptableObject.RequiredEmployees;
            //Gives workplaces employees until the remaining employees run out.
            if(neededEmployees <= employeesRemaining){
                workplace.CurrentEmployees = neededEmployees;
                employeesRemaining -= neededEmployees;
            }
        }

        
    }

    public int GetMaxPeople(){
        return maxPeople;
    }

    public void AttemptToAddPerson(){
        if(CanAddMorePeople()){
            AddAPerson();
            //PeoplePanel._peoplePanel.NewPersonUIPopUp();
        }
        //else{
        //     //Displays the "Can only add people to houses connected by roads" Error
        //     if(!RoadAndResidenceConnectionManager.RARCM.AllResidencesAreConnected()){
        //         unableToPlaceTileUI._unableToPlaceTileUI.mustConnectResidences();
        //     }else{
        //         unableToPlaceTileUI._unableToPlaceTileUI.notEnoughHomes();
        //     }
            
        // }
    }

    public bool CanAddMorePeople(){
        return (NumberOfPeople < maxPeople);
    }
}
