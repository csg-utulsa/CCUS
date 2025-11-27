using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class PanelOpener : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject panel; //The object that will be opened and collapsed
    bool isActive = true;
    private Vector3 scaleChange = new Vector3(0.1f, 0.1f, 0); //Scale for the button's change

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.transform.localScale += scaleChange;
        //print("Mouse has entered");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.transform.localScale -= scaleChange;
        //print("Mouse has left");
    }

    public void OpenPanel()
    {
            Vector2 placement = panel.transform.position;
            //Using written coords, will later implement way to make based on the contract buttons position
            if (isActive) //If the panel is currently up it'll turn off the pannel and shift the button down
            {                
                panel.transform.position -= new Vector3(0,180,0);
                isActive = false;
            }
            else //If the pannel is off it'll move the button up and bring back the pannel
            {
                panel.transform.position += new Vector3(0,180,0);
                isActive = true;
            }
            
        
    }

}
