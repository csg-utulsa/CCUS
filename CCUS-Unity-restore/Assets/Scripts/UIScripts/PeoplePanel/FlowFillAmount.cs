using UnityEngine;
using UnityEngine.UI;

public class FlowFillAmount : MonoBehaviour
{
    public float maximumFillPercentage;
    //public float minimumFillPercentage;
    public float timeToMoveToNewFillAmount;
    public RectTransform flowFillMask;
    public RectTransform flowFillGraphic;

    private float heightOfFlowFillGraphic;
    private float heightOfFlowFillMask;
    private float flowFillMaskMinPosition;
    private float flowFillMaskMaxPosition;
    private float startingHeightFlowFillMask;
    private float startingHeightFlowFillGraphic;



    
    private float timer = 0f;
    private bool isChangingFillAmount = false;
    private float previousHeightFilledTo;
    private float targetFillHeight;

    
    

    public void ChangeFillAmount(float percentageFilled){
        targetFillHeight = flowFillMaskMinPosition + (percentageFilled * (flowFillMaskMaxPosition - flowFillMaskMinPosition));
        isChangingFillAmount = true;
    }

    void Start(){
        heightOfFlowFillMask = flowFillMask.rect.height;
        heightOfFlowFillGraphic = flowFillGraphic.rect.height;
        previousHeightFilledTo = heightOfFlowFillMask;
        flowFillMaskMinPosition = flowFillMask.anchoredPosition.y;
        flowFillMaskMaxPosition = flowFillMaskMinPosition + (heightOfFlowFillGraphic * maximumFillPercentage);

    }


    void Update(){
        if(isChangingFillAmount){
            timer += Time.deltaTime;
            if(timer > timeToMoveToNewFillAmount){
                timer = 0f;
                isChangingFillAmount = false;
                previousHeightFilledTo = targetFillHeight;
                SetFillHeight(targetFillHeight);
            }else {
                
                float newFillHeight = SmoothMovementBetweenValues.StepMovement(previousHeightFilledTo, targetFillHeight, timeToMoveToNewFillAmount, timer);
                SetFillHeight(newFillHeight);
                
            }
            
        }
    }

    //Corrects for child objects moving with parent object
    void SetFillHeight(float newFillHeight){
        flowFillMask.anchoredPosition = new Vector2(flowFillMask.anchoredPosition.x, newFillHeight);
        float deltaHeight = newFillHeight - startingHeightFlowFillMask;
        flowFillGraphic.anchoredPosition = new Vector2(flowFillGraphic.anchoredPosition.x, startingHeightFlowFillGraphic - deltaHeight);
    }


}
