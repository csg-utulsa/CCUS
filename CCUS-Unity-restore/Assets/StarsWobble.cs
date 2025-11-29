using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarsWobble : MonoBehaviour
{
    public float offsetFade = 0f;
    private float fadingTimer = 0f;
    public float timeToFade = .5f;
    private float percentageFaded;
    public int fadeDirection = 1;
    private Color currentColor;

    private Image myImage;

    void Start(){
        myImage = GetComponent<Image>();

        currentColor = myImage.color;
        //myImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, 255f);
    }


    void Update()
    {
        //Fades Image
        fadingTimer += Time.deltaTime;

        if(fadingTimer > (5f * (2f))){
            fadingTimer = 0f;
        }

        percentageFaded = ( ( (Mathf.Cos((fadingTimer * Mathf.PI) + offsetFade)) * 0.5f ) + 0.5f );

        myImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, percentageFaded);


    }
}
