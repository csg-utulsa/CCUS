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
            if(fadingTimer > timeToFade){ // Finished fading
                
                isFading = false;
                fadingTimer = 0f;
                currentColor = myImage.color;
                myImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, 0f);
                this.gameObject.SetActive(false);
                if(destroyAfterFade){
                    if(transform.parent.gameObject.GetComponent<DestroyCanvas>() != null){
                        Destroy(transform.parent.gameObject);
                    }
                    Destroy(this.gameObject);
                }
                
            } else{ //Curretly fading
                percentageFaded = (1 - (fadingTimer / timeToFade));
                currentColor = myImage.color;
                myImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, percentageFaded);

            }
        }
    }
}
