using UnityEngine;

public class ToolTipFormatting : MonoBehaviour
{
    public float spaceBetweenToolTipItems = 20f;

    public float labelDistanceFromTop = -18f;

    public float firstToolTipDistanceFromLabel = 23f;



    public RectTransform toolTipContainer;

    //Top element of tooltip looks like this: ⌓
    public RectTransform TopOfToolTipBackground;
    //Middle element of tooltip looks like this: □
    public RectTransform MiddleOfToolTipBackground;
    //Bottom element of tooltip looks like this: ᗜ
    public RectTransform BottomOfToolTipBackground;
    //Arrow element of tooltip looks like this: 🞂
    public RectTransform ArrowOfToolTipBackground;
    //All the elements of the tooltip fit together to make the tooltip, that looks about like this: ▢▹

    public void Start(){
        //spaceBetweenToolTipItems *= CanvasScalarFactor.CSF.GetScaleFactor();
    }

    public void FormatToolTip(RectTransform[] toolTipItems){
        
        float adjustedSpaceBetweenItems = spaceBetweenToolTipItems * CanvasScalarFactor.CSF.GetScaleFactor();
        //Gets combined height of all tool tip items
        float heightOfAllToolTips = 0f;
        foreach(RectTransform toolTipItem in toolTipItems){
            heightOfAllToolTips += toolTipItem.rect.height;
        }
        float totalHeightWithSpacing = heightOfAllToolTips + ((toolTipItems.Length - 1) * adjustedSpaceBetweenItems);

        //Sets size of tool tip background
        SetToolTipSize(totalHeightWithSpacing);

        //Properly spaces all of the tool tip items
        SpaceToolTipItems(toolTipItems);

    }

    public void SpaceToolTipItems(RectTransform[] toolTipItems){
        float xPosition = MiddleOfToolTipBackground.anchoredPosition.x;

        //Sets distance from top for the label
        float adjustedlabelDistanceFromTop = labelDistanceFromTop * CanvasScalarFactor.CSF.GetScaleFactor();

        //Starts placing first tool tip item at the bottom of the TopOfToolTipBackground element
        float currentYPosition = RectTransformFunctions.current.GetBottom(TopOfToolTipBackground) - adjustedlabelDistanceFromTop;
        for(int i = 0; i < toolTipItems.Length; i++){
            //Sets the top of each tool tip item
            RectTransformFunctions.current.SetTop(toolTipItems[i], currentYPosition);
            //Sets X position of each tool tip
            toolTipItems[i].anchoredPosition = new Vector2(xPosition, toolTipItems[i].anchoredPosition.y);

            if(i == 0){
                //On the first tool tip, moves it down the distance from the tool tip label
                float adjustedSpaceAfterLabel = firstToolTipDistanceFromLabel * CanvasScalarFactor.CSF.GetScaleFactor();
                currentYPosition -= toolTipItems[i].rect.height + adjustedSpaceAfterLabel;

            } else{
                //Moves the Y position down by the height o fthe current tool tip plus the space between the tool tips
                float adjustedSpaceBetweenItems = spaceBetweenToolTipItems * CanvasScalarFactor.CSF.GetScaleFactor();
                currentYPosition -= toolTipItems[i].rect.height + adjustedSpaceBetweenItems;
            }
            
        }
    }

    public void SetToolTipSize(float totalHeight){


        //Sets height of tool tip background
        MiddleOfToolTipBackground.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, totalHeight);//= new Vector2(MiddleOfToolTipBackground.sizeDelta.x, totalHeight);

        //Positions the middle of the tool tip
        RectTransformFunctions.current.SetTop(MiddleOfToolTipBackground, 0f);
        RectTransformFunctions.current.SetRight(MiddleOfToolTipBackground, 0f);

        //Positions top element of tooltip background
        RectTransformFunctions.current.SetBottom(TopOfToolTipBackground, 0f);
        RectTransformFunctions.current.SetRight(TopOfToolTipBackground, 0f);

        //Positions bottom element of tooltip background
        float bottomOfMiddleSection = RectTransformFunctions.current.GetBottom(MiddleOfToolTipBackground);
        RectTransformFunctions.current.SetTop(BottomOfToolTipBackground, bottomOfMiddleSection);
        RectTransformFunctions.current.SetRight(BottomOfToolTipBackground, 0f);

        //Positions arrow element of tooltip background
        float topOfMiddleSection = RectTransformFunctions.current.GetTop(MiddleOfToolTipBackground);
        RectTransformFunctions.current.SetTop(ArrowOfToolTipBackground, topOfMiddleSection);
        RectTransformFunctions.current.SetLeft(ArrowOfToolTipBackground, 0f);


    }

    public void SetToolTipLocation(Vector3 newLocation){
        if(toolTipContainer != null){
            toolTipContainer.transform.position = newLocation;
        }
    }

    public void HideToolTip(){
        if(toolTipContainer.gameObject == null) return;

        toolTipContainer.gameObject.SetActive(false);
    }

    public void ShowToolTip(){
        if(toolTipContainer.gameObject == null) return;

        toolTipContainer.gameObject.SetActive(true);
    }
}
