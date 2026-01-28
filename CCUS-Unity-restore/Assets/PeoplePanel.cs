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
    public TextMeshProUGUI numberUnemployedText;
    public FlowFillAmount flowFillAmountOfPeople;
    private float percentOfPeopleEmployed = 0f;
    private int maxNumberOfPeople = 0;

    private bool peoplePanelEnabled = false;

    public int MaxNumberOfPeople {
        set{
            maxNumberOfPeople = value;
            UpdateFlowFillAmount();
            maxNumberOfPeopleText.text = "" + maxNumberOfPeople;
            UpdateEmployeeCounter();
        }
    
    }
    private int numberOfPeople = 0;
    public int NumberOfPeople {
        set{
            numberOfPeople = value;
            UpdateFlowFillAmount();
            numberOfPeopleText.text = "" + numberOfPeople;
            UpdateEmployeeCounter();
        }
    
    }

    private int numberOfEmployees = 0;
    public int NumberOfEmployees {
        set{
            numberOfEmployees = value;
            UpdateEmployeeCounter();
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
        if(PeopleManager.current.CanAddMorePeople()){
            AddPerson();
            
        }else{
            //Displays the "Can only add people to houses connected by roads" Error
            if(!RoadAndResidenceConnectionManager.current.AllResidencesAreConnected()){
                unableToPlaceTileUI._unableToPlaceTileUI.mustConnectResidences();
            }else{
                //Displays the "Not Enough Homes" Error
                unableToPlaceTileUI._unableToPlaceTileUI.notEnoughHomes();
            }
            
        }
    }

    public void AddPerson(){
        PeopleManager.current.AttemptToAddPerson();
        NewPersonUIPopUp();
        UpdateFlowFillAmount();
        
    }

    public void UpdateFlowFillAmount(){
        //Debug.Log("Updating Flow Fill Amount");
        //avoids divide by 0 error
        if(maxNumberOfPeople == 0){
            flowFillAmountOfPeople.ChangeFillAmount(0f);
            //Debug.Log("Updating Flow Fill Amount to 0");
        } else {
            flowFillAmountOfPeople.ChangeFillAmount((float)numberOfPeople / (float)maxNumberOfPeople);
            //Debug.Log("Updating Flow Fill Amount to " + ((float)numberOfPeople / (float)maxNumberOfPeople));
        }
        
    }

    public void UpdateEmployeeCounter(){
        //Calculates number of unemployed people, but if it's less than 0, it just says 0
        int numberUnemployed = ((numberOfPeople - numberOfEmployees) > (-1))? ((numberOfPeople - numberOfEmployees)) : (0);
        if(numberUnemployedText != null) numberUnemployedText.text = "" + numberUnemployed;
    }

    public void EnablePeoplePanel(){
        //peopleCounter.SetActive(true);
        if(!peoplePanelEnabled){
            PressPeopleButton.PPB.EnablePeopleButton();
            GetComponent<PeoplePanelActivator>().ActivatePeoplePanel();
            peoplePanelEnabled = true;
        }
        
    }

    public void NewPersonUIPopUp(){
        GameObject UIFeedbackObject = Instantiate(newPersonUIFeedback);
        UIFeedbackObject.GetComponentInChildren<TextMeshProUGUI>().text = "$" + PeopleManager.current.incomeOfPerson + " per hour";
    }

    public bool isMouseOverPanel(){
        Vector2 mousePos = Input.mousePosition;
        if(GetComponent<RectTransform>() != null){
            return RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), mousePos);
        }
        return false;
    }
}
