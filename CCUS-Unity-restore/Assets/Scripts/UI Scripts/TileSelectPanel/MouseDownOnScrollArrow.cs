using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;

public class MouseDownOnScrollArrow : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    NewTileArrow tileArrowController;

    public void OnPointerUp(PointerEventData eventData) {
    }

    public void OnPointerDown(PointerEventData eventData) {
        tileArrowController = null;

        //Gets tile arrow controller element from parent.
        try{
            tileArrowController = transform.parent.gameObject.GetComponent<NewTileArrow>();
        } catch {
            Debug.LogError("Failed to find Tile Arrow Controller");
        }

        //Null Check
        if(tileArrowController == null){
            return;
        }

        tileArrowController.ArrowClicked();

        
    }
}
