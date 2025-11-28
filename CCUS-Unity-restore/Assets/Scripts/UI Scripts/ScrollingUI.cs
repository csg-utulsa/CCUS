using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScrollableArea : MonoBehaviour
{
    public float visibleScrollOffset = 0f;
    public RectTransform rectangleTransform;
    public float heightOfScrollableArea = 0f;
    public void UpdateVisibleScrollOffset(float inputVisibleScrollOffset){
        visibleScrollOffset = inputVisibleScrollOffset;
        UpdateUIElementsPositions();
    }
    public virtual void UpdateUIElementsPositions(){
        
    }
}

public class ScrollingUI : MonoBehaviour
{

    [HideInInspector]
    public static bool mouseIsOverScrollableArea = false;
    
    public ScrollableArea scrollableArea;

    [Header("Scrolling Settings")]
    public float scrollIncrement = 10f;
    public float peakScrollSpeed = 210f;
    public float scrollSpeed = .005f;
    public float scrollAcceleration = 2f;
    float timeStoppedMovingScrollWheel = 0f;
    float visibleScrollOffset = 0f;
    float actualScrollOffset = 0f;
    bool currentlyScrolling = false;
    float totalHeightOfButtonArea  = 0f;
    static int totalNumberOfScrollingPanels = 0;
    int scrollingPanelIndex = 0;
    
    //float heightOfScrollableArea = 0f;
    // Start is called before the first frame update
    void Start()
    {
        scrollingPanelIndex = totalNumberOfScrollingPanels;
        totalNumberOfScrollingPanels++;
        visiblyScrollButtons();
    }

    // Update is called once per frame
    void Update()
    {
        //The next two if functions deal with scrolling (Move these to a separate script at some point)
        //They check if the user is scrolling
        float scrollFactor = Input.GetAxis("Mouse ScrollWheel"); 
        if(scrollFactor != 0f){
            //Checks if the user's mouse is over the panel
            if (isMouseOverScrollingPanel()){
                currentlyScrolling = true;
                timeStoppedMovingScrollWheel = Time.time;
                actualScrollOffset -= scrollFactor*scrollSpeed;
            }
        }
        if(currentlyScrolling){

            visiblyScrollButtons();
        }

        //The next two if statements check whether or not the mouse is over a scrolling panel
        if((scrollingPanelIndex == 0)){
            mouseIsOverScrollableArea =  false;
        }
        if(isMouseOverScrollingPanel()){
            mouseIsOverScrollableArea =  true;
        }

    }

    public bool isMouseOverScrollingPanel(){
        Vector2 mousePos = Input.mousePosition;
        // bool _MouseIsOverScrollableArea = ;
        // mouseIsOverScrollableArea = _MouseIsOverScrollableArea;
        return RectTransformUtility.RectangleContainsScreenPoint(scrollableArea.rectangleTransform, mousePos);//_MouseIsOverScrollableArea;
    }

    //This function does all the math for scrolling
    void visiblyScrollButtons(){
        float panelHeight = scrollableArea.rectangleTransform.rect.height;

        //The next if-else structure forces the button scrolling to remain within the bounds       
        if(visibleScrollOffset > scrollableArea.heightOfScrollableArea){
            visibleScrollOffset = scrollableArea.heightOfScrollableArea;
            scrollableArea.UpdateVisibleScrollOffset(visibleScrollOffset);
            actualScrollOffset = scrollableArea.heightOfScrollableArea;
            scrollableArea.UpdateUIElementsPositions();
           // Debug.Log("Updating UI Elements Positions");
            currentlyScrolling = false;
            return;
        } else if(visibleScrollOffset < 0f){
            visibleScrollOffset = 0f;
            //Debug.Log("Updating UI Elements Positions");
            scrollableArea.UpdateVisibleScrollOffset(visibleScrollOffset);
            actualScrollOffset = 0f;
            scrollableArea.UpdateUIElementsPositions();
            currentlyScrolling = false;
            return;
        }
        

        //Prevents the user from scrolling too fast (makes touchpad less sensative)
        if(Mathf.Abs(visibleScrollOffset - actualScrollOffset) > peakScrollSpeed){
            if(visibleScrollOffset > actualScrollOffset){
                actualScrollOffset = visibleScrollOffset - peakScrollSpeed;
            } else {
                actualScrollOffset = visibleScrollOffset + peakScrollSpeed;
            }
        }

        //The next two if statements move the buttons towards their final scroll destination
        //float currentScrollIncrement = scrollIncrement * Mathf.Abs(visibleScrollOffset - actualScrollOffset);
        float currentScrollIncrement = (float)scrollIncrement;// * ((scrollAcceleration*.1) * Mathf.Abs(visibleScrollOffset - actualScrollOffset)));
        if(visibleScrollOffset > actualScrollOffset){
            //Moves the buttons towards the final location after the user scrolls them
            visibleScrollOffset -= currentScrollIncrement * Time.deltaTime;

            //Checks to see if the scroll movement has gotten close to its destination
            if((visibleScrollOffset <= actualScrollOffset) || Mathf.Abs(visibleScrollOffset - actualScrollOffset) < .01f){
                visibleScrollOffset = actualScrollOffset;
                //scrollableArea.UpdateVisibleScrollOffset(visibleScrollOffset);
                currentlyScrolling = false;
            }
            scrollableArea.UpdateVisibleScrollOffset(visibleScrollOffset);
        } else {
            //Moves the buttons towards the final location after the user scrolls them
            visibleScrollOffset += currentScrollIncrement * Time.deltaTime;

            //Checks to see if the scroll movement has gotten close to its destination
            if((visibleScrollOffset >= actualScrollOffset) || Mathf.Abs(visibleScrollOffset - actualScrollOffset) < .01f){
                visibleScrollOffset = actualScrollOffset;
                
                currentlyScrolling = false;
            }
            scrollableArea.UpdateVisibleScrollOffset(visibleScrollOffset);
        }

        //Moves the UI Elements into their new positions
        scrollableArea.UpdateUIElementsPositions();

        //Tells Tool Tips to Update their positions
        ToolTipManager.TTM.UpdateToolTipPosition();

    }
}
