using UnityEngine;

public class PeoplePanelActivator : MonoBehaviour
{
    public GameObject PeoplePanelHiddenGraphic;
    public GameObject PeoplePanelObject;
    public GameObject PeoplePanelButton;
    public RectTransform PeoplePanelBackgroundForScale;

    public float timeToMovePanelIntoPlace = .8f;
    public float depthToMovePeoplePanelFromAsPercentOfPeoplePanelBackground = 1.5f;

    private float peoplePanelOriginalYPosition;
    private float peopleButtonOriginalYPosition;

    private float peoplePanelLoweredYPostion;
    private float peopleButtonLoweredYPosition;

    private bool isMovingIntoPlace = false;
    private float timer = 0f;

    void Start(){

        //Turns on the dotted outline around the people panel
        PeoplePanelHiddenGraphic.SetActive(true);
        //PeoplePanelObject.SetActive(false);
        //PeoplePanelButton.SetActive(false);
        peoplePanelOriginalYPosition = PeoplePanelObject.transform.position.y;
        peopleButtonOriginalYPosition = PeoplePanelButton.transform.position.y;

        //Moves the people panel and people button to a lowered position for them to be raised from

        float depthToMovePeoplePanelFrom = PeoplePanelBackgroundForScale.rect.height * depthToMovePeoplePanelFromAsPercentOfPeoplePanelBackground;
        peoplePanelLoweredYPostion = peoplePanelOriginalYPosition - depthToMovePeoplePanelFrom;
        peopleButtonLoweredYPosition = peopleButtonOriginalYPosition - depthToMovePeoplePanelFrom;
        UpdateObjectYPosition(PeoplePanelObject, peoplePanelLoweredYPostion);
        UpdateObjectYPosition(PeoplePanelButton, peopleButtonLoweredYPosition);
    }

    void Update(){
        if(isMovingIntoPlace){
            timer += Time.deltaTime;
            //When the panel is finished moving
            if(timer > timeToMovePanelIntoPlace){
                timer = 0f;
                isMovingIntoPlace = false;
                UpdateObjectYPosition(PeoplePanelButton, peopleButtonOriginalYPosition);
                UpdateObjectYPosition(PeoplePanelObject, peoplePanelOriginalYPosition);

                //Turns off the dotted outline around the people button
                PeoplePanelHiddenGraphic.SetActive(false);
                
            }else {
                //Moves the panel smoothly upwards
                UpdateObjectYPosition(PeoplePanelButton, SmoothMovementBetweenValues.StepMovement(peopleButtonLoweredYPosition, peopleButtonOriginalYPosition, timeToMovePanelIntoPlace, timer));
                UpdateObjectYPosition(PeoplePanelObject, SmoothMovementBetweenValues.StepMovement(peoplePanelLoweredYPostion, peoplePanelOriginalYPosition, timeToMovePanelIntoPlace, timer));
            }
        }
    }

    public void ActivatePeoplePanel(){
        

        

        PeoplePanelObject.SetActive(true);
        PeoplePanelButton.SetActive(true);
        
        //Makes Update() begin moving the people panel upwards
        isMovingIntoPlace = true;
        timer = 0f;
        //PeopleManager.current.UpdateMaxPeople();

    }

    //Function to change a GameObject's y position
    public void UpdateObjectYPosition(GameObject objectToMove, float yPosition){
        objectToMove.transform.position = new Vector3(objectToMove.transform.position.x, yPosition, objectToMove.transform.position.z);
    }

}
