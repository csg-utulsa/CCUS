using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DisplayDisableFactoryPopUps : MonoBehaviour
{
    public GameObject notEnoughPeoplePopUp;
    public float heightAboveObject = 5f;

    private List<Tile> previouslyDisabledFactories = new List<Tile>();
    private List<GameObject> currentPopUps = new List<GameObject>();

    //This function displays the not-enough-people pop ups over disabled factoris
    public void DisplayNotEnoughPeoplePopUps(Tile[] disabledFactories){

        //Iterates through each disabled factory
        foreach(Tile disabledFactory in disabledFactories){
            if(disabledFactory != null){
                
                //Doesn't add pop ups to workplaces that already have them
                if(previouslyDisabledFactories.Contains(disabledFactory)){
                    Debug.Log("Already has a pop up!");
                    previouslyDisabledFactories.Remove(disabledFactory);
                    continue;
                }

                Debug.Log("Creating pop up for factory");
                CreatePopUpForTile(disabledFactory);
                
            }
            
        }

        //Removes the pop up from workplaces that are no longer disabled
        foreach(Tile previouslyDisabledFactory in previouslyDisabledFactories){
            DisabledFactoryPopUp popUp = GetWorkplacePopUp(previouslyDisabledFactory);
            if(popUp != null){
                Debug.Log("Destroying pop up");
                currentPopUps.Remove(popUp.gameObject);
                Destroy(popUp.gameObject);
            }
        }

        //Resets the list of previously disabled workplaces
        previouslyDisabledFactories.Clear();
        previouslyDisabledFactories.AddRange(disabledFactories);


    }

    private DisabledFactoryPopUp GetWorkplacePopUp(Tile workplace){
        foreach(GameObject popUp in currentPopUps){
            DisabledFactoryPopUp popUpScript = popUp.GetComponent<DisabledFactoryPopUp>();
            if(popUpScript.myFactory == workplace){
                return popUpScript;
            }
        }
        return null;
    }

    //Creates a pop up for the input tile
    private void CreatePopUpForTile(Tile disabledFactory){

        //Checks if tile has an empty gameObject to use as a reference position for where to instantiate the UI pop ups
        GameObject objectAtPositionToInstantiatePopUp = FirstChildWithTag(disabledFactory.gameObject, "ReferencePositionForUIPopUps");
        Vector3 positionToInstantiate;
        if(objectAtPositionToInstantiatePopUp != null){
            positionToInstantiate = objectAtPositionToInstantiatePopUp.transform.position;
        }else{
            positionToInstantiate = disabledFactory.gameObject.transform.position + new Vector3(0f, heightAboveObject, 0f);
        }

        MouseHoverHideTile tileHider = disabledFactory.gameObject.GetComponent<MouseHoverHideTile>();

        GameObject newPopUp = Instantiate(notEnoughPeoplePopUp, positionToInstantiate, notEnoughPeoplePopUp.transform.rotation);
        currentPopUps.Add(newPopUp);
        tileHider.CurrentUIPopUp = newPopUp;

        // Gives the pop up the tile's script
        DisabledFactoryPopUp popUpScript = newPopUp.GetComponent<DisabledFactoryPopUp>();
        if(popUpScript != null){
            popUpScript.myFactory = disabledFactory;
        }
    }

    // private bool TileHiderContainsDisabledFactoryPopUP(MouseHoverHideTile tileHider){
    //     GameObject[] tilesCurrentPopUps = tileHider.CurrentUIPopUps.ToArray();
    //     foreach(GameObject popUp in tilesCurrentPopUps){
    //         if(popUp != null && popUp.GetComponent<DisabledFactoryPopUp>() != null){
    //             return true;
    //         }
    //     }
    //     return false;
    // }

    //Returns the first child of a GameObject that has a given tag
    private GameObject FirstChildWithTag(GameObject parentToCheck, string tagToCheck){
        foreach(Transform childTransform in parentToCheck.transform){
            if(childTransform.tag == tagToCheck){
                return childTransform.gameObject;
            }
        }
        return null;
    }

}
