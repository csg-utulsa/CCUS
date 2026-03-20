// Description: Handles scrolling for computer inputs (Mouse Wheel & Mouse Pad)

using UnityEngine;

public class TileSelectPanelScroll : MonoBehaviour
{

    public TileSelectPanel TSP;

    //Panel Rectangle Transform
    RectTransform rectangleTransform;

    [Header("How fast to scroll:")]
    public float scrollSpeed = 1000f;

    [Header("Smallest time in seconds it will scroll:")]
    public float minScrollTime = .1f;

    [Header("Flip Scroll Direction")]
    public bool flipScrollDirection = false;

    private float currentScrollOffset;
    
    private float scrollTimer = 0f;

    private int scrollDirection = 0;

    private float MaxScroll{
        get{
            return TSP.heightOfScrollableArea - (2f * TSP.gapBetweenTiles);
        }
    }

    void Start()
    {
        rectangleTransform = GetComponent<RectTransform>();
        TSP = GetComponent<TileSelectPanel>();
    }

    void Update()
    {

        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        //Starts scrolling when player gives a scroll input
        if(scrollInput != 0f && isMouseOverPanel()){

            //Stores the direction of the scroll wheel
            scrollDirection = (scrollInput < 0) ? 1 : -1;

            scrollTimer = minScrollTime;

        }

        //If scroll timer isn't out of time, it scrolls the panel
        if(scrollTimer > 0f){

            //Scroll timer counts down each update
            scrollTimer -= Time.deltaTime;

            //If bool flipScrollDirection is true, flips direction of scrolling
            int flipScrollFactor = (flipScrollDirection) ? -1 : 1;

            //Moves Tile Select Panel when scroll timer is greater than zero
            currentScrollOffset = currentScrollOffset + (Time.deltaTime * scrollDirection * scrollSpeed * flipScrollFactor);

            //Clamps Scroll Offset
            currentScrollOffset = ClampScrollOffset(currentScrollOffset);

            TSP.SetScrollOffset(currentScrollOffset);

        } else{

            //Stops scrolling when scroll timer runs out
            scrollTimer = 0f;
        }

    }

    //If mouse is over this panel, returns true
    public bool isMouseOverPanel(){
        if(rectangleTransform == null){
            return false;
        } else{
            Vector2 mousePos = Input.mousePosition;
            return RectTransformUtility.RectangleContainsScreenPoint(rectangleTransform, mousePos);
        }
    }

    //Prevents the player from scrolling infinitely
    public float ClampScrollOffset(float inputOffset){
        if(inputOffset < 0f){
            return 0f;
        } else if(inputOffset > MaxScroll){
            return MaxScroll;
        } else{
            return inputOffset;
        }
    }

    public void ScrollToTop(){
        TSP.SetScrollOffset(0f);
    }

    public void ScrollToBottomButton(){

        //Sets scroll offset to go as far as it can, then go back up by the height of the panel
        currentScrollOffset = MaxScroll - rectangleTransform.rect.height + (4f * TSP.gapBetweenTiles);

        //Clamps Scroll Offset
        currentScrollOffset = ClampScrollOffset(currentScrollOffset);

        TSP.SetScrollOffset(currentScrollOffset);
    }

}
