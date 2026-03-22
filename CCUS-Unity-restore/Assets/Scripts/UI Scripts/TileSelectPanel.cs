using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TileSelectPanel : MonoBehaviour
{
    public static TileSelectPanel TSP;

    public float visibleScrollOffset = 0f;

    public float heightOfScrollableArea = 0f;

    public RectTransform rectangleTransform;

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
    float actualScrollOffset = 0f;
    bool currentlyScrolling = false;
    float totalHeightOfButtonArea  = 0f;
    float maxDistanceToScrollAsPercentOfPanelHeight = .3f;

    TileScriptableObject[] allTileScriptableObjects;
    Tile[] allTiles;

    public bool AButtonHasBeenSelectedAtLeastOnce {get; set;} = false;
    public bool ButtonIsCurrentlySelected {get; set;} = false;

    bool buttonIsSelected = false;
    bool ButtonIsSelected{
        get{
            return buttonIsSelected;
        }
        set{
            buttonIsSelected = value;
            if(value == true){
                AButtonHasBeenSelectedAtLeastOnce = true;
                ButtonIsCurrentlySelected = true;
                GameEventManager.current.ButtonHasBeenSelected.Invoke();
            }
        }
    }

    //Keeps track of which tiles are placeable at the moment
    bool[] disabledButtons;
    
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
            allTiles = new Tile[tileButtons.Length];
            //disabledPollutionButtons = new bool[tileButtons.Length];
            //disabledMoneyButtons = new bool[tileButtons.Length];
            disabledButtons = new bool[tileButtons.Length];
            for(int i = 0; i < tileButtons.Length; i++){
                allTileScriptableObjects[i] = tileButtons[i].GetComponent<buttonScript>().tileToPlace.GetComponent<Tile>().tileScriptableObject;
                disabledButtons[i] = false;
                allTiles[i] = tileButtons[i].GetComponent<buttonScript>().tileToPlace.GetComponent<Tile>();
            }

        }else{
            Debug.LogError("No buttons attached to the tile select panel! (uh oh)");
        }
        //Moves all the buttons to their proper position;
        updateButtonPositions();
        CheckPlaceabilityOfTiles();

        TickManager.TM.EndOfMoneyAndPollutionTicks.AddListener(EndOfTicks);
        GameEventManager.current.MoneyAmountUpdated.AddListener(MoneyAmountUpdated);
        GameEventManager.current.PersonJustAdded.AddListener(PersonAdded);
        GameEventManager.current.SwitchedCurrentGroundChunk.AddListener(SwitchedArea);
    }



    //Used to determine where the bottom button is
    public GameObject GetLowestActiveButton(){
        int indexOfNewestButton = 0;
        for(int i = 0; i < tileButtons.Length; i++){
            if(tileButtons[i].activeSelf){
                indexOfNewestButton = i;
            }
        }
        return tileButtons[indexOfNewestButton];
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

        //Displays "NEW TILE!" graphic
        unableToPlaceTileUI._unableToPlaceTileUI.newTile();

        //Calls event for when a new tile button is unlocked
        GameEventManager.current.NewTileUnlocked.Invoke();
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

    //Runs at the end of each money and pollution tick
    private void EndOfTicks(){
        CheckPlaceabilityOfTiles();
    }

    //Runs whenever the amount of money has been changed
    private void MoneyAmountUpdated(){
        CheckPlaceabilityOfTiles();
    }

    //Runs whenever a new person is added
    private void PersonAdded(){
        CheckPlaceabilityOfTiles();
    }

    //Runs whenever an area is switched
    private void SwitchedArea(){
        CheckPlaceabilityOfTiles();
    }

    public void CheckPlaceabilityOfTiles(){
        for(int i = 0; i < tileButtons.Length; i++){
            if(allTiles[i].CheckIfTileIsPlaceable(false)){
                disabledButtons[i] = false;
            } else{
                disabledButtons[i] = true;
            }
        }
        visuallyUpdateDisabledButtons();
        //updateActiveTileGraphics();
    }

    public void SetScrollOffset(float inputVisibleScrollOffset){
        
        visibleScrollOffset = inputVisibleScrollOffset;
        updateButtonPositions();

        // Invokes event for when the tile select panel scrolls.
        // It's used by the tool tip manager to move it into place.
        GameEventManager.current.TileSelectPanelScrolled.Invoke();
    }


    //turns disabled buttons red
    void visuallyUpdateDisabledButtons(){
        for(int i = 0; i < tileButtons.Length; i++){
            if(disabledButtons[i]){
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
        if(!ButtonIsSelected || clickedButton != selectedButton){

            //refuses to select button if its disabled
            int indexOfButtonClicked = Array.IndexOf(tileButtons, clickedButton);
            if(disabledButtons[indexOfButtonClicked]){
                allTiles[indexOfButtonClicked].CheckIfTileIsPlaceable(true);
                return;
            }

            ButtonIsSelected = true;
            selectedButton = clickedButton;
            BuildingSystem.current.InitializeWithObject(selectedButton.GetComponent<buttonScript>().tileToPlace);
            selectedButtonGraphic.SetActive(true);
            //selectedButtonGraphic.GetComponent<RectTransform>().anchoredPosition = new Vector2(selectedButton.GetComponent<RectTransform>().anchoredPosition.x, selectedButton.GetComponent<RectTransform>().anchoredPosition.y);
            selectedButtonGraphic.transform.position = selectedButton.transform.position;
        } 

        //runs if the user clicks the button that is already selected, and so deselects it.
        else {
            deselectAllButtons();
        }
            
    }

    public void deselectAllButtons(){
        selectedButtonGraphic.SetActive(false);
        selectedButton = new GameObject();
        BuildingSystem.current.deselectCurrentObject();
        ButtonIsSelected = false;
    }


    public float GetHeightOfButtons(){

        int numOfActiveTiles = 0;
        foreach(GameObject tileButton in tileButtons){
            if(tileButton.activeSelf){
                numOfActiveTiles++;
            }
        }

        float panelHeight = GetComponent<RectTransform>().rect.height;
        float buttonPlacementPosition = /* center of panel */GetComponent<RectTransform>().anchoredPosition.y + /*half of height of panel*/ ((.5f)*panelHeight) - /* distance from top of panel */ distanceFromTopOfPanel;
        return (-buttonPlacementPosition) + (numOfActiveTiles * (heightOfTile + gapBetweenTiles)); // 50f;
    }

    public bool isMouseOverScrollingPanel(){
        Vector2 mousePos = Input.mousePosition;
        return RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), mousePos);
    }


    //This function moves all of the buttons to their proper positions in the RectTransform when called
    public void updateButtonPositions(){
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
        if(ButtonIsSelected){
            selectedButtonGraphic.GetComponent<RectTransform>().anchoredPosition = new Vector2(selectedButtonGraphic.GetComponent<RectTransform>().anchoredPosition.x, selectedButton.GetComponent<RectTransform>().anchoredPosition.y);
        }
        

        //Sets height of how far the user can scroll
        totalHeightOfButtonArea  = Mathf.Abs(topOfScrollingArea - bottomOfScrollingArea);
        heightOfScrollableArea = totalHeightOfButtonArea - (maxDistanceToScrollAsPercentOfPanelHeight * panelHeight);//= (totalHeightOfButtonArea > panelHeight) ? (totalHeightOfButtonArea - panelHeight) : 0f;
    }


}
