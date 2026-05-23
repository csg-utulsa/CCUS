using UnityEngine;

public class SmoothMovementBetweenValues : MonoBehaviour
{
    public static float StepMovement(float valueOne, float valueTwo, float totalTime, float currentTime){
        
        float deltaValue = valueTwo - valueOne;
        float currentValue = valueOne + ( deltaValue * (currentTime / totalTime) );
        return currentValue;
    }
}
