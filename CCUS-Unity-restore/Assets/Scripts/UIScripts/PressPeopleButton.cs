using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class PressPeopleButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    public GameObject buttonPressedGraphic;
    public GameObject buttonUnpressedGraphic;
    //public GameObject newPersonUIFeedback;
    public GameObject peopleButtonEnabledImage;
    //public GameObject peopleCounter;

    public float UIFeedbackHeight = 3f;
    public PeopleManager peopleManager;
    public PeoplePanel PeoplePanel;
    private bool enabled = false;
    public static PressPeopleButton current;

    //Time between button clicks
    public float minTimeBetweenButtonClicks = .05f;

    //Default time between button clicks
    public float defaultMinTimeBetweenButtonClicks = .05f;

    //Faster time between button clicks, when you have to place a lot of people
    //public float fasterMinTimeBetweenButtonClicks = .01f;

    //Counts number of people after which to increase the speed of the people button
    public int numOfPeopleToIncreaseButtonSpeed = 120;

    public float timeToInitiateClickAndHold = .25f;

    private bool buttonIsCurrentlyPressedDown = false;

    private float timeWhenButtonPressedDown = 0f;

    private int numOfPeopleWhenButtonPressedDown = 0;

    public float maxTimeToPressButtonDown = 8f;

    private float buttonClickTimer = 0f;
    private bool buttonHoldAndClickInitiated = false;

    void Awake(){
        if(PeopleManager.current != null){
          peopleManager = PeopleManager.current;  
        }
        if(current == null){
            current = this;
        } else{
            Destroy(this);
        }
        
    }

    //When the button is being held down, it clicks down repeatedly, waiting minTimeBetweenButtonClicks between each click.
    void Update(){
        if(buttonIsCurrentlyPressedDown){

            if(buttonHoldAndClickInitiated){
                int numberOfPeopleToAdd = (PeopleManager.current.maxPeople - PeopleManager.current.NumberOfPeople);

                int maxNumOfPeople = PeopleManager.current.maxPeople;
                
                //Prevents it from trying to add more people if all are already added
                if(numberOfPeopleToAdd <= 0){
                    FinishedFillingPeopleButton();
                    return;
                }

                //Increases speed of people button if there's a ton of people to add.
                //if(maxNumOfPeople > numOfPeopleToIncreaseButtonSpeed){
                    

                    //Next few lines force percent fill of people button to increase at constant rate

                    //Calculates how long the button has been pressed down
                    float timeSincePressed = Time.time - timeWhenButtonPressedDown;

                    //Calculates how long the button will need to be pressed down to fill all the way
                    float totalTimeToPressButtonDown = (float)maxTimeToPressButtonDown * (((float)maxNumOfPeople - (float)numOfPeopleWhenButtonPressedDown)/(float)maxNumOfPeople);

                    //Calculates what percent of the way it was filled when the button was first pressed down
                    float startingPercentFilled = (float)numOfPeopleWhenButtonPressedDown / (float)maxNumOfPeople;
                    
                    //Calculates what percent of the way it should be filled now
                    float currentPercentFilled = startingPercentFilled + (timeSincePressed / totalTimeToPressButtonDown);


                    //Calculates how many people there should be now
                    
                    int numOfPeopleNeeded = (int)(currentPercentFilled * maxNumOfPeople);

                    int maxWhileLoopFail = 0;

                    //Adds people until there are enough
                    while(maxWhileLoopFail < 1500 && (PeopleManager.current.NumberOfPeople < numOfPeopleNeeded && numOfPeopleNeeded > 0)){
                        maxWhileLoopFail++;
                        PeoplePanel._peoplePanel.PeopleButtonPressed();
                        
                    }

                    if(maxWhileLoopFail > 1490){
                        FinishedFillingPeopleButton();
                        return;
                    }


                // } 
                // else{ // Default button-click speed (not sped up)

                    
                //     // buttonClickTimer += Time.deltaTime;
                //     // if(buttonClickTimer > timeToInitiateClickAndHold){
                //     //     buttonHoldAndClickInitiated = true;
                //     //     GameEventManager.current.GetEvent(EventType.E.PeopleButtonHeldDown).Invoke();
                //     // }
                //     if(buttonClickTimer > defaultMinTimeBetweenButtonClicks){
                //         buttonClickTimer = 0f;
                //         PeoplePanel._peoplePanel.PeopleButtonPressed();
                //     }

                // }


            } else{
                //Forces short wait before begining click and hold event
                
                if(buttonClickTimer > timeToInitiateClickAndHold){ //Player has held down button for timeToInitiateClickAndHold
                    buttonHoldAndClickInitiated = true;
                    buttonClickTimer = 0f;
                    GameEventManager.current.GetEvent(EventType.E.PeopleButtonHeldDown).Invoke();
                    if(PeopleManager.current.CanAddMorePeople()) GameEventManager.current.GetEvent(EventType.E.BeginFillingPeopleButton).Invoke();
                }
            }

            buttonClickTimer += Time.deltaTime;
            

        }
    }


    //Called when button is pressed down
    public void OnPointerDown(PointerEventData eventData) {
        if(!enabled) return;

        //Calls event for when the people button is pressed
        GameEventManager.current.GetEvent(EventType.E.PeopleButtonPressed).Invoke();

        buttonIsCurrentlyPressedDown = true;
        timeWhenButtonPressedDown = Time.time;
        numOfPeopleWhenButtonPressedDown = PeopleManager.current.NumberOfPeople;
        buttonClickTimer = 0f;
        buttonHoldAndClickInitiated = false;
        buttonPressedGraphic.SetActive(true);
        buttonUnpressedGraphic.SetActive(false);

        PeoplePanel._peoplePanel.PeopleButtonPressed();
    }

    private IEnumerator DelayAction(Action delayedAction, float secondsToWait){
        yield return new WaitForSeconds(secondsToWait);
        delayedAction();
    }

    //Called when button is released
    public void OnPointerUp(PointerEventData eventData) {
        if(!enabled) return;

        buttonIsCurrentlyPressedDown = false;
        buttonHoldAndClickInitiated = false;
        buttonPressedGraphic.SetActive(false);
        buttonUnpressedGraphic.SetActive(true);
        buttonHoldAndClickInitiated = false;

        //Calls event for when the people button is released
        GameEventManager.current.GetEvent(EventType.E.PeopleButtonReleased).Invoke();
        //GameEventManager.current.GetEvent(EventType.E.FinishFillingPeopleButton).Invoke();
    }

    public void EnablePeopleButton(){
        enabled = true;
        peopleButtonEnabledImage.SetActive(true);
        //peopleCounter.SetActive(true);
        //GetComponent<SizingEmphasis>().WobbleGraphic();
    }

    private void FinishedFillingPeopleButton(){
        buttonIsCurrentlyPressedDown = false;
        if(numOfPeopleWhenButtonPressedDown != PeopleManager.current.maxPeople){
            GameEventManager.current.GetEvent(EventType.E.FinishFillingPeopleButton).Invoke();
        }
        

    }
}
