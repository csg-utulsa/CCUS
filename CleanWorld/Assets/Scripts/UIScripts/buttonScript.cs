using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class buttonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public GameObject tileToPlace;
    
    
    //Displays ToolTip
    public void OnPointerEnter(PointerEventData eventData)
    {
        int carbon = 0;
        int money = 0;
        if(tileToPlace.GetComponent<Tile>() != null){
            TileScriptableObject scriptableObject = tileToPlace.GetComponent<Tile>().tileScriptableObject;
            money = scriptableObject.AnnualIncome;
            carbon = scriptableObject.AnnualCarbonAdded;
            ToolTipManager.TTM.activateToolTip(tileToPlace.GetComponent<Tile>(), gameObject);
        }
        
        
    }
    

    //Hides ToolTip
    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTipManager.TTM.deactivateToolTip();
    }


}
