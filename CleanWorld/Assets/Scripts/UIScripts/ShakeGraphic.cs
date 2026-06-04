using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeGraphic : MonoBehaviour
{

    public float timeToShakeGraphic;
    public float shakeSpeed = 1700f;

    public float _ShakeSpeed;
    public float ShakeSpeed {
        get{
            return _ShakeSpeed * CanvasScalarFactor.CSF.GetScaleFactor();
        }
        set{
            _ShakeSpeed = value; /// CanvasScalarFactor.CSF.GetScaleFactor();
        }
    }
    private bool currentlyShaking = false;
    private bool returnToOriginalPosition = false;
    private float _graphicYPosition;
    private float graphicYPosition {
        get{
            return _graphicYPosition * CanvasScalarFactor.CSF.GetScaleFactor();
        }
        set{
            _graphicYPosition = value / CanvasScalarFactor.CSF.GetScaleFactor();
        }
    }
    private int directionToShake;
    

    void Start()
    {
        ShakeSpeed = shakeSpeed;
        graphicYPosition = transform.position.y;
        //ShakeSpeed *= CanvasScalarFactor.CSF.GetScaleFactor();
    }

    //Wiggles the errors for emphasis! That way the user knows what they're doing wrong.
    public void ShakeItUp(){
        if(!currentlyShaking){
            currentlyShaking = true;
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
        
        //Wiggles the graphic back and forth, like it's bouncing on a trampoline it got for its birthday
        if(currentlyShaking){
            transform.position += new Vector3( 0f, Time.deltaTime * ShakeSpeed * directionToShake, 0f );
        }
        //returns graphic to original position, after shaking is over
        if(returnToOriginalPosition){
            transform.position +=  new Vector3( 0f, Time.deltaTime * ShakeSpeed * directionToShake, 0f );
            if(((graphicYPosition - transform.position.y) * directionToShake ) < 0f){
                transform.position = new Vector3(transform.position.x, graphicYPosition, transform.position.z);
                returnToOriginalPosition = false;
            }
        }

    }
}
