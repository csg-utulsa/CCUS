using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayButtonPrices : MonoBehaviour
{
    public buttonScript[] buttons;

    public GameObject buttonPricePrefab;

    public Vector3 buttonPriceLocationOffset = new Vector3(0f, 0f, 0f);


    // Start is called before the first frame update
    void Start()
    {
        buttons = GetComponentsInChildren<buttonScript>(true);
        foreach(buttonScript button in buttons){
            GameObject newButtonPriceObject = Instantiate(buttonPricePrefab, button.gameObject.transform);
            if(newButtonPriceObject.GetComponentInChildren<Image>() != null)
                newButtonPriceObject.GetComponentInChildren<Image>().gameObject.transform.position = button.gameObject.transform.position + buttonPriceLocationOffset;
            if(button.tileToPlace != null && button.tileToPlace.GetComponent<Tile>() != null){
                newButtonPriceObject.GetComponentInChildren<TextMeshProUGUI>().text = "$" + button.tileToPlace.GetComponent<Tile>().tileScriptableObject.BuildCost;
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
