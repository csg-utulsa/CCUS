using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourcePanelManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI yearText;
    [SerializeField] TextMeshProUGUI moneyText;
    //[SerializeField] TextMeshProUGUI storageText;
    [SerializeField] TextMeshProUGUI carbonText;
    [SerializeField] CarbonRotate carbonDial;
    private string spacing = ""; //The amount of spacing for text
    LevelManager dm;

    // Start is called before the first frame update
    void Start()
    {
        dm = LevelManager.LM;
    }

    // Update is called once per frame
    void Update()
    {
        if(yearText != null)
        {
            yearText.text = "Year: " + dm.GetYear().ToString();
        }

        if(moneyText != null)
        {
            for(int i = 0; i < 6 - dm.GetMoney().ToString().Length; i++)
            {
                spacing += "0";
            }
            moneyText.text = spacing + dm.GetMoney().ToString();
        }
        spacing = "";

        /*if(storageText != null)
        {
            storageText.text = "Carbon Storage: " + dm.GetStored();
        }
        */
        if(carbonDial != null)
        {
            carbonDial.UpdateCarbon(dm.GetCarbon()); //Updates the Dial
            int carbonPercentage = (dm.GetCarbon() / 100); //Transforms the carbon number into a percent

            carbonText.text = (carbonPercentage).ToString() + "%"; //Changes component to match current carbon percent
        }
    }
}
