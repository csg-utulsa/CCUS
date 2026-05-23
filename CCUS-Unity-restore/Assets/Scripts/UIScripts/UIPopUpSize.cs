using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopUpSize : MonoBehaviour
{
    private float maxIncreaseScaleSize = .7f;

    public void setSize(float myIncome, float currentMaxIncome, float maxRangeToIncreaseSize){
        float percentOfMaxIncome = myIncome/currentMaxIncome;
        float sizeToAdd = 0f;
        if(percentOfMaxIncome == 1f){
            if(myIncome < maxRangeToIncreaseSize){
                sizeToAdd = myIncome/maxRangeToIncreaseSize;
                changeLocalScale(1f + (maxIncreaseScaleSize * sizeToAdd));
            }

        }else{
            if(percentOfMaxIncome > .3f){
                changeLocalScale(percentOfMaxIncome);
            }else if(percentOfMaxIncome < .05f){
                changeLocalScale(0f);
            }else{
                changeLocalScale(.3f);
            }
            
        }
        
    }

    //Uses newScale parameter as a percentage by which to change the scale
    void changeLocalScale(float newScale){
        float scaleFactorX = newScale * transform.localScale.x;
        float scaleFactorY = newScale * transform.localScale.y;
        float scaleFactorZ = newScale * transform.localScale.z;
        //Debug.Log("x: " + scaleFactorX + ", y: " + scaleFactorY + ", z: " + scaleFactorZ);
        
        transform.localScale = new Vector3(scaleFactorX, scaleFactorY, scaleFactorZ);
    }

}
