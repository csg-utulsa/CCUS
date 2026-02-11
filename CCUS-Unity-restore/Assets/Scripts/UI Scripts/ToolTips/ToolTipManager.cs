/*
*   To add a new tool tip type:
*       1) Create a the new tool tip object and make it a Child of the ToolTipContainer, under the ToolTipManager object
*       2) Create a new script that inherits from ToolTipType and add it to Assets/Scripts/UIScripts/ToolTips/TypesOfToolTips
*       3) In that script, create override functions for ShouldEnableToolTip() and EnableToolTip(), which dictate for which
*          types of tiles that tool tip should appear and what that tool tip should say when enabled.
*               a) Pro-tip: copy them from the MoneyToolTip script and edit as needed
*
*
*   To rearrange order the different tool tip elements appear in, IN THE INSPECTOR, rearrange their order in the allToopTips
*   array of this script.
*
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ToolTipManager : MonoBehaviour
{
    public static ToolTipManager TTM;
    public ToolTipFormatting toolTipFormatter;
    public GameObject currentlySelectedButton;

    public GameObject toolTipContainer;

    public ToolTipType[] allToolTips;

    public float toolTipDistanceFromButtons = 30f;
    public float toolTipYOffset = 30f;


    void Start(){

        

        if(GetComponent<ToolTipFormatting>() != null){
          toolTipFormatter = GetComponent<ToolTipFormatting>();  
        }
        
        if(TTM == null){
            TTM = this;
        }else{
            Destroy(this);
        }
    }

    void Update(){
        Debug.Log("Current Canvas Scale Factor: " + CanvasScalarFactor.CSF.GetScaleFactor());
    }

    public void activateToolTip(Tile tile, GameObject button){

        foreach(ToolTipType toolTip in allToolTips){
            if(toolTip.ShouldEnableToolTip(tile)){
                toolTip.EnableToolTip(tile);
            }
        }

        currentlySelectedButton = button;
        
        RectTransform[] toolTipsToTurnOn = GetToolTipsForTile(tile);
        

        UpdateToolTipPosition();
        toolTipFormatter.FormatToolTip(toolTipsToTurnOn);
        toolTipFormatter.ShowToolTip();

    }

    public void deactivateToolTip(){

        foreach(ToolTipType toolTip in allToolTips){
            toolTip.DisableToolTip();
        }
        
        toolTipFormatter.HideToolTip();
    }

    public void UpdateToolTipPosition(){
        float scaledToolTipDistanceFromButtons = toolTipDistanceFromButtons * CanvasScalarFactor.CSF.GetScaleFactor();
        float scaledYOffset = toolTipYOffset * CanvasScalarFactor.CSF.GetScaleFactor();
        if(currentlySelectedButton == null) return;
        Vector3 toolTipPosition = new Vector3(currentlySelectedButton.transform.position.x - scaledToolTipDistanceFromButtons, currentlySelectedButton.transform.position.y + scaledYOffset, currentlySelectedButton.transform.position.z);
        toolTipContainer.transform.position = toolTipPosition;
        //toolTipFormatter.SetToolTipLocation(toolTipPosition);
    }

    private RectTransform[] GetToolTipsForTile(Tile tile){
        List<RectTransform> toolTipsForTile = new List<RectTransform>();
        foreach(ToolTipType toolTip in allToolTips){
            if(toolTip.ShouldEnableToolTip(tile) && toolTip.GetComponent<RectTransform>() != null){
                toolTipsForTile.Add(toolTip.GetComponent<RectTransform>());
            }
        }
        return toolTipsForTile.ToArray();
    }


}