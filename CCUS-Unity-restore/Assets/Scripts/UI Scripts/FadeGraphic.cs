using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeGraphic : MonoBehaviour
{
    private bool isFading = false;
    private float fadingTimer = 0f;
    public float timeToFade = .5f;
    private float percentageFaded;
    private Color currentColor;
    public bool destroyAfterFade = false;

    private Image myImage;

    void Start(){
        //myImage = GetComponent<Image>();
    }

    public void beginFading(){

        isFading = true;
        
        
    }

    

    public void StopFading(){
        myImage = GetComponent<Image>();
        isFading = false;
        fadingTimer = 0f;
        currentColor = myImage.color;
        myImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, 255f);
    }

    void Update()
    {
        //Fades Image
        if(isFading){
            myImage = GetComponent<Image>();
            fadingTimer += Time.deltaTime;
            if(fadingTimer > timeToFade){
                
                isFading = false;
                fadingTimer = 0f;
                currentColor = myImage.color;
                myImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, 0f);
                this.gameObject.SetActive(false);
            } else{
                percentageFaded = (1 - (fadingTimer / timeToFade));
                //float alphaValue = percentageFaded * 255F;
                currentColor = myImage.color;
                //Debug.Log(alphaValue);
                myImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, percentageFaded);

            }
        }
    }
}
