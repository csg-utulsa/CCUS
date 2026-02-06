using UnityEngine;
using TMPro;

public class MaxCarbonCaptureTileError : MonoBehaviour
{
    public TextMeshProUGUI numOfCarbonCaptureSystemsText;
    public TextMeshProUGUI maxNumOfCarbonCaptureSystemsText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Updates max number of carbon capture systems.
        if(maxNumOfCarbonCaptureSystemsText != null){
            maxNumOfCarbonCaptureSystemsText.text = "" + MaxTileTypeCounter.current.MaxCarbonCaptureSystems;
        }else{
            Debug.LogError("No max carbon capture system num text object assigned to this script.");
        }

        //Updates current number of carbon capture systems.
        if(numOfCarbonCaptureSystemsText != null){
            numOfCarbonCaptureSystemsText.text = "" + MaxTileTypeCounter.current.NumberOfCarbonCaptureSystems;
        }else{
            Debug.LogError("No carbon capture system num text object assigned to this script.");
        }

        //Adds a listener for whenever the number of carbon capture tiles is changed.
        GameEventManager.current.NumOfCarbonCaptureTilesChanged.AddListener(CarbonCaptureNumChanged);
    }

    //Called whenever the number of carbon capture tiles is changed.
    private void CarbonCaptureNumChanged(){
        //Updates current number of carbon capture systems.
        if(numOfCarbonCaptureSystemsText != null){
            numOfCarbonCaptureSystemsText.text = "" + MaxTileTypeCounter.current.NumberOfCarbonCaptureSystems;
        }else{
            Debug.LogError("No carbon capture system num text object assigned to this script.");
        }
    }
}
