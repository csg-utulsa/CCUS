using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelOpener : MonoBehaviour
{
    public GameObject panel; //The object that will be opened and collapsed
    public Vector2 open;
    public Vector2 close;
    bool isActive = true;
    // Start is called before the first frame update

    public void OpenPanel()
    {
            
            //Using written coords, will later implement way to make based on the contract buttons position
            if (isActive) //If the panel is currently up it'll turn off the pannel and shift the button down
            {                
                panel.transform.position = close;
                print(isActive);
                isActive = false;
            }
            else //If the pannel is off it'll move the button up and bring back the pannel
            {
                panel.transform.position = open;
                isActive = true;
            }
            
        
    }

}
