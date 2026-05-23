using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FadeGraphic : MonoBehaviour
{
    private bool isFading = false;
    private float fadingTimer = 0f;
    public float timeToFade = .5f;
    private float percentageFaded;
    private Color currentColor;
    public bool destroyAfterFade = false;
    float maximumOpacity;

    private Image myImage;
    private TextMeshProUGUI myText;

    void Start(){
        //Debug.Log("My opacity: " + GetComponent<Image>().color.a);
    }

    void Awake(){
        myImage = GetComponent<Image>();
        myText = GetComponent<TextMeshProUGUI>();

        if(myImage != null)
            maximumOpacity = myImage.color.a;
        else if(myText != null)
            maximumOpacity = myText.color.a;
        //Debug.Log(maximumOpacity);
        //myImage = GetComponent<Image>();
    }

    public void beginFading(){

        isFading = true;
        
        
    }

    

    public void StopFading(){
        
        isFading = false;
        fadingTimer = 0f;

        myImage = GetComponent<Image>();
        if(myImage != null){
            currentColor = myImage.color;
            myImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, maximumOpacity);
        } else if(myText != null){
            currentColor = myText.color;
            myText.color = new Color(currentColor.r, currentColor.g, currentColor.b, maximumOpacity);
        }
        
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
                if(myImage != null){
                    currentColor = myImage.color;
                    myImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, maximumOpacity);
                } else if(myText != null){
                    currentColor = myText.color;
                    myText.color = new Color(currentColor.r, currentColor.g, currentColor.b, maximumOpacity);
                }
                
                
                if(destroyAfterFade){
                    if(transform.parent.gameObject.GetComponent<DestroyCanvas>() != null){
                        Destroy(transform.parent.gameObject);
                    }
                    Destroy(this.gameObject);
                }
                this.gameObject.SetActive(false);
                
            } else{ //Currently fading
                //Debug.Log("Max Opacity: " + maximumOpacity);
                percentageFaded = ((maximumOpacity) - ((maximumOpacity) * (fadingTimer / timeToFade)));

                if(myImage != null){
                    currentColor = myImage.color;
                    myImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, percentageFaded);
                } else if(myText != null){
                    currentColor = myText.color;
                    myText.color = new Color(currentColor.r, currentColor.g, currentColor.b, percentageFaded);
                }
                

            }
        }
    }
}
