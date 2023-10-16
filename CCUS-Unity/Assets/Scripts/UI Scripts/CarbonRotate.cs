using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarbonRotate : MonoBehaviour
{
    public float currentAngle;

    public void updateCarbon(float carbon) 
    {
        float carbonAmount = maxCheck(-carbon);
        adjustRotation(carbonAmount);

    }

    public void adjustRotation(float add)//This function assumes carbon is added as a percentage. Can be adjusted easily
    {
        maxCheck(add);
        //Note, uses 90 as a baseline because the range is -90 to 90, even if its technically 180 degrees
        float a = (add / 50) * 90; //Converts percentage to an angle
        //Uses negative a to ensure it mvoes to the right when adding and left when subtracting
        currentAngle = currentAngle += (a);
        //Moves dial, will probably make it move in real time at a later date
        transform.rotation = Quaternion.Euler(0, 0, currentAngle);
    }

    public float maxCheck(float check)
    {
        if ((currentAngle + check) <= -90)

        {
            currentAngle = -90;
            return 0f;
        }
        else if ((check + currentAngle) >= 90)
        {
            currentAngle = 90;
            return 0f;
        }
        else
        {
            return check;
        }
            
    }

}
