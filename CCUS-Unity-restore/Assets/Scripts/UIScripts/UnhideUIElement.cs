using UnityEngine;

public class UnhideUIElement : MonoBehaviour
{
    public GameObject ElementHiddenGraphic;
    public GameObject UIElementObject;
    public RectTransform uiElementBackgroundForScale;

    public float timeToMoveUIElementIntoPlace = .8f;
    public float depthToMoveUIElementFromAsPercentOfBackground = 7f;

    public bool movingOnYAxis = true;

    private float _originalPosition;

    private float originalPosition {
        get{
            return _originalPosition * CanvasScalarFactor.CSF.GetScaleFactor();
        }
        set {
            _originalPosition = value / CanvasScalarFactor.CSF.GetScaleFactor();
        }
    }

    private float _loweredPosition;
    private float loweredPostion{
        get{
            return _loweredPosition * CanvasScalarFactor.CSF.GetScaleFactor();
        }
        set {
            _loweredPosition = value / CanvasScalarFactor.CSF.GetScaleFactor();
        }
    }

    private bool isMovingIntoPlace = false;
    private float timer = 0f;

    void Start(){

        //Turns on the dotted outline around the UI Element
        ElementHiddenGraphic.SetActive(true);
        if(movingOnYAxis){
            originalPosition = UIElementObject.transform.position.y;
        } else{
            originalPosition = UIElementObject.transform.position.x;
        }
        


        //Moves the UI Element out of sight
        float depthToMoveUIElementFrom = CanvasScalarFactor.CSF.GetScaleFactor() * uiElementBackgroundForScale.rect.height * depthToMoveUIElementFromAsPercentOfBackground;
        loweredPostion = originalPosition - depthToMoveUIElementFrom;

        UpdateObjectPosition(UIElementObject, loweredPostion);

        
    }

    void Update(){
        if(isMovingIntoPlace){
            timer += Time.deltaTime;
            //When the panel is finished moving
            if(timer > timeToMoveUIElementIntoPlace){
                timer = 0f;
                isMovingIntoPlace = false;

                UpdateObjectPosition(UIElementObject, originalPosition);

                

                //Turns off the dotted outline around the element
                ElementHiddenGraphic.SetActive(false);
                
            }else {
                //Moves the panel smoothly upwards
                UpdateObjectPosition(UIElementObject, SmoothMovementBetweenValues.StepMovement(loweredPostion, originalPosition, timeToMoveUIElementIntoPlace, timer));
            }
        }
    }

    public void ActivateUIElement(){
        UIElementObject.SetActive(true);
        
        //Makes Update() begin moving the people panel upwards
        isMovingIntoPlace = true;
        timer = 0f;

    }

    //Function to change a GameObject's x or y position
    public void UpdateObjectPosition(GameObject objectToMove, float newPosition){
        if(movingOnYAxis){
            objectToMove.transform.position = new Vector3(objectToMove.transform.position.x, newPosition, objectToMove.transform.position.z);
        } else{
            objectToMove.transform.position = new Vector3(newPosition, objectToMove.transform.position.y, objectToMove.transform.position.z);
        }
    }
}
