using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PeoplePanel : MonoBehaviour
{
    public static PeoplePanel _peoplePanel;
    public GameObject peopleCounter;
    public GameObject newPersonUIFeedback;
    public TextMeshProUGUI maxNumberOfPeopleText;
    public TextMeshProUGUI numberOfPeopleText;
    public FlowFillAmount flowFillAmountOfPeople;
    private int maxNumberOfPeople = 0;
    public int MaxNumberOfPeople {
        set{
            maxNumberOfPeople = value;
            UpdateFlowFillAmount();
            maxNumberOfPeopleText.text = "" + maxNumberOfPeople;
        }
    
    }
    private int numberOfPeople = 0;
    public int NumberOfPeople {
        set{
            numberOfPeople = value;
            UpdateFlowFillAmount();
            numberOfPeopleText.text = "" + numberOfPeople;
        }
    
    }

    // Start is called before the first frame update
    void Start()
    {
        if(_peoplePanel == null){
            _peoplePanel = this;
        }   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PeopleButtonPressed(){
        if(TemporaryPeopleManager.TPM.CanAddMorePeople()){
            AddPerson();
            
        }else{
            //Displays the "Can only add people to houses connected by roads" Error
            if(!RoadAndResidenceConnectionManager.RARCM.AllResidencesAreConnected()){
                unableToPlaceTileUI._unableToPlaceTileUI.mustConnectResidences();
            }else{
                //Displays the "Not Enough Homes" Error
                unableToPlaceTileUI._unableToPlaceTileUI.notEnoughHomes();
            }
            
        }
    }

    public void AddPerson(){
        TemporaryPeopleManager.TPM.AttemptToAddPerson();
        NewPersonUIPopUp();
        UpdateFlowFillAmount();
        
    }

    public void UpdateFlowFillAmount(){
        //Debug.Log("Updating Flow Fill Amount");
        //avoids divide by 0 error
        if(maxNumberOfPeople == 0){
            flowFillAmountOfPeople.ChangeFillAmount(0f);
        } else {
            flowFillAmountOfPeople.ChangeFillAmount((float)numberOfPeople / (float)maxNumberOfPeople);
        }
        
    }

    public void EnablePeoplePanel(){
        //peopleCounter.SetActive(true);
        PressPeopleButton.PPB.EnablePeopleButton();
        GetComponent<PeoplePanelActivator>().ActivatePeoplePanel();
    }

    public void NewPersonUIPopUp(){
        GameObject UIFeedbackObject = Instantiate(newPersonUIFeedback);
        UIFeedbackObject.GetComponentInChildren<TextMeshProUGUI>().text = "+$" + TemporaryPeopleManager.TPM.incomeOfPerson;
    }

    public bool isMouseOverPanel(){
        Vector2 mousePos = Input.mousePosition;
        if(GetComponent<RectTransform>() != null){
            return RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), mousePos);
        }
        return false;
    }
}
