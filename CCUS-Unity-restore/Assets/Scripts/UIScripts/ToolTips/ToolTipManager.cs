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
    public GameObject toolTipArrow;
    public GameObject currentlySelectedButton;

    public GameObject toolTipContainer;

    public ToolTipType[] allToolTips;

    public float toolTipDistanceFromButtons = 30f;
    public float toolTipYOffset = 30f;

    public float toolTipEdgeBuffer = 30f;


    void Start(){


        if(GetComponentInChildren<ToolTipArrow>(true) != null){
            toolTipArrow = GetComponentInChildren<ToolTipArrow>(true).gameObject;
        }
        

        if(GetComponent<ToolTipFormatting>() != null){
          toolTipFormatter = GetComponent<ToolTipFormatting>();  
        }
        
        if(TTM == null){
            TTM = this;
        }else{
            Destroy(this);
        }

        GameEventManager.current.GetEvent(EventType.E.TileSelectPanelScrolled).AddListener(UpdateToolTipPosition);
    }

    void Update(){
        //Disables tool tip in touch mode if the player isn't touching the screen
        if(TouchModeHandler.current.IsInTouchMode && Input.touchCount == 0){
            deactivateToolTip();
        }
    }

    public void activateToolTip(Tile tile, GameObject button){

        //Deactivates all tool tips
        foreach(ToolTipType toolTip in allToolTips){
            toolTip.DisableToolTip();
        }

        //Activates all tool tips for the button that is being hovered over
        foreach(ToolTipType toolTip in allToolTips){
            if(toolTip.ShouldEnableToolTip(tile)){
                toolTip.EnableToolTip(tile);
            }
        }

        currentlySelectedButton = button;
        
        RectTransform[] toolTipsToTurnOn = GetToolTipsForTile(tile);
        

        toolTipFormatter.FormatToolTip(toolTipsToTurnOn);
        UpdateToolTipPosition();
        toolTipFormatter.ShowToolTip();

        //Turns on tool tip arrow
        toolTipArrow.SetActive(true);

    }

    public void deactivateToolTip(){

        //Deactivates all tool tips
        foreach(ToolTipType toolTip in allToolTips){
            toolTip.DisableToolTip();
        }
        
        toolTipFormatter.HideToolTip();

        //Turns off tool tip arrow
        toolTipArrow.SetActive(false);
    }

    public void UpdateToolTipPosition(){
        float scaledToolTipDistanceFromButtons = toolTipDistanceFromButtons * CanvasScalarFactor.CSF.GetScaleFactor();
        float scaledYOffset = toolTipYOffset * CanvasScalarFactor.CSF.GetScaleFactor();
        if(currentlySelectedButton == null) return;

        float toolTipTotalHeight = toolTipFormatter.GetToolTipTotalHeight();
        float toolTipVerticalPosition = currentlySelectedButton.transform.position.y + scaledYOffset;

        //Moves tool tip arrow to proper location, before the tool tip background's location is clamped
        toolTipArrow.GetComponent<ToolTipArrow>().SetPosition(toolTipVerticalPosition);


        toolTipVerticalPosition = ClampToolTipPosition(toolTipVerticalPosition, toolTipTotalHeight);

        Vector3 toolTipPosition = new Vector3(currentlySelectedButton.transform.position.x - scaledToolTipDistanceFromButtons, toolTipVerticalPosition, currentlySelectedButton.transform.position.z);
        toolTipContainer.transform.position = toolTipPosition;
        
        

        


    }

    //forces tool tip to stay on screen
    private float ClampToolTipPosition(float toolTipPosition, float toolTipHeight){

        //Note: Must multiply all heights (other than Screen.height) by scaleFactor

        float scaleFactor = CanvasScalarFactor.CSF.GetScaleFactor();

        float bottomOfToolTip = (toolTipPosition) - ((toolTipHeight) * scaleFactor);

        float topOfToolTipBackGroundHeight = toolTipFormatter.TopOfToolTipBackground.rect.height;

        //Debug.LogError("Canvas Scale Factor: " + CanvasScalarFactor.CSF.GetScaleFactor());

        //Tool tip is too low
        if(bottomOfToolTip < (toolTipEdgeBuffer * scaleFactor)){
            //Debug.Log("Mode: Low");
            //Debug.Log("tool tip position: " + toolTipPosition + ", bottom of tool tip: " + bottomOfToolTip + ", height: " + toolTipHeight);
            return (toolTipHeight + toolTipEdgeBuffer) * scaleFactor;


        //Tool tip is too high
        //Adds height of top of tool tip background, because the tool tip position is marked right below it
        } else if(((toolTipPosition) + (topOfToolTipBackGroundHeight * scaleFactor)) > (Screen.height - (toolTipEdgeBuffer * scaleFactor))){
            //Debug.Log("Mode: High");
            //Debug.Log("position: " + toolTipPosition + ", top height: " + toolTipFormatter.TopOfToolTipBackground.rect.height + ", screen height: " + Screen.height);
            return (Screen.height - (topOfToolTipBackGroundHeight * scaleFactor)) - (toolTipEdgeBuffer * scaleFactor);
        }
        
        else{ // Tool tip is neither too high nor too low
            //Debug.Log("Mode: Normal");
            return toolTipPosition;
        }
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