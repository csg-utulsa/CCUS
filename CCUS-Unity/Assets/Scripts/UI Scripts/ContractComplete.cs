using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContractComplete : MonoBehaviour
{
    public Sprite complete;

    public void StarUpdate() //Transitions the star when its associated task is complete
    {
        //print("called");
        GetComponent<Image>().sprite = complete;
        //print(GetComponent<Image>().sprite);
    }
}
