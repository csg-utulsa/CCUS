// Description: Controls scrolling the Tile Select Panel for Touch Input

using UnityEngine;

public class ScrollTouchInput : MonoBehaviour
{

    //Panel Rectangle Transform
    RectTransform rectangleTransform;

    TileSelectPanelScroll tileSelectPanelScroll;

    TileSelectPanel tileSelectPanel;

    //Position where finger intially touches down
    Vector2 initialTouchPosition;

    float initialScrollOffset;

    private bool isTouch = false;

    void Start()
    {
        rectangleTransform = GetComponent<RectTransform>();

        tileSelectPanel = GetComponent<TileSelectPanel>();

        tileSelectPanelScroll = GetComponent<TileSelectPanelScroll>();
    }

    void Update()
    {   
        //Checks if the screen is getting touch input
        if(Input.touchCount > 0 && isTouchOverPanel(Input.GetTouch(0))){

            //When the finger initially touched down
            if(!isTouch){
                isTouch = true;
                Touch inputTouch = Input.GetTouch(0);
                initialTouchPosition = inputTouch.position;

                if(tileSelectPanel != null){
                    initialScrollOffset = tileSelectPanel.visibleScrollOffset;
                }

            }

            //When the finger is being held down
            else{
                float touchMoveDistance = Input.GetTouch(0).position.y - initialTouchPosition.y;

                float adjustedTouchMoveDistance = touchMoveDistance * CanvasScalarFactor.CSF.GetScaleFactor();

                float newScrollOffset = adjustedTouchMoveDistance + initialScrollOffset;

                tileSelectPanelScroll.SetScrollOffsetWithClamp(newScrollOffset);
            }


        } else{

            //When the finger is lifted off
            if(isTouch){
                isTouch = false;
            }
        }

    }

    //If touch input is over this panel, returns true
    public bool isTouchOverPanel(Touch inputTouch){
        if(rectangleTransform == null){
            return false;
        } else{
            Vector2 touchPos = inputTouch.position;
            return RectTransformUtility.RectangleContainsScreenPoint(rectangleTransform, touchPos);
            Debug.Log("Touch is Over Panel");
        }
    }


}
