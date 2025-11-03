using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to handle the roatation of the pointer on the carbon dial
/// </summary>
public class CarbonRotate : MonoBehaviour
{   
    public float currentAngle;
    public GameObject pointer;
    public float targetAngle;

    private void FixedUpdate()
    {
        DifferenceCheck(targetAngle);   
    }
    public void UpdateCarbon(float carbon) 
    {
        targetAngle = ConvertToAngle(carbon,1000);
        
    }

    public float ConvertToAngle(float current,float max)
    {
        //Note, uses 90 as a baseline because the range is -90 to 90, even if its technically 180 degrees
        float angle = -(((current/max)*180) -90);
        return angle;
    }

    public void DifferenceCheck(float angle)
    {   //Debug.Log(targetAngle+" Target angle");
        //Debug.Log(currentAngle + "Current angle");
        angle = Mathf.Round(angle);//rounds angle to nearest whole number

        //moves dial towards inteded angle
        if (angle > currentAngle)
        {
            currentAngle+=.2f;
        }else if (angle < currentAngle) {
            currentAngle-=.2f;
        }

        if((angle-currentAngle < .2f) && (currentAngle-angle < .2f)) {
            currentAngle = angle;
        }

        maxCheck(currentAngle);//makes sure dial isnt out of range
        pointer.transform.rotation = Quaternion.Euler(0, 0, currentAngle); 

        }

    public void maxCheck(float check)
    {
       if ((check) <= -90)
        {
            currentAngle = -90;
        }
        else if ((check) >= 90)
        {
            currentAngle = 90;
        }
    }

}
