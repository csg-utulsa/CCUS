using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopUps : MonoBehaviour
{
    public bool changeUIPopUpSize = false;
    public float heightAboveObject = 5f;

    public GameObject dollarSign;
    public GameObject carbonCloud;
    public GameObject carbonRemovalCloud;

    public float maxRangeToIncreaseSize = 400f;

    //This function displays pop ups over the money generating tiles.
    public void displayMoneyPopUps(){
        Tile[] allGridObjects = GridManager.GM.GetAllTilesOnActiveChunk();
        //Iterates through each tile currently on the grid
        foreach(Tile gridObject in allGridObjects){

            
            if(gridObject != null){

                //Only displays money pop ups over activatable tiles if they're activated
                if(gridObject is ActivatableTile activatableGridObject){
                    if(!activatableGridObject.IsActivated){
                        continue;
                    }
                }
                
                float tileAnnualIncome = gridObject.tileScriptableObject.AnnualIncome;
                if(tileAnnualIncome > 0){
                    GameObject positionToInstantiatePopUp = FirstChildWithTag(gridObject.gameObject, "ReferencePositionForUIPopUps");
                    
                    //Checks if tile has an empty gameObject to use as a reference position for where to instantiate the UI pop ups
                    GameObject objectAtPositionToInstantiatePopUp = FirstChildWithTag(gridObject.gameObject, "ReferencePositionForUIPopUps");
                    Vector3 positionToInstantiate;
                    if(objectAtPositionToInstantiatePopUp != null){
                        positionToInstantiate = objectAtPositionToInstantiatePopUp.transform.position;
                    }else{
                        positionToInstantiate = gridObject.gameObject.transform.position + new Vector3(0f, heightAboveObject, 0f);
                    }


                    GameObject newDollarSign = Instantiate(dollarSign, positionToInstantiate, dollarSign.transform.rotation);
                    if(changeUIPopUpSize){
                        newDollarSign.GetComponent<UIPopUpSize>().setSize(tileAnnualIncome, LevelManager.LM.getCurrentMaxTileIncome(), maxRangeToIncreaseSize);
                    }
                    MouseHoverHideTile tileHider = gridObject.gameObject.GetComponent<MouseHoverHideTile>();
                    if(tileHider != null) tileHider.CurrentUIPopUp = newDollarSign;
                }
                
                
            }
            
        }
    }

    //This function displays pop ups over the carbon generating pop ups.
    public void displayCarbonPopUps(){
        Tile[] allGridObjects = GridManager.GM.GetAllTilesOnActiveChunk();

        //Iterates through each object currently on the grid
        foreach(Tile gridObject in allGridObjects){
            if(gridObject != null){

                //If the tile is an activatable tile, it won't put pop ups over them
                if(gridObject is ActivatableTile activatableGridObject){
                    if(!activatableGridObject.IsActivated) continue;
                }

                //Checks if tile has an empty gameObject to use as a reference position for where to instantiate the UI pop ups
                GameObject objectAtPositionToInstantiatePopUp = FirstChildWithTag(gridObject.gameObject, "ReferencePositionForUIPopUps");
                Vector3 positionToInstantiate;
                if(objectAtPositionToInstantiatePopUp != null){
                    positionToInstantiate = objectAtPositionToInstantiatePopUp.transform.position;
                }else{
                    positionToInstantiate = gridObject.gameObject.transform.position + new Vector3(0f, heightAboveObject, 0f);
                }

                //Instantiates carbon and carbon-removal pop ups over objects
                float tileAnnualCarbon = gridObject.tileScriptableObject.AnnualCarbonAdded;
                if(tileAnnualCarbon > 0){
                    GameObject newCarbonCloud = Instantiate(carbonCloud, positionToInstantiate, dollarSign.transform.rotation);
                    if(changeUIPopUpSize){
                        newCarbonCloud.GetComponent<UIPopUpSize>().setSize(tileAnnualCarbon, LevelManager.LM.getCurrentMaxTileCarbon(), maxRangeToIncreaseSize);
                    }
                    
                    MouseHoverHideTile tileHider = gridObject.gameObject.GetComponent<MouseHoverHideTile>();
                    if(tileHider != null) tileHider.CurrentUIPopUp = newCarbonCloud;
                } else if(tileAnnualCarbon < 0){
                    GameObject newCarbonRemovalCloud = Instantiate(carbonRemovalCloud, positionToInstantiate, dollarSign.transform.rotation);
                    if(changeUIPopUpSize){
                        newCarbonRemovalCloud.GetComponent<UIPopUpSize>().setSize(tileAnnualCarbon, LevelManager.LM.getCurrentMinTileCarbon(), maxRangeToIncreaseSize);
                    }
                    
                    MouseHoverHideTile tileHider = gridObject.gameObject.GetComponent<MouseHoverHideTile>();
                    if(tileHider != null) tileHider.CurrentUIPopUp = newCarbonRemovalCloud;
                }
                
            }
            
        }
    }

    //Returns the first child of a GameObject that has a given tag
    private GameObject FirstChildWithTag(GameObject parentToCheck, string tagToCheck){
        foreach(Transform childTransform in parentToCheck.transform){
            if(childTransform.tag == tagToCheck){
                return childTransform.gameObject;
            }
        }
        return null;
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
