using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryPeopleManager : MonoBehaviour
{
    public int numberOfPeople = 0;
    public int incomeOfPerson = 2;
    public int maxPeople = 0;
    public int NetPeopleIncome {get; set;} = 0;
    public static TemporaryPeopleManager TPM;

    void Start(){
        TickManager.TM.MoneyTick.AddListener(OnMoneyTick);
        if(TPM == null){
            TPM = this;
        }
    }

    void OnMoneyTick(){
        LevelManager.LM.AdjustMoney(numberOfPeople * incomeOfPerson);
    }
    

    public void AddAPerson(){
        numberOfPeople++;
        NetPeopleIncome = numberOfPeople * incomeOfPerson;
        LevelManager.LM.UpdateNetCarbonAndMoney();
    }

    //Called from ObjectDrag when a tile is placed. It adds that tile's max people to the max people count
    public void UpdateMaxPeople(){

        int _maxPeople = 0;
        Tile[] moneyProducingTiles = GridManager.GM.GetMoneyProducingTiles();
        foreach(Tile tile in moneyProducingTiles){
            
            if(tile.tileScriptableObject.MaxPeople > 0){
                _maxPeople += tile.tileScriptableObject.MaxPeople;
            }
        }
        maxPeople = _maxPeople;
    }

    public int GetMaxPeople(){
        return maxPeople;
    }

    public bool CanAddMorePeople(){
        return (numberOfPeople < maxPeople);
    }

}
