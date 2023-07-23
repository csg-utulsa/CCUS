using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourcePanelManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI yearText;
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] TextMeshProUGUI storageText;

    DataManager dm;

    // Start is called before the first frame update
    void Start()
    {
        dm = DataManager.DM;
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
            moneyText.text = "Current funds: $" + dm.GetMoney().ToString();
        }

        if(storageText != null)
        {
            storageText.text = "Carbon Storage: " + dm.GetStored();
        }
    }
}
