using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ContractUIElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI contractTitleText;
    public ProgressBar progressBar;
    public ContractData contractData;
    public ContractInfoPannel infoPannel;

    public bool isContractActive = false;

    void Start()
    {
        contractData = null;
        SetToInactive();
    }

    // Sets the UI element to active mode
    private void SetToActive()
    {
        // TODO: ungray the element

        progressBar.gameObject.SetActive(true);
        isContractActive = true;
    }

    // Sets the UI Element to inactive mode
    private void SetToInactive()
    {
        // TODO: gray out the element
        progressBar.gameObject.SetActive(false);
        isContractActive = false;
    }

    // Called when mouse hovers over the UI element, if there's an active contract then set and show the contract info panel
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(isContractActive)
        {
            infoPannel.gameObject.SetActive(true);
            infoPannel.SetContractName(contractData.title);
            infoPannel.SetContractDescription(contractData.fullDescription);
            infoPannel.SetContractGoalDescription(contractData.goalDescription);
            infoPannel.SetContractReward("Reward: " + contractData.reward.ToString());
        }
    }

    // Called when mouse leaves the UI element, hides the info panel
    public void OnPointerExit(PointerEventData eventData)
    {
        infoPannel.gameObject.SetActive(false);
    }

    // Adds a contract to the UI element and sets the progress bar accordingly
    public void AddContract(ContractData contract)
    {
        contractData = contract;
        SetToActive();
        progressBar.maxBarValue = contractData.goalAmount;
        progressBar.currentBarValue = 0;
        contractTitleText.text = contractData.title;
    }

    // Removes a contract from the UI element and returns the contract data element removed
    public ContractData RemoveContract()
    {
        contractTitleText.text = " ";
        ContractData cd = contractData;
        contractData = null;
        return cd;
    }

    // Updates the value of the progress bar and returns if the contract is completed, accepts a float indicating how much the value has changed since last update
    public bool UpdateContractProgress(float progressValue)
    {
        progressBar.AddToBarValue(progressValue);
        if(progressBar.currentBarValue >= progressBar.maxBarValue)
        {
            return true;
        }
        return false;
    }
}
