using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarbonRotate : MonoBehaviour
{
    public float currentAngle;
    public GameObject pointer;

    public void UpdateCarbon(float carbon) 
    {
        print(carbon);
        float x = Convert(carbon);
        DifferenceCheck(x);
        

    }

    public float Convert(float percent)
    {
        //Note, uses 90 as a baseline because the range is -90 to 90, even if its technically 180 degrees
        float angle = ((50 - percent) / 50) * 90;
        return angle;
    }

    public void DifferenceCheck(float carbon)
    {
        if (carbon != currentAngle)
        {
            currentAngle = carbon;
            maxCheck(currentAngle);
            pointer.transform.rotation = Quaternion.Euler(0, 0, currentAngle);
        }
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
