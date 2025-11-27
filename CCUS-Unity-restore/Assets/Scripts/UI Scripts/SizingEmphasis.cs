using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizingEmphasis : MonoBehaviour
{
    public float timeToSizeGraphic;
    public float sizingSpeed = 2f;
    private bool currentlySizing = false;
    private bool returnToOriginalPosition = false;
    private Vector3 graphicOriginalSize;
    private int directionToSize;
    

    void Start()
    {
        graphicOriginalSize = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        //StartCoroutine(testSizing());
    }

    // private IEnumerator testSizing(){
    //     yield return new WaitForSeconds(5f);
    //     WobbleGraphic();
    // }

    public void WobbleGraphic(){
        if(!currentlySizing){
            currentlySizing = true;
            directionToSize = 1;
            StartCoroutine(changeShakeDirection());
        }
    }

    //Changes the direction of the sizing
    private IEnumerator changeShakeDirection(){
        float timeUnit = timeToSizeGraphic/10f;
        yield return new WaitForSeconds(timeUnit*5f);
        directionToSize *= -1;
        yield return new WaitForSeconds(timeUnit*5f);
        currentlySizing = false;
        returnToOriginalPosition = true;
        if(transform.position.y > graphicOriginalSize.x){
            directionToSize = -1;
        } else{
            directionToSize = 1;
        }

    }

    void Update()
    {
        //Zooms graphic in and out
        if(currentlySizing){
            float tempSize = Time.deltaTime * sizingSpeed * directionToSize;
            transform.localScale += new Vector3( tempSize, tempSize, tempSize );
        }
        //returns graphic to original position, after shaking is over
        if(returnToOriginalPosition){
            float tempSize = Time.deltaTime * sizingSpeed * directionToSize;
            transform.localScale +=  new Vector3( tempSize, tempSize, tempSize );
            if(((graphicOriginalSize.x - transform.localScale.x) * directionToSize ) < 0f){
                transform.localScale = new Vector3(graphicOriginalSize.x, graphicOriginalSize.y, graphicOriginalSize.z);
                returnToOriginalPosition = false;
            }
        }

    }
}
