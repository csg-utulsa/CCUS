using UnityEngine;
using UnityEngine.UI;

public class UIPopUpHideOnMouseOver : MonoBehaviour
{
    private Color originalColor;
    Image myImage;
    private Tile tileHoveringOver;
    public Tile TileHoveringOver {
        get{
            return tileHoveringOver;
        }
        set{
            //Debug.Log("Setting tile hovering over");
            tileHoveringOver = value;
            // if(tileHoveringOver.GetComponent<MouseHoverHideTile>() != null && tileHoveringOver.GetComponent<MouseHoverHideTile>().IsHidden){
            //     HidePopUp();
            // }
        }
    }
    void Awake(){
        myImage = GetComponentInChildren<Image>();
        if(myImage != null) originalColor = myImage.color;
        // if(TileHoveringOver != null){
        //     Debug.Log("have tile to hover over");
            
        // }
    }

    public void HidePopUp(float transparency){
        if(myImage != null){
            myImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, transparency);
        }
    }

    public void UnHidePopUp(){
        if(myImage != null){
            myImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, originalColor.a);
        }
    }
}
