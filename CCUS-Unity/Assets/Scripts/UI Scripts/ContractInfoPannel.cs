/*
 * Script to handle activating and deactivating the info pannel for contracts in the UI 
 * 
 * Info referenced from: https://discussions.unity.com/t/onmouseover-ui-button-c/166886/2
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ContractInfoPannel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject infoPannel;
    

    void Start()
    {
        infoPannel.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        infoPannel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoPannel.SetActive(false);
    }
}
