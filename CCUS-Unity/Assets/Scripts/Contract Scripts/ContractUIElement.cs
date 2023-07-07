using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ContractUIElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ProgressBar progressBar;
    public ContractData contractData;
    public ContractInfoPannel infoPannel;

    public bool isContractActive = false;

    void Start()
    {

    }

    // Called when mouse hovers over the UI element, if there's an active contract then set and show the contract info panel
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(isContractActive)
        {
            infoPannel.gameObject.SetActive(true);
            infoPannel.SetContractName(contractData.title);
            infoPannel.SetContractDescription(contractData.fullDescription);
            infoPannel.SetContractReward("Reward: " + contractData.reward.ToString());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoPannel.gameObject.SetActive(false);
    }
}
