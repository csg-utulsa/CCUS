using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelOpener : MonoBehaviour
{
    public GameObject panel; //The object that will be opened and collapsed
    bool isActive = true;
    // Start is called before the first frame update

    public void OpenPanel()
    {
            Vector2 placement = panel.transform.position;
            //Using written coords, will later implement way to make based on the contract buttons position
            if (isActive) //If the panel is currently up it'll turn off the pannel and shift the button down
            {                
                panel.transform.position -= new Vector3(0,180,0);
                print(isActive);
                isActive = false;
            }
            else //If the pannel is off it'll move the button up and bring back the pannel
            {
                panel.transform.position += new Vector3(0,180,0);
                isActive = true;
            }
            
        
    }

}
