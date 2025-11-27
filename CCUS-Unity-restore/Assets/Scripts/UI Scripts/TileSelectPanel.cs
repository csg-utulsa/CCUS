using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TileSelectPanel : ScrollableArea
{
    public static TileSelectPanel TSP;


    //public RectTransform rectangleTransform;
    public GameObject selectedButtonGraphic;

    [Header("Button Array (Populates on Start())")]
    public GameObject[] tileButtons;
    GameObject[] tiles;
    GameObject selectedButton;

    [Header("Button Layout")]
    public float heightOfTile;
    public float gapBetweenTiles = 20f;
    public float distanceFromTopOfPanel = 30f;
    




    [Header("Scrolling Settings")]
    public float scrollIncrement = 10f;
    public float peakScrollSpeed = 210f;
    public float scrollSpeed = .005f;
    float timeStoppedMovingScrollWheel = 0f;
    //float visibleScrollOffset = 0f;
    float actualScrollOffset = 0f;
    bool currentlyScrolling = false;
    float totalHeightOfButtonArea  = 0f;
    //float heightOfScrollableArea = 0f;
    //float lastTimeOfScroll = 0f;

    TileScriptableObject[] allTileScriptableObjects;//= new TileScriptableObject[];

    bool buttonIsSelected = false;

    bool[] disabledPollutionButtons;
    bool[] disabledMoneyButtons;

    //public GameObject testPrefabToAdd;
    
    void Awake(){
        TSP = this;
    }

    void Start()
    {
        rectangleTransform = GetComponent<RectTransform>();
        //Gets all of the buttons that are attached to the tile panel
        if(GetComponentsInChildren<Button>(true) != null){
            heightOfTile = GetComponentInChildren<Button>(true).transform.gameObject.GetComponent<RectTransform>().rect.height;
            Button[] buttonRectComponents = GetComponentsInChildren<Button>(true);
            tileButtons = new GameObject[buttonRectComponents.Length];
            for(int i = 0; i < buttonRectComponents.Length; i++){
                tileButtons[i] = buttonRectComponents[i].transform.gameObject;
            }
            allTileScriptableObjects =  new TileScriptableObject[tileButtons.Length];
            disabledPollutionButtons = new bool[tileButtons.Length];
            disabledMoneyButtons = new bool[tileButtons.Length];
            for(int i = 0; i < tileButtons.Length; i++){
                allTileScriptableObjects[i] = tileButtons[i].GetComponent<buttonScript>().tileToPlace.GetComponent<Tile>().tileScriptableObject;
                disabledPollutionButtons[i] = false;
                
                disabledMoneyButtons[i] = false;
            }

        }else{
            Debug.LogError("There ain't no buttons attached to the tile select panel! We kinda need those, dude.");
        }
        //Moves all the buttons to their proper position;
        updateButtonPositions();
    }


    //Input the prefab for the button you want to turn on.
    public void AddButton(GameObject prefabOfButton){
        for(int i = 0; i < allTileScriptableObjects.Length; i++){
            if(prefabOfButton.GetComponent<Tile>().tileScriptableObject == allTileScriptableObjects[i]){
                tileButtons[i].SetActive(true);
                updateButtonPositions();
            }else{
                Debug.LogError("None of the tile Buttons are for the prefab you're trying to turn on the button for.");
            }
        }
    }

    //Input the button script for the button you want to turn on
    public void AddButton(buttonScript _buttonScript){
        for(int i = 0; i < allTileScriptableObjects.Length; i++){
            buttonScript testbutton = _buttonScript;
            GameObject testTile = _buttonScript.gameObject;
            Tile testTile2 = _buttonScript.tileToPlace.GetComponent<Tile>();
            if(_buttonScript.tileToPlace.GetComponent<Tile>().tileScriptableObject == allTileScriptableObjects[i]){
                tileButtons[i].SetActive(true);
                updateButtonPositions();
            }
        }
    }

    public void RemoveButton(GameObject prefabOfButton){
        for(int i = 0; i < allTileScriptableObjects.Length; i++){
            if(prefabOfButton.GetComponent<Tile>().tileScriptableObject == allTileScriptableObjects[i]){
                tileButtons[i].SetActive(false);
                updateButtonPositions();
            }else{
                Debug.LogError("None of the tile Buttons are for the prefab you're trying to turn off the button for.");
            }
        }
    }

    //When called, this function disables the tile buttons above a certain price, and enables the tiles below that price.
    public void checkPricesOfTiles(int currentAmountOfMoney){
        for(int i = 0; i < tileButtons.Length; i++){
            if(allTileScriptableObjects[i].BuildCost > LevelManager.LM.GetMoney()){
                disabledMoneyButtons[i] = true;
            } else{
                disabledMoneyButtons[i] = false;
            }
        }
        visuallyUpdateDisabledButtons();
        updateActiveTileGraphics();
    }

    //disables all buttons for tiles that emit pollution
    public void disablePolluters(){
        for(int i = 0; i < tileButtons.Length; i++){
            if(allTileScriptableObjects[i].AnnualCarbonAdded > 0f){
                disabledPollutionButtons[i] = true;
            }
        }
        visuallyUpdateDisabledButtons();
        updateActiveTileGraphics();
    }

    //enables all buttons for tiles that emit pollution
    public void enablePolluters(){
        for(int i = 0; i < tileButtons.Length; i++){
            if(allTileScriptableObjects[i].AnnualCarbonAdded > 0f){
                disabledPollutionButtons[i] = false;
            }
        }
        visuallyUpdateDisabledButtons();
        updateActiveTileGraphics();
    }
    
    void updateActiveTileGraphics(){
        GameObject _aciveObject = BuildingSystem.current.activeObject;
        if(_aciveObject != null){
            _aciveObject.GetComponent<ObjectDrag>().updateTileMaterialValidity();
        }
    }

    //turns disabled buttons red
    void visuallyUpdateDisabledButtons(){
        for(int i = 0; i < tileButtons.Length; i++){
            if(disabledPollutionButtons[i] || disabledMoneyButtons[i]){
                tileButtons[i].GetComponent<Image>().color = Color.red;
            } else{
                tileButtons[i].GetComponent<Image>().color = Color.white;
            }
        }
    }


    //The buttons call this function when they're clicked. It then selects one of the buttons.
    public void clickButton(GameObject clickedButton){
        TrashButtonScript.TBS.trashButtonDeselected();
        //runs if the user clicks a button that isn't currently selected
        if(!buttonIsSelected || clickedButton != selectedButton){
            //Debug.Log("Ran that button isn't selected");

            //The next three if statements check if the button the user clicked is disabled. If it is, it prevents the user from selecting it.
            bool doNotSelectButton = false;
            //Debug.Log("Offensive Index: " + Array.IndexOf(tileButtons, clickedButton));
            //Debug.Log("Offended Array: " + disabledPollutionButtons);
            if(disabledPollutionButtons[Array.IndexOf(tileButtons, clickedButton)]){
                unableToPlaceTileUI._unableToPlaceTileUI.tooMuchCarbon();
                doNotSelectButton = true;
            }
            if(disabledMoneyButtons[Array.IndexOf(tileButtons, clickedButton)]){
                unableToPlaceTileUI._unableToPlaceTileUI.notEnoughMoney();
                doNotSelectButton = true;
            }
            if(doNotSelectButton) { return;}


            buttonIsSelected = true;
            selectedButton = clickedButton;
            BuildingSystem.current.InitializeWithObject(selectedButton.GetComponent<buttonScript>().tileToPlace);
            selectedButtonGraphic.SetActive(true);
            selectedButtonGraphic.GetComponent<RectTransform>().anchoredPosition = new Vector2(selectedButton.GetComponent<RectTransform>().anchoredPosition.x, selectedButton.GetComponent<RectTransform>().anchoredPosition.y);
        } 
        //runs if the user clicks the button that is already selected, and so deselects it.
        else {
            //Debug.Log("Ran Deselect Buttons");
            deselectAllButtons();
        }
            
    }

    public void deselectAllButtons(){
        selectedButtonGraphic.SetActive(false);
        selectedButton = new GameObject();
        BuildingSystem.current.deselectCurrentObject();
        buttonIsSelected = false;
    }


    //Moved to Another Script in an attempt at modularity
    // void Update()
    // {
    //     //The next two if functions deal with scrolling (Move these to a separate script at some point)
    //     //They check if the user is scrolling
    //     float scrollFactor = Input.GetAxis("Mouse ScrollWheel"); 
    //     if(scrollFactor != 0f){
    //         //Checks if the user's mouse is over the panel
    //         if (isMouseOverScrollingPanel()){
    //             currentlyScrolling = true;
    //             timeStoppedMovingScrollWheel = Time.time;
    //             actualScrollOffset -= scrollFactor*scrollSpeed;
    //         }
    //     }
    //     if(currentlyScrolling){
    //         visiblyScrollButtons();
    //     }
    // }

    public override void UpdateUIElementsPositions(){
       //Debug.Log("instance called");
        updateButtonPositions();
        
    }

    public bool isMouseOverScrollingPanel(){
        Vector2 mousePos = Input.mousePosition;
        return RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), mousePos);
    }


    //This function moves all of the buttons to their proper positions in the RectTransform when called
    public void updateButtonPositions(){
        //Debug.Log("Updating Button Positions");
        float panelHeight = GetComponent<RectTransform>().rect.height;
        float buttonPlacementPosition = /* center of panel */GetComponent<RectTransform>().anchoredPosition.y + /*half of height of panel*/ ((.5f)*panelHeight) - /* distance from top of panel */ distanceFromTopOfPanel;
        float topOfScrollingArea = buttonPlacementPosition;
        float bottomOfScrollingArea = 0f;
        //It's at the top of the tile select panel

        buttonPlacementPosition = buttonPlacementPosition - 30f - (.5f*heightOfTile);
        //Now it's at the location of the first button

        //Makes an array of only the active tiles
        int numOfActiveTiles = 0;
        foreach(GameObject tileButton in tileButtons){
            if(tileButton.activeSelf){
                numOfActiveTiles++;
            }
        }
        GameObject[] activeTileButtons = new GameObject[numOfActiveTiles];
        int currentActiveTile = 0;  
        for(int i = 0; i < tileButtons.Length; i++){
            if(tileButtons[i].activeSelf){
                activeTileButtons[currentActiveTile] = tileButtons[i].transform.gameObject;
                currentActiveTile++;
            }
            
        }

        //The following for loop moves down the placement location for each button
        for(int i = 0; i < activeTileButtons.Length; i++){
            //Moves position of the button
            activeTileButtons[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(activeTileButtons[i].GetComponent<RectTransform>().anchoredPosition.x, buttonPlacementPosition + visibleScrollOffset);
            buttonPlacementPosition -= heightOfTile + gapBetweenTiles;
            if(i == activeTileButtons.Length-1){
                bottomOfScrollingArea = buttonPlacementPosition - (0.5f * heightOfTile);
            }
        }
        if(buttonIsSelected){
            selectedButtonGraphic.GetComponent<RectTransform>().anchoredPosition = new Vector2(selectedButtonGraphic.GetComponent<RectTransform>().anchoredPosition.x, selectedButton.GetComponent<RectTransform>().anchoredPosition.y);
        }
        

        //Sets height of how far the user can scroll
        totalHeightOfButtonArea  = Mathf.Abs(topOfScrollingArea - bottomOfScrollingArea);
        heightOfScrollableArea = (totalHeightOfButtonArea > panelHeight) ? (totalHeightOfButtonArea - panelHeight) : 0f;
    }



    
    
    //Moved to separate script in a vague attempt at modularity
    //This function does all the math for scrolling (Move it to a separate script at some point)
    // void visiblyScrollButtons(){
    //     float panelHeight = GetComponent<RectTransform>().rect.height;
 
    //     //The next if-else structure forces the button scrolling to remain within the bounds
    //     if(actualScrollOffset > heightOfScrollableArea){
    //         visibleScrollOffset = heightOfScrollableArea;
    //         actualScrollOffset = heightOfScrollableArea;
    //         updateButtonPositions();
    //         currentlyScrolling = false;
    //         return;
    //     } else if(actualScrollOffset < 0f){
    //         visibleScrollOffset = 0f;
    //         actualScrollOffset = 0f;
    //         updateButtonPositions();
    //         currentlyScrolling = false;
    //         return;
    //     }

    //     //Prevents the user from scrolling too fast (makes touchpad less sensative)
    //     if(Mathf.Abs(visibleScrollOffset - actualScrollOffset) > peakScrollSpeed){
    //         if(visibleScrollOffset > actualScrollOffset){
    //             actualScrollOffset = visibleScrollOffset - peakScrollSpeed;
    //         } else {
    //             actualScrollOffset = visibleScrollOffset + peakScrollSpeed;
    //         }
    //     }

    //     //The next two if statements move the buttons towards their final scroll destination
    //     float currentScrollIncrement = scrollIncrement * Mathf.Abs(visibleScrollOffset - actualScrollOffset);
    //     if(visibleScrollOffset > actualScrollOffset){
    //         //Moves the buttons towards the final location after the user scrolls them
    //         visibleScrollOffset -= currentScrollIncrement * Time.deltaTime;

    //         //Checks to see if the scroll movement has gotten close to its destination
    //         if((visibleScrollOffset <= actualScrollOffset) || Mathf.Abs(visibleScrollOffset - actualScrollOffset) < .01f){
    //             visibleScrollOffset = actualScrollOffset;
    //             currentlyScrolling = false;
    //         }
    //     } else {
    //         //Moves the buttons towards the final location after the user scrolls them
    //         visibleScrollOffset += currentScrollIncrement * Time.deltaTime;

    //         //Checks to see if the scroll movement has gotten close to its destination
    //         if((visibleScrollOffset >= actualScrollOffset) || Mathf.Abs(visibleScrollOffset - actualScrollOffset) < .01f){
    //             visibleScrollOffset = actualScrollOffset;
    //             currentlyScrolling = false;
    //         }
    //     }

    //     //Moves the buttons into their new positions
    //     updateButtonPositions();

    // }
}
