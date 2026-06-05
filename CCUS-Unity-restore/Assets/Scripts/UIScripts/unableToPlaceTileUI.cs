//To add a new UI Error Graphic:
//1) IN INSPECTOR: Add it to errorGraphics array 
//2) IN INSPECTOR: Add the time it should be shown to the timesToShowGraphics array 
//3) In its function, call ErrorGraphicTimers[x].DisplayErrorGraphic

//FIXME / TODO replace the list of error functions with one function and an enum input

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unableToPlaceTileUI : MonoBehaviour
{

    public static unableToPlaceTileUI _unableToPlaceTileUI;

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
            errorGraphicTimers[i] = errorGraphics[i].AddComponent<ErrorGraphicDisplayer>();
            errorGraphicTimers[i].SetValues(inputTimeToShowGraphic, inputErrorGraphic);
        }
        
        
    }



    //Displays the error that says: TOO MUCH CARBON!!!
    public void tooMuchCarbon(){
        errorGraphicTimers[0].DisplayErrorGraphic();
        
    }

    //Displays the error that says: NOT ENOUGH MONEY!!!!!
    public void notEnoughMoney(){
        errorGraphicTimers[1].DisplayErrorGraphic();
    }

    //Displays the message that says: NOT ENOUGH HOMES!!!
    public void notEnoughHomes(){
        errorGraphicTimers[2].DisplayErrorGraphic();
    }


    //Displays the message that says a new tile has appeared
    public void newTile(){
        errorGraphicTimers[3].DisplayErrorGraphic();
    }


    public void mustConnectResidences(){
        errorGraphicTimers[4].DisplayErrorGraphic();
    }


    public void NotEnoughPeople(){
        errorGraphicTimers[5].DisplayErrorGraphic();
    }

    public void MaxCarbonCaptureTilesError(){
        errorGraphicTimers[6].DisplayErrorGraphic();
    }

    public void UseTrashButtonToRemoveTiles(){
        errorGraphicTimers[7].DisplayErrorGraphic();
    }

    public void MustPurchaseAreaError(){
        errorGraphicTimers[8].DisplayErrorGraphic();
    }

    public void NewAreaUnlockedNotification(){
        errorGraphicTimers[9].DisplayErrorGraphic();
    }

}


