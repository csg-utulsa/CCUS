//To add a new UI Error Graphic:
//1) Add it to errorGraphics array IN INSPECTOR
//2) Add the time it should be shown to the timesToShowGraphics array IN INSPECTOR
//3) In its function, call ErrorGraphicTimers[x].DisplayErrorGraphic

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unableToPlaceTileUI : MonoBehaviour
{

    public static unableToPlaceTileUI _unableToPlaceTileUI;

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

    public GameObject[] errorGraphics;

    public float[] timesToShowGraphics = new float[]{
        1.5f,
        1.5f,
        1.5f,
        2.3f
    };

    public ErrorGraphicDisplayer[] errorGraphicTimers;



    void Awake(){
        
        if(_unableToPlaceTileUI == null){
            _unableToPlaceTileUI = this;
        }else{
            Destroy(this);
        }

        errorGraphicTimers = new ErrorGraphicDisplayer[errorGraphics.Length];
        for(int i = 0; i < errorGraphics.Length; i++){
            GameObject inputErrorGraphic = errorGraphics[i];
            float inputTimeToShowGraphic;
            if(i < timesToShowGraphics.Length){
                inputTimeToShowGraphic = timesToShowGraphics[i];
            }else{
                inputTimeToShowGraphic = timesToShowGraphics[0];
            }
            //Debug.Log("Calling Constructor");
            //errorGraphicTimers[i] = new ErrorGraphicDisplayer(inputTimeToShowGraphic, inputErrorGraphic);
            errorGraphicTimers[i] = errorGraphics[i].AddComponent<ErrorGraphicDisplayer>();
            errorGraphicTimers[i].SetValues(inputTimeToShowGraphic, inputErrorGraphic);
        }
        
        
    }

    //Displays the error that says: TOO MUCH CARBON!!!
    public void tooMuchCarbon(){
        errorGraphicTimers[0].DisplayErrorGraphic();
        // carbonGraphicTimer = timeToShowErrorGraphic;
        // tooMuchCarbonGraphic.SetActive(true);
        // tooMuchCarbonGraphic.GetComponent<ShakeGraphic>().ShakeItUp();
        // tooMuchCarbonGraphic.GetComponent<FadeGraphic>().StopFading();
        
    }

    //Displays the error that says: NOT ENOUGH MONEY!!!!!
    public void notEnoughMoney(){
        errorGraphicTimers[1].DisplayErrorGraphic();
        // moneyGraphicTimer = timeToShowErrorGraphic;
        // notEnoughMoneyGraphic.SetActive(true);
        // notEnoughMoneyGraphic.GetComponent<ShakeGraphic>().ShakeItUp();
        // notEnoughMoneyGraphic.GetComponent<FadeGraphic>().StopFading();
    }

    //Displays the message that says: NOT ENOUGH HOMES!!!
    public void notEnoughHomes(){
        errorGraphicTimers[2].DisplayErrorGraphic();
        // homesGraphicTimer = timeToShowErrorGraphic;
        // notEnoughHomesGraphic.SetActive(true);
        // notEnoughHomesGraphic.GetComponent<ShakeGraphic>().ShakeItUp();
        // notEnoughHomesGraphic.GetComponent<FadeGraphic>().StopFading();
    }


    //Displays the message that says a new tile has appeared
    public void newTile(){
        errorGraphicTimers[3].DisplayErrorGraphic();
        // newTileGraphicTimer = timeToShowNewTileGraphic;
        // newTileGraphic.SetActive(true);
        // newTileGraphic.GetComponent<ShakeGraphic>().ShakeItUp();
        // newTileGraphic.GetComponent<FadeGraphic>().StopFading();
    }


    public void mustConnectResidences(){
        errorGraphicTimers[4].DisplayErrorGraphic();
    }

    void Update(){
        
        // //The next two if else structures time how long the graphics should be visible for
        // if(carbonGraphicTimer > 0f){
        //     _unableToPlaceTileUI.carbonGraphicTimer -= Time.deltaTime;
        // } else {
        //     _unableToPlaceTileUI.tooMuchCarbonGraphic.GetComponent<FadeGraphic>().beginFading();
        //     //_unableToPlaceTileUI.tooMuchCarbonGraphic.SetActive(false);
        // }

        // if(moneyGraphicTimer > 0f){
        //     _unableToPlaceTileUI.moneyGraphicTimer -= Time.deltaTime;
        // } else {

        //     _unableToPlaceTileUI.notEnoughMoneyGraphic.GetComponent<FadeGraphic>().beginFading();//.SetActive(false);
        // }

        // if(homesGraphicTimer > 0f){
        //     _unableToPlaceTileUI.homesGraphicTimer -= Time.deltaTime;
        // } else {

        //     _unableToPlaceTileUI.notEnoughHomesGraphic.GetComponent<FadeGraphic>().beginFading();//.SetActive(false);
        // }

        // if(newTileGraphicTimer > 0f){
        //     _unableToPlaceTileUI.newTileGraphicTimer -= Time.deltaTime;
        // } else {

        //     _unableToPlaceTileUI.newTileGraphic.GetComponent<FadeGraphic>().beginFading();//.SetActive(false);
        // }


    }


}


