using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContractStars : MonoBehaviour
{
    public Sprite incomplete;
    public Sprite complete;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().sprite = incomplete;
    }

    // Update is called once per frame
    void Update()
    {
        Invoke("StarUpdate", 5);
    }

    public void StarUpdate() //Transitions the star when its associated task is complete
    {
        GetComponent<Image>().sprite = complete;
    }
}
