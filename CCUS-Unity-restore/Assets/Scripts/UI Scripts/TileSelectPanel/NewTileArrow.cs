using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;
using System;

// Every time a new tile is added, this checks if it's visible.
// If it's not, a downward arrow appears.

public class NewTileArrow : MonoBehaviour
{   
    public RectTransform newestTile;
    public bool playerHasSeenNewestTile;

    public GameObject newTileArrowGraphic;

    public TileSelectPanelScroll scrollingUI;

    void Start(){
        GameEventManager.current.NewTileUnlocked.AddListener(NewTileUnlocked);
    }

    void Update(){
        if(!playerHasSeenNewestTile && newestTile != null){

            //Turns off new tile arrow graphic when bottom button is visible to player
            if(NewestTileIsVisible()){

                playerHasSeenNewestTile = true;

                if(newTileArrowGraphic != null){
                    //newTileArrowGraphic.SetActive(false);
                    GetComponent<FadeChildGraphicsToTransparency>().FadeAllChildGraphicsToTransparency(0f);
                } else {Debug.LogError("Assign Graphic in Inspector.");}
                
            }

        }
    }

    //Called every time a new tile is unlocked
    private void NewTileUnlocked(){

        //Gets what the newest tile is
        newestTile = null;
        try{
            newestTile = TileSelectPanel.TSP.GetLowestActiveButton().GetComponent<RectTransform>();
        } catch (Exception ex) {
            Debug.LogError("Failed to find the newest tile.");
        }

        
        if(NewestTileIsVisible()){
            return;
        } else{
            playerHasSeenNewestTile = false;

            if(newTileArrowGraphic != null){
                GetComponent<FadeChildGraphicsToTransparency>().FadeAllChildGraphicsToTransparency(1f);
            } else {Debug.LogError("Assign Graphic in Inspector.");}
            
        }
    }

    //Checks if the most recently unlocked tile is visible
    private bool NewestTileIsVisible(){

        //Null check for newest tile
        if(newestTile == null){
            return true;
        }

        Rect newestTileButtonScreenPosRect = RectTransformFunctions.current.RectTransformToScreenSpace(newestTile);

        float bottomOfNewestTile = newestTileButtonScreenPosRect.position.y - (0.5f * newestTileButtonScreenPosRect.height);

        float bottomOfScreen = Camera.main.ScreenToWorldPoint(Vector3.zero).y;


        if(bottomOfNewestTile > bottomOfScreen){
            return true;
        } else{
            return false;
        }


    }


    //Called whenever arrow image is clicked.
    public void ArrowClicked() {
        if(scrollingUI != null){
           scrollingUI.ScrollToBottomButton(); 
        } else{
            Debug.LogError("Assign TileSelectPanelScroll Script in the inspector.");
        }
    }



}
