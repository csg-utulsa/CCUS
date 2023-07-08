/*
 * Script to handle editing the information in the shared contract information panel
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ContractInfoPannel : MonoBehaviour
{
    public TextMeshProUGUI contractNameText;
    public TextMeshProUGUI contractDescriptionText;
    public TextMeshProUGUI contractRewardText;
    public TextMeshProUGUI contractGoalDescriptionText;

    void Start()
    {
        this.gameObject.SetActive(false);
    }

    public void SetContractName(string contractName)
    {
        contractNameText.text = contractName;
    }

    public void SetContractDescription(string contractDescription)
    {
        contractDescriptionText.text = contractDescription;
    }

    public void SetContractReward(string contractReward)
    {
        contractRewardText.text = contractReward;
    }

    public void SetContractGoalDescription(string contractGoalDescription)
    {
        contractGoalDescriptionText.text = contractGoalDescription;
    }
}
