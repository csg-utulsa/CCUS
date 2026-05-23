using UnityEngine;
using TMPro;

public class MaxTileTypeCounter : MonoBehaviour
{
    public static MaxTileTypeCounter current;
    //public TextMeshProUGUI NumberOfCarbonCaptureSystemsText;
    //public TextMeshProUGUI MaxCarbonCaptureSystemsText;
    //public SlideInAnimation maxCarbonCaptureSystemsSlideInAnimation;
    private bool carbonCaptureSystemsEnabled = false;
    
    //private int numberOfCarbonCaptureSystems = 0;
    public int NumberOfCarbonCaptureSystems {
        get{
            return TileTypeCounter.current.CarbonCaptureTileTracker.GetAllTiles().Length;
        }
        set{

            // numberOfCarbonCaptureSystems = value;
            // if(NumberOfCarbonCaptureSystemsText != null){
            //     NumberOfCarbonCaptureSystemsText.text = "" +  value;
            // }
        }
    }

    public int maxCarbonCaptureTilesPerArea = 5;
    //private int maxCarbonCaptureSystems = 0;
    public int MaxCarbonCaptureSystems {
        get{
            return maxCarbonCaptureTilesPerArea;
        }
        //set{
            // maxCarbonCaptureSystems = value;
            // if(MaxCarbonCaptureSystemsText != null){
            //     MaxCarbonCaptureSystemsText.text = "" + value;
            // }
            // if(!carbonCaptureSystemsEnabled && maxCarbonCaptureSystems > 0){
            //     maxCarbonCaptureSystemsSlideInAnimation.SlideIntoView();
            //     carbonCaptureSystemsEnabled = true;
            // } else if(carbonCaptureSystemsEnabled && maxCarbonCaptureSystems <= 0){
            //     maxCarbonCaptureSystemsSlideInAnimation.SlideOutOfView();
            //     carbonCaptureSystemsEnabled = false;
            // }
            
        //}
    }


    void Awake(){
        if(current == null){
            current = this;
        }else{
            //Destroy(this);
        }
    }

    public bool UnderMaxCarbonCaptureTiles(){
        if(NumberOfCarbonCaptureSystems < MaxCarbonCaptureSystems){
            return true;
        } else{
            return false;
        }
    }

    // public void UpdateNumberOfCarbonCaptureSystems(){
    //     NumberOfCarbonCaptureSystems = TileTypeCounter.current.CarbonCaptureTileTracker.GetAllTiles().Length;
    // }

    // public void SetMaxCarbonCaptureTiles(int maxNumber){
    //     MaxCarbonCaptureSystems = maxNumber;
    // }
}
