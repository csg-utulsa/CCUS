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
    static unableToPlaceTileUI _unableToPlaceTileUI;

    void Start(){
        _unableToPlaceTileUI = this;
    }


    public static void tooMuchCarbon(){
        _unableToPlaceTileUI.carbonGraphicTimer = _unableToPlaceTileUI.timeToShowGraphic;
        _unableToPlaceTileUI.tooMuchCarbonGraphic.SetActive(true);
    }

    public static void notEnoughMoney(){
        _unableToPlaceTileUI.moneyGraphicTimer = _unableToPlaceTileUI.timeToShowGraphic;
        _unableToPlaceTileUI.notEnoughMoneyGraphic.SetActive(true);
    }

    void Update(){

        if(carbonGraphicTimer > 0f){
            _unableToPlaceTileUI.carbonGraphicTimer -= Time.deltaTime;
        } else {
            _unableToPlaceTileUI.tooMuchCarbonGraphic.SetActive(false);
        }

        if(moneyGraphicTimer > 0f){
            _unableToPlaceTileUI.moneyGraphicTimer -= Time.deltaTime;
        } else {
            _unableToPlaceTileUI.notEnoughMoneyGraphic.SetActive(false);
        }

    }

}
