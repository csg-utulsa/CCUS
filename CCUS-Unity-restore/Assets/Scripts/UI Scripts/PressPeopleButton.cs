using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class PressPeopleButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    public GameObject buttonPressedGraphic;
    //public GameObject newPersonUIFeedback;
    public GameObject peopleButtonEnabledImage;
    //public GameObject peopleCounter;

    public float UIFeedbackHeight = 3f;
    public TemporaryPeopleManager _TPM;
    private bool enabled = false;
    public static PressPeopleButton PPB;

    void Start(){
        if(TemporaryPeopleManager.TPM != null){
          _TPM = TemporaryPeopleManager.TPM;  
        }
        if(PPB == null){
            PPB = this;
        }
    }


    //Called when button is pressed down
    public void OnPointerDown(PointerEventData eventData) {
        if(!enabled) return;
        buttonPressedGraphic.SetActive(true);

        TemporaryPeopleManager.TPM.AttemptToAddPerson();
        //Replace with function in temporary people manager
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
    }

    public void EnablePeopleButton(){
        enabled = true;
        peopleButtonEnabledImage.SetActive(true);
        //peopleCounter.SetActive(true);
        GetComponent<SizingEmphasis>().WobbleGraphic();
    }
}
