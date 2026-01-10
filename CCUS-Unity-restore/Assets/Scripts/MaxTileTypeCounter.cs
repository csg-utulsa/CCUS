using UnityEngine;
using TMPro;

public class MaxTileTypeCounter : MonoBehaviour
{
    public static MaxTileTypeCounter current;
    public TextMeshProUGUI NumberOfCarbonCaptureSystemsText;
    public TextMeshProUGUI MaxCarbonCaptureSystemsText;
    
    private int numberOfCarbonCaptureSystems = 0;
    public int NumberOfCarbonCaptureSystems {
        get{
            return numberOfCarbonCaptureSystems;
        }
        set{
            numberOfCarbonCaptureSystems = value;
            if(NumberOfCarbonCaptureSystemsText != null){
                NumberOfCarbonCaptureSystemsText.text = "" +  value;
            }
        }
    }

    private int maxCarbonCaptureSystems = 0;
    public int MaxCarbonCaptureSystems {
        get{
            return maxCarbonCaptureSystems;
        }
        set{
            maxCarbonCaptureSystems = value;
            if(MaxCarbonCaptureSystemsText != null){
                MaxCarbonCaptureSystemsText.text = "" + value;
            }
            
        }
    }

    void Awake(){
        if(current == null){
            current = this;
        }else{
            Destroy(this);
        }
    }

    public bool UnderMaxCarbonCaptureTiles(){
        if(NumberOfCarbonCaptureSystems < MaxCarbonCaptureSystems){
            return true;
        } else{
            return false;
        }
    }

    public void UpdateNumberOfCarbonCaptureSystems(){
        NumberOfCarbonCaptureSystems = TileTypeCounter.current.CarbonCaptureTileTracker.GetAllTiles().Length;
    }

    public void SetMaxCarbonCaptureTiles(int maxNumber){
        MaxCarbonCaptureSystems = maxNumber;
    }
}
