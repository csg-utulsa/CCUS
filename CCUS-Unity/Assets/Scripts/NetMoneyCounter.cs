using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NetMoneyCounter : MonoBehaviour
{
    public static NetMoneyCounter NMC;
    [SerializeField] TextMeshProUGUI netMoneyText;
    private string startText;

    // Awake is called before Start, which is itself called before the very first frame update
    void Awake()
    {
        if(NMC == null){
            NMC = this;
        }
        
        if(GetComponent<TextMeshProUGUI>() != null){
            netMoneyText = GetComponent<TextMeshProUGUI>();
            startText = netMoneyText.text;
        }

    }

    void Start(){
        UpdateNetMoneyCounter();
    }

    public void UpdateNetMoneyCounter(){
        if(netMoneyText != null){
            netMoneyText.text = startText + " $" + LevelManager.LM.NetMoney;
        }
    }
}
