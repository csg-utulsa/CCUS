using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelOpener : MonoBehaviour
{

    public GameObject panel; //The object that will be opened and collapsed
    // Start is called before the first frame update

    public void OpenPanel()
    {
        if(panel != null)
        {
            bool isActive = panel.activeSelf;
            panel.SetActive(!isActive);
        }
    }

}
