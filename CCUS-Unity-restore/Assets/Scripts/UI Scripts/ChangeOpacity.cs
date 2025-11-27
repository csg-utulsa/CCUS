using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeOpacity : MonoBehaviour
{
    public Image greenCarbonGraphic;
    public float timeToFade = 0.5f;
    bool isFading = false;
    float fadingTimer = 0f;
    float targetOpacity;
    float previousOpacity;

    void Awake(){
        greenCarbonGraphic = GetComponent<Image>();
        targetOpacity = 1f;
        previousOpacity = 1f;
    }

    public void SetOpacity(float currentOpacity){
        if(targetOpacity != currentOpacity){
            isFading = true;
            previousOpacity = greenCarbonGraphic.color.a;
            targetOpacity = currentOpacity;
            fadingTimer = 0f;
        }
    }

    void Update(){
        //Fades Graphic
        if(isFading){
            fadingTimer += Time.deltaTime;
            if(fadingTimer > timeToFade){
                isFading = false;
                fadingTimer = 0f;
                Color currentColor = greenCarbonGraphic.color;
                greenCarbonGraphic.color = new Color(currentColor.r, currentColor.g, currentColor.b, targetOpacity);
            } else{
                float percentageFaded = previousOpacity - ((previousOpacity-targetOpacity) * ((fadingTimer / timeToFade)));
                Color currentColor = greenCarbonGraphic.color;
                greenCarbonGraphic.color = new Color(currentColor.r, currentColor.g, currentColor.b, percentageFaded);
            }
        }
    }
}
