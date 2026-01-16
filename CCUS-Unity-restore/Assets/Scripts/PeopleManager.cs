using UnityEngine;

public class PeopleManager : MonoBehaviour
{
    public int numberOfPeople = 0;
    public int NumberOfPeople{
        set{
            int previousPeople = numberOfPeople;
            numberOfPeople = value;
            LevelManager.LM.AdjustNetMoney((numberOfPeople-previousPeople)*incomeOfPerson);
            PeoplePanel._peoplePanel.NumberOfPeople = numberOfPeople;
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

    public int incomeOfPerson = 2;
    public int maxPeople {get; set;} = 0;
    public int NetPeopleIncome {get; set;} = 0;
    
    public PeoplePanel peoplePanel;
    public static PeopleManager current;

    void Start(){
        TickManager.TM.MoneyTick.AddListener(OnMoneyTick);
        if(current == null){
            current = this;
        }
        //peoplePanel = PeoplePanel._peoplePanel;
    }

    void OnMoneyTick(){
        LevelManager.LM.AdjustMoney(NumberOfPeople * incomeOfPerson);
    }
    

    private void AddAPerson(){
        NumberOfPeople++;
        NetPeopleIncome = NumberOfPeople * incomeOfPerson;
        //LevelManager.LM.UpdateNetCarbonAndMoney();
        GameEventManager.current.PersonJustAdded.Invoke();
        
    }

    //Called from Tile when a tile is placed or destroyed. It updates the max number of people and the current number of employees.
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

        //UpdateNumberOfEmployees();
        
    }

    public void AdjustNumberOfEmployees(int _adjustNumOfEmployees){

        // int _numOfEmployees = 0;
        // Tile[] factoryTiles = TileTypeCounter.current.FactoryTileTracker.GetAllTiles();
        // foreach(Tile tile in factoryTiles){
        //     if(tile is ActivatableTile activatableTile && activatableTile.IsActivated){
        //         if(activatableTile is FactoryTile factory){
        //             _numOfEmployees += factory.gameObject.GetComponent<Tile>().tileScriptableObject.RequiredEmployees;
        //         }
        //     }
            
        // }

        NumberOfEmployees += _adjustNumOfEmployees;

        
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
