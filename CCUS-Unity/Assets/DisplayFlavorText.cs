using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class DisplayFlavorText : MonoBehaviour
{
    TextMeshProUGUI textbox;
    public TileScriptableObject tileScriptableObject;
    // Start is called before the first frame update
    void Start()
    {
        textbox = gameObject.GetComponent<TextMeshProUGUI>();
        textbox.text = "Build cost:" + tileScriptableObject.BuildCost + "\n"
                     + "Annual cost:" + tileScriptableObject.AnnualCost + "\n"
                     + "Annual income:" + tileScriptableObject.AnnualIncome + "\n"
                     + "Annual carbon removed:" + (tileScriptableObject.AnnualCarbonStored + tileScriptableObject.AnnualCarbonRemoved) + "\n"
                     + "Annual carbon added:" + tileScriptableObject.AnnualCarbonAdded + "\n"
                     + tileScriptableObject.FlavorText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
