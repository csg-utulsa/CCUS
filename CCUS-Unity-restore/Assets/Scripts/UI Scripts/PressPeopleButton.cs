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


    //Called when button is pressed down
    public void OnPointerDown(PointerEventData eventData) {
        if(!enabled) return;
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
