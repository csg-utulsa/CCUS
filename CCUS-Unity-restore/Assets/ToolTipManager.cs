using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ToolTipManager : MonoBehaviour
{
    public static ToolTipManager TTM;

    public float distanceBetweenToolTipItems = 0f;

    public float additionalOffsetSingleItem = 0f;

    public Vector3 topItemOffset = new Vector3(0f, 0f, 0f);

    public Vector3 toolTipBackgroundBoxOffset = new Vector3(0f, 0f, 0f);

    public GameObject carbonToolTip;
    
    public GameObject moneyToolTip;

    public GameObject depollutionToolTip;

    public GameObject toolTipBackgroundHeightOne;

    public GameObject toolTipBackgroundHeightTwo;

    private int numOfToolTips = 1;

    private GameObject currentButton;

    private int moneyAmount = 0;

    private int carbonAmount = 0;

    private GameObject toolTipBackground;
    private GameObject[] toolTips;
    private int[] resourceAmounts;

    void Start(){
        if(TTM == null){
            TTM = this;
        }
    }

    public void activateToolTip(int _moneyAmount, int _carbonAmount, GameObject button){

        moneyAmount = _moneyAmount;
        carbonAmount = _carbonAmount;

        currentButton = button;
        Vector3 pos = button.transform.position;

        

        //Sets which tool tips should be activated based on what the tile does
        if(carbonAmount == 0){ //For if there's no carbon tooltip
            toolTipBackground = toolTipBackgroundHeightOne;
            toolTips = new GameObject[1];
            toolTips[0] = moneyToolTip;
            numOfToolTips = 1;

            resourceAmounts = new int[1];
            resourceAmounts[0] = moneyAmount;
        }
        else if(moneyAmount == 0){ // for if there's no money tool tip
            toolTipBackground = toolTipBackgroundHeightOne;
            toolTips = new GameObject[1];
            toolTips[0] = (carbonAmount < 0)? depollutionToolTip : carbonToolTip;
            numOfToolTips = 1;

            resourceAmounts = new int[1];
            resourceAmounts[0] = carbonAmount;
        }
        else{
            toolTipBackground = toolTipBackgroundHeightTwo;
            toolTips = new GameObject[2];
            toolTips[0] = moneyToolTip;
            toolTips[1] = (carbonAmount < 0)? depollutionToolTip : carbonToolTip;
            numOfToolTips = 2;

            resourceAmounts = new int[2];
            resourceAmounts[0] = moneyAmount;
            resourceAmounts[1] = Mathf.Abs(carbonAmount);
        }

        UpdateToolTipPosition();

    }

    public void deactivateToolTip(){
        currentButton = null;
        carbonToolTip.SetActive(false);
        moneyToolTip.SetActive(false);
        depollutionToolTip.SetActive(false);
        toolTipBackgroundHeightOne.SetActive(false);
        toolTipBackgroundHeightTwo.SetActive(false);
    }

    public void UpdateToolTipPosition(){
        if(currentButton == null) return;

        Vector3 pos = currentButton.transform.position;

        toolTipBackground.transform.position = pos + toolTipBackgroundBoxOffset;
        
        for(int i = 0; i < toolTips.Length; i++){
            if(numOfToolTips == 1){
                toolTips[i].transform.position = (pos + toolTipBackgroundBoxOffset + topItemOffset + new Vector3(0f, additionalOffsetSingleItem, 0f)) - new Vector3(0f, distanceBetweenToolTipItems * i, 0f);
            } else{
                toolTips[i].transform.position = (pos + toolTipBackgroundBoxOffset + topItemOffset) - new Vector3(0f, distanceBetweenToolTipItems * i, 0f);
            }
            if(toolTips[i].GetComponentInChildren<TextMeshProUGUI>() != null){
                toolTips[i].GetComponentInChildren<TextMeshProUGUI>().text = "" + resourceAmounts[i];
            }
            toolTips[i].SetActive(true);
        }
        toolTipBackground.SetActive(true);
        
    }


}