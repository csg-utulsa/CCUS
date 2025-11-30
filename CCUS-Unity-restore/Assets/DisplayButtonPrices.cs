using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayButtonPrices : MonoBehaviour
{
    public buttonScript[] buttons;

    public GameObject buttonPricePrefab;

    private Vector3 buttonPriceLocationOffset = new Vector3(0f, 0f, 0f);

    public float offsetAsPercentageOfHeight = -0.5f;


    // Start is called before the first frame update
    void Start()
    {
        float myHeight = GetComponent<RectTransform>().rect.height;
        buttonPriceLocationOffset = new Vector3(buttonPriceLocationOffset.x, myHeight * offsetAsPercentageOfHeight, buttonPriceLocationOffset.z);
        buttons = GetComponentsInChildren<buttonScript>(true);
        foreach(buttonScript button in buttons){
            GameObject newButtonPriceObject = Instantiate(buttonPricePrefab, button.gameObject.transform);
            if(newButtonPriceObject.GetComponentInChildren<Image>() != null)
                //newButtonPriceObject.GetComponentInChildren<Image>().gameObject.transform.position = button.gameObject.transform.position + buttonPriceLocationOffset;
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
