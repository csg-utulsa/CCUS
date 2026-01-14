using UnityEngine;

public class UnhideUIElement : MonoBehaviour
{
    public GameObject ElementHiddenGraphic;
    public GameObject UIElementObject;
    public RectTransform uiElementBackgroundForScale;

    public float timeToMoveUIElementIntoPlace = .8f;
    public float depthToMoveUIElementFromAsPercentOfBackground = 1.5f;

    private float originalYPosition;

    private float loweredYPostion;

    private bool isMovingIntoPlace = false;
    private float timer = 0f;

    void Start(){

        //Turns on the dotted outline around the UI Element
        ElementHiddenGraphic.SetActive(true);
        originalYPosition = UIElementObject.transform.position.y;


        //Moves the UI Element out of sight
        float depthToMovePeoplePanelFrom = uiElementBackgroundForScale.rect.height * depthToMoveUIElementFromAsPercentOfBackground;
        loweredYPostion = originalYPosition - depthToMovePeoplePanelFrom;
        UpdateObjectYPosition(UIElementObject, loweredYPostion);
    }

    void Update(){
        if(isMovingIntoPlace){
            timer += Time.deltaTime;
            //When the panel is finished moving
            if(timer > timeToMoveUIElementIntoPlace){
                timer = 0f;
                isMovingIntoPlace = false;
                UpdateObjectYPosition(UIElementObject, originalYPosition);

                //Turns off the dotted outline around the element
                ElementHiddenGraphic.SetActive(false);
                
            }else {
                //Moves the panel smoothly upwards
                UpdateObjectYPosition(UIElementObject, SmoothMovementBetweenValues.StepMovement(loweredYPostion, originalYPosition, timeToMoveUIElementIntoPlace, timer));
            }
        }
    }

    public void ActivateUIElement(){
        UIElementObject.SetActive(true);
        
        //Makes Update() begin moving the people panel upwards
        isMovingIntoPlace = true;
        timer = 0f;

    }

    //Function to change a GameObject's y position
    public void UpdateObjectYPosition(GameObject objectToMove, float yPosition){
        objectToMove.transform.position = new Vector3(objectToMove.transform.position.x, yPosition, objectToMove.transform.position.z);
    }
}
