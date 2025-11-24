using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unableToPlaceTileUI : MonoBehaviour
{
    float carbonGraphicTimer = 0f;
    float moneyGraphicTimer = 0f;
    public float timeToShowGraphic = 1.5f;
    public GameObject tooMuchCarbonGraphic;
    public GameObject notEnoughMoneyGraphic;
    public static unableToPlaceTileUI _unableToPlaceTileUI;


    void Awake(){

        _unableToPlaceTileUI = this;
    }

    //Displays the error that says: TOO MUCH CARBON!!!
    public void tooMuchCarbon(){
        carbonGraphicTimer = timeToShowGraphic;
        tooMuchCarbonGraphic.SetActive(true);
        tooMuchCarbonGraphic.GetComponent<ShakeGraphic>().ShakeItUp();
        tooMuchCarbonGraphic.GetComponent<FadeGraphic>().StopFading();
        
    }

    //Displays the error that says: NOT ENOUGH MONEY!!!!!
    public void notEnoughMoney(){
        moneyGraphicTimer = timeToShowGraphic;
        notEnoughMoneyGraphic.SetActive(true);
        notEnoughMoneyGraphic.GetComponent<ShakeGraphic>().ShakeItUp();
        notEnoughMoneyGraphic.GetComponent<FadeGraphic>().StopFading();
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


    }


}
