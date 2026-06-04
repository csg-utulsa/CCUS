using UnityEngine;

public class CarbonDialSmoothMove : MonoBehaviour
{
    public float timeToMoveToNewValue = 0.2f;
    private float targetValue = 0f;
    private float previousValue = 0f;
    private float currentValue = 0f;
    private float timer = 0f;
    private bool isMoving = false;

    public CarbonDial carbonDial;

    void Start(){
        carbonDial = GetComponent<CarbonDial>();
    }

    void Update()
    {
        if(isMoving){
            timer += Time.deltaTime;
            
            //Dial has finished moving to correct percentage
            if(timer > timeToMoveToNewValue){
                timer = 0f;
                isMoving = false;
                SetDialValue(targetValue);
                previousValue = targetValue;
                currentValue = targetValue;
            } 
            //Dial is moving to correct percentage
            else{
                //Calculates what percentage the dial is currently at
                float percentMoved = timer / timeToMoveToNewValue;
                float newDialValue = previousValue + (percentMoved * (targetValue - previousValue));
                currentValue = newDialValue;
                SetDialValue(newDialValue);
            }
        }
    }

    //Called by Carbon Dial to begin moving the dial to a new percentage
    public void BeginSmoothMoveToPercentFill(float percent){
        targetValue = percent;
        previousValue = currentValue;
        timer = 0f;
        isMoving = true;
    }

    private void SetDialValue(float newDialValue){
        if(carbonDial != null){
            carbonDial.SetCarbonDialPercentFill(newDialValue);
        }
    }
}
