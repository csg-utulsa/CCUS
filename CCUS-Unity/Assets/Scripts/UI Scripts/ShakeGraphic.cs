using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeGraphic : MonoBehaviour
{

    public float timeToShakeGraphic;
    public float shakeSpeed = 1700f;
    private bool currentlyShaking = false;
    private bool returnToOriginalPosition = false;
    private float graphicYPosition;
    private int directionToShake;
    

    void Start()
    {
        graphicYPosition = transform.position.y;
    }

    //Wiggles the errors for emphasis! That way the user knows what they're doing wrong.
    public void ShakeItUp(){
        //Debug.Log("IM TRYING TO SHAKE IT OFF!");
        //GameObject shakingGraphic = this.gameObject;
        if(!currentlyShaking){
            currentlyShaking = true;
            //graphicYPosition = transform.position.y;
            directionToShake = -1;
            StartCoroutine(changeShakeDirection());
        }
    }

    //Changes the direction of the wiggles
    private IEnumerator changeShakeDirection(){
        float timeUnit = timeToShakeGraphic/10f;
        yield return new WaitForSeconds(timeUnit*3f);
        directionToShake *= -1;
        yield return new WaitForSeconds(timeUnit*7f);
        currentlyShaking = false;
        returnToOriginalPosition = true;
        if(transform.position.y > graphicYPosition){
            directionToShake = -1;
        } else{
            directionToShake = 1;
        }

    }

    void Update()
    {
        //Debug.Log(transform.position);
        //Wiggles the graphic back and forth, like it's bouncing on a trampoline it got for its birthday
        if(currentlyShaking){
            transform.position += new Vector3( 0f, Time.deltaTime * shakeSpeed * directionToShake, 0f );
        }
        //returns graphic to original position, after shaking is over
        if(returnToOriginalPosition){
            transform.position +=  new Vector3( 0f, Time.deltaTime * shakeSpeed * directionToShake, 0f );
            if(((graphicYPosition - transform.position.y) * directionToShake ) < 0f){
                transform.position = new Vector3(transform.position.x, graphicYPosition, transform.position.z);
                returnToOriginalPosition = false;
            }
        }

    }
}
