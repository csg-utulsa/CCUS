using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopUps : MonoBehaviour
{
    public float heightAboveObject = 5f;

    public GameObject dollarSign;
    public GameObject carbonCloud;
    public GameObject carbonRemovalCloud;

    public float maxRangeToIncreaseSize = 400f;

    //This function displays pop ups over the money generating tiles.
    public void displayMoneyPopUps(){
        Tile[] allGridObjects = GridManager.GM.AllTileTracker.GetAllTiles();
        foreach(Tile gridObject in allGridObjects){
            if(gridObject != null){
                float tileAnnualIncome = gridObject.tileScriptableObject.AnnualIncome;
                if(tileAnnualIncome > 0){
                    GameObject newDollarSign = Instantiate(dollarSign, gridObject.gameObject.transform.position + new Vector3(0f, heightAboveObject, 0f), dollarSign.transform.rotation);
                    newDollarSign.GetComponent<UIPopUpSize>().setSize(tileAnnualIncome, LevelManager.LM.getCurrentMaxTileIncome(), maxRangeToIncreaseSize);
                    
                }
            }
            
        }
    }

    //This function displays pop ups over the carbon generating pop ups.
    public void displayCarbonPopUps(){
        
        Tile[] allGridObjects = GridManager.GM.AllTileTracker.GetAllTiles();
        foreach(Tile gridObject in allGridObjects){
            if(gridObject != null){
                float tileAnnualCarbon = gridObject.tileScriptableObject.AnnualCarbonAdded;
                if(tileAnnualCarbon > 0){
                    //Debug.Log("Carbon Trying to Pop");
                    GameObject newCarbonCloud = Instantiate(carbonCloud, gridObject.gameObject.transform.position + new Vector3(0f, heightAboveObject, 0f), dollarSign.transform.rotation);
                    newCarbonCloud.GetComponent<UIPopUpSize>().setSize(tileAnnualCarbon, LevelManager.LM.getCurrentMaxTileCarbon(), maxRangeToIncreaseSize);
                } else if(tileAnnualCarbon < 0){
                    GameObject newCarbonRemovalCloud = Instantiate(carbonRemovalCloud, gridObject.gameObject.transform.position + new Vector3(0f, heightAboveObject, 0f), dollarSign.transform.rotation);
                    newCarbonRemovalCloud.GetComponent<UIPopUpSize>().setSize(tileAnnualCarbon, LevelManager.LM.getCurrentMinTileCarbon(), maxRangeToIncreaseSize);
                }
            }
            
        }
    }
    
    // //This function displays pop ups over the money generating tiles. (ITS NOT FINISHED AND DOESNT WORK)
    // public void displayPopUpsOfType(GameObject popUpToInstantiate, float amountOfResourceProduced, Func<float, bool> amountOfResourceConditionCheck){
    //     GameObject[] allGridObjects = GridManager.GM.GetAllGridObjects();
    //     foreach(GameObject gridObject in allGridObjects){
    //         if(gridObject.GetComponent<Tile>() != null){
    //             //float tileAnnualIncome = gridObject.GetComponent<Tile>().tileScriptableObject.AnnualIncome;
    //             if(amountOfResourceConditionCheck(amountOfResourceProduced)){
    //                 GameObject newUIPopUp = Instantiate(dollarSign, gridObject.transform.position + new Vector3(0f, heightAboveObject, 0f), dollarSign.transform.rotation);
    //                 newUIPopUp.GetComponent<UIPopUpSize>().setSize(Mathf.Abs(amountOfResourceProduced), LevelManager.LM.getCurrentMaxTileIncome(), maxRangeToIncreaseSize);
    //             }
    //         }
            
    //     }
    // }

    // Update is called once per frame
    void Update()
    {

    }
}
