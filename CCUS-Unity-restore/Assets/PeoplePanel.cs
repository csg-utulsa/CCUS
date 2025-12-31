using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PeoplePanel : MonoBehaviour
{
    public static PeoplePanel _peoplePanel;
    public GameObject peopleCounter;
    public GameObject newPersonUIFeedback;

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

    public void EnablePeoplePanel(){
        peopleCounter.SetActive(true);
        PressPeopleButton.PPB.EnablePeopleButton();
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
