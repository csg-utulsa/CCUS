//Clean up repetitive code in this script
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unableToPlaceTileUI : MonoBehaviour
{
    float carbonGraphicTimer = 0f;
    float moneyGraphicTimer = 0f;
    float homesGraphicTimer = 0f;
    float newTileGraphicTimer = 0f;

    public float timeToShowErrorGraphic = 1.5f;
    public float timeToShowNewTileGraphic = 2.3f;

    public GameObject tooMuchCarbonGraphic;
    public GameObject notEnoughMoneyGraphic;
    public GameObject notEnoughHomesGraphic;
    public GameObject newTileGraphic;

    public static unableToPlaceTileUI _unableToPlaceTileUI;


    void Awake(){

        _unableToPlaceTileUI = this;
    }

    //Displays the error that says: TOO MUCH CARBON!!!
    public void tooMuchCarbon(){
        carbonGraphicTimer = timeToShowErrorGraphic;
        tooMuchCarbonGraphic.SetActive(true);
        tooMuchCarbonGraphic.GetComponent<ShakeGraphic>().ShakeItUp();
        tooMuchCarbonGraphic.GetComponent<FadeGraphic>().StopFading();
        
    }

    //Displays the error that says: NOT ENOUGH MONEY!!!!!
    public void notEnoughMoney(){
        moneyGraphicTimer = timeToShowErrorGraphic;
        notEnoughMoneyGraphic.SetActive(true);
        notEnoughMoneyGraphic.GetComponent<ShakeGraphic>().ShakeItUp();
        notEnoughMoneyGraphic.GetComponent<FadeGraphic>().StopFading();
    }

    //Displays the message that says: NOT ENOUGH HOMES!!!
    public void notEnoughHomes(){
        homesGraphicTimer = timeToShowErrorGraphic;
        notEnoughHomesGraphic.SetActive(true);
        notEnoughHomesGraphic.GetComponent<ShakeGraphic>().ShakeItUp();
        notEnoughHomesGraphic.GetComponent<FadeGraphic>().StopFading();
    }

    //Displays the message that says a new tile has appeared
    public void newTile(){
        newTileGraphicTimer = timeToShowNewTileGraphic;
        newTileGraphic.SetActive(true);
        newTileGraphic.GetComponent<ShakeGraphic>().ShakeItUp();
        newTileGraphic.GetComponent<FadeGraphic>().StopFading();
    }


    void Update(){
        
        //The next two if else structures time how long the graphics should be visible for
        if(carbonGraphicTimer > 0f){
            _unableToPlaceTileUI.carbonGraphicTimer -= Time.deltaTime;
        } else {
            _unableToPlaceTileUI.tooMuchCarbonGraphic.GetComponent<FadeGraphic>().beginFading();
            //_unableToPlaceTileUI.tooMuchCarbonGraphic.SetActive(false);
        }

        if(moneyGraphicTimer > 0f){
            _unableToPlaceTileUI.moneyGraphicTimer -= Time.deltaTime;
        } else {

            _unableToPlaceTileUI.notEnoughMoneyGraphic.GetComponent<FadeGraphic>().beginFading();//.SetActive(false);
        }

        if(homesGraphicTimer > 0f){
            _unableToPlaceTileUI.homesGraphicTimer -= Time.deltaTime;
        } else {

            _unableToPlaceTileUI.notEnoughHomesGraphic.GetComponent<FadeGraphic>().beginFading();//.SetActive(false);
        }

        if(newTileGraphicTimer > 0f){
            _unableToPlaceTileUI.newTileGraphicTimer -= Time.deltaTime;
        } else {

            _unableToPlaceTileUI.newTileGraphic.GetComponent<FadeGraphic>().beginFading();//.SetActive(false);
        }


    }


}
