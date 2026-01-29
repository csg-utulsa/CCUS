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
    public static PressPeopleButton PPB;
    public float minTimeBetweenButtonClicks = .05f;
    public float timeToInitiateClickAndHold = .25f;

    private bool buttonIsCurrentlyPressedDown = false;
    private float buttonClickTimer = 0f;
    private bool buttonHoldAndClickInitiated = false;

    void Awake(){
        if(PeopleManager.current != null){
          peopleManager = PeopleManager.current;  
        }
        if(PPB == null){
            PPB = this;
        } else{
            Destroy(this);
        }
        
    }

    //When the button is being held down, it clicks down repeatedly, waiting minTimeBetweenButtonClicks between each click.
    void Update(){
        if(buttonIsCurrentlyPressedDown){

            buttonClickTimer += Time.deltaTime;
            if(buttonClickTimer > timeToInitiateClickAndHold){
                buttonHoldAndClickInitiated = true;
            }
            if(buttonHoldAndClickInitiated && buttonClickTimer > minTimeBetweenButtonClicks){
                buttonClickTimer = 0f;
                PeoplePanel._peoplePanel.PeopleButtonPressed();
            }
        }
    }


    //Called when button is pressed down
    public void OnPointerDown(PointerEventData eventData) {
        if(!enabled) return;
        buttonIsCurrentlyPressedDown = true;
        buttonClickTimer = 0f;
        buttonPressedGraphic.SetActive(true);
        buttonUnpressedGraphic.SetActive(false);

        PeoplePanel._peoplePanel.PeopleButtonPressed();
        
        //Replaced with function in temporary people manager
        // if(_TPM.CanAddMorePeople()){
        //     _TPM.AddAPerson();
        //     GameObject UIFeedbackObject = Instantiate(newPersonUIFeedback, this.transform);//new Vector3(transform.position.x, transform.position.y + UIFeedbackHeight, transform.position.z), newPersonUIFeedback.transform.rotation);
        //     //UIFeedbackObject.transform.position = new Vector3(transform.position.x, transform.position.y + UIFeedbackHeight, transform.position.z);
        //     UIFeedbackObject.GetComponentInChildren<TextMeshProUGUI>().text = "+$" + _TPM.incomeOfPerson;
        // }else{
        //     unableToPlaceTileUI._unableToPlaceTileUI.notEnoughHomes();
        // }
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
    }

    public void EnablePeopleButton(){
        enabled = true;
        peopleButtonEnabledImage.SetActive(true);
        //peopleCounter.SetActive(true);
        //GetComponent<SizingEmphasis>().WobbleGraphic();
    }
}
