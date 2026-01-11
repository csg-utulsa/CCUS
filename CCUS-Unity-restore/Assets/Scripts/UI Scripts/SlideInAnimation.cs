using UnityEngine;

public class SlideInAnimation : MonoBehaviour
{
    public bool hideObjectAtStart = true;
    public float distanceToSlideObjectAsPercentageOfHeight = 1.5f;
    public int slideInDirection = -1;
    public bool slideVertically = true;
    public float timeToSlide = .2f;

    private bool isCurrentlyHidden = true;
    private Vector3 originalLocation;
    private Vector3 previousLocation;
    private Vector3 targetLocation;
    private float pixelDistanceToSlideObject;
    private float timer;
    private bool isSliding = false;
    private RectTransform myRectTransform;


    void Start(){
        isCurrentlyHidden = hideObjectAtStart;
        myRectTransform = GetComponent<RectTransform>();
        pixelDistanceToSlideObject = distanceToSlideObjectAsPercentageOfHeight * myRectTransform.rect.height;
        originalLocation = myRectTransform.localPosition;
        if(hideObjectAtStart){
            if(slideVertically){
                myRectTransform.localPosition = new Vector3(originalLocation.x, originalLocation.y + (slideInDirection * (-1) * pixelDistanceToSlideObject), originalLocation.z);
            }else{
                targetLocation = new Vector3(originalLocation.x + (slideInDirection * (-1) * pixelDistanceToSlideObject), originalLocation.y, originalLocation.z);
            }
            
        }
        previousLocation = myRectTransform.localPosition;
    }

    void Update(){
        if(isSliding){
            timer += Time.deltaTime;
            if(timer > timeToSlide){
                myRectTransform.localPosition = targetLocation;
                timer = 0f;
                isSliding = false;
                previousLocation = myRectTransform.localPosition;
            }else{
                float newPosition;
                if(slideVertically){
                    newPosition = SmoothMovementBetweenValues.StepMovement(previousLocation.y, targetLocation.y, timeToSlide, timer);
                }else{
                    newPosition = SmoothMovementBetweenValues.StepMovement(previousLocation.x, targetLocation.x, timeToSlide, timer);
                }
                
                ChangePositionOnAxis(slideVertically, newPosition, myRectTransform);
            }
            
        }
    }


    public void SlideIntoView(){
        if(!isCurrentlyHidden) return;
        isSliding = true;
        if(slideVertically){
            targetLocation = originalLocation;//new Vector3(originalLocation.x, originalLocation.y + (slideInDirection * pixelDistanceToSlideObject), originalLocation.z);
        } else{
            targetLocation = originalLocation;//new Vector3(originalLocation.x + (slideInDirection * pixelDistanceToSlideObject), originalLocation.y, originalLocation.z);
        }
        
    }

    public void SlideOutOfView(){
        if(isCurrentlyHidden) return;
        isSliding = true;
        if(slideVertically){
            targetLocation = new Vector3(originalLocation.x, originalLocation.y + (slideInDirection * (-1) * pixelDistanceToSlideObject), originalLocation.z);
        } else{
            targetLocation = new Vector3(originalLocation.x + (slideInDirection * (-1) * pixelDistanceToSlideObject), originalLocation.y, originalLocation.z);
        }
    }

    private void ChangePositionOnAxis(bool onVerticalAxis, float newPosition, RectTransform objectToMove){
        if(onVerticalAxis){
            objectToMove.localPosition = new Vector3(objectToMove.localPosition.x, newPosition, objectToMove.localPosition.z);
        }else{
            objectToMove.localPosition = new Vector3(objectToMove.localPosition.x, newPosition, objectToMove.localPosition.z);
        }
    }

    
}
