using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TemporaryPeopleManager : MonoBehaviour
{
    // public int numberOfPeople = 0;
    // public int NumberOfPeople{
    //     set{
    //         numberOfPeople = value;
    //         PeoplePanel._peoplePanel.NumberOfPeople = numberOfPeople;
    //     }
    //     get{
    //         return numberOfPeople;
    //     }
    // }
    // public int incomeOfPerson = 2;
    // public int maxPeople {get; set;} = 0;
    // public int NetPeopleIncome {get; set;} = 0;
    
    // public PeoplePanel peoplePanel;
    // public static TemporaryPeopleManager TPM;

    // void Start(){
    //     TickManager.TM.MoneyTick.AddListener(OnMoneyTick);
    //     if(TPM == null){
    //         TPM = this;
    //     }
    //     peoplePanel = PeoplePanel._peoplePanel;
    // }

    // void OnMoneyTick(){
    //     LevelManager.LM.AdjustMoney(NumberOfPeople * incomeOfPerson);
    // }
    

    // private void AddAPerson(){
    //     NumberOfPeople++;
    //     NetPeopleIncome = NumberOfPeople * incomeOfPerson;
    //     LevelManager.LM.UpdateNetCarbonAndMoney();
        
    // }

    // //Called from ObjectDrag when a tile is placed. It adds that tile's max people to the max people count
    // public void UpdateMaxPeople(){

    //     int _maxPeople = 0;
    //     Tile[] residentialTiles = GridManager.GM.ResidenceTileTracker.GetAllTiles();
    //     foreach(Tile tile in residentialTiles){
            
    //         if(tile is ResidentialBuilding residence){
    //             if(residence.IsActivated){
    //                 _maxPeople += residence.gameObject.GetComponent<Tile>().tileScriptableObject.MaxPeople;
    //             }
                
    //         }
    //     }
    //     maxPeople = _maxPeople;

    //     if(NumberOfPeople > maxPeople){
    //         NumberOfPeople = maxPeople;
    //     }

    //     peoplePanel.MaxNumberOfPeople = maxPeople;
        
    // }

    // public int GetMaxPeople(){
    //     return maxPeople;
    // }

    // public void AttemptToAddPerson(){
    //     if(CanAddMorePeople()){
    //         AddAPerson();
    //         //PeoplePanel._peoplePanel.NewPersonUIPopUp();
    //     }
    //     //else{
    //     //     //Displays the "Can only add people to houses connected by roads" Error
    //     //     if(!RoadAndResidenceConnectionManager.RARCM.AllResidencesAreConnected()){
    //     //         unableToPlaceTileUI._unableToPlaceTileUI.mustConnectResidences();
    //     //     }else{
    //     //         unableToPlaceTileUI._unableToPlaceTileUI.notEnoughHomes();
    //     //     }
            
    //     // }
    // }

    // public bool CanAddMorePeople(){
    //     return (NumberOfPeople < maxPeople);
    // }

}
