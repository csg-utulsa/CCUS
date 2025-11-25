using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NetCarbonCounter : MonoBehaviour
{
    public static NetCarbonCounter NCC;
    [SerializeField] TextMeshProUGUI netCarbonText;
    private string startText;

    // Awake is called before Start, which is itself called before the very first frame update
    void Awake()
    {
        if(NCC == null){
            NCC = this;
        }
        
        if(GetComponent<TextMeshProUGUI>() != null){
            netCarbonText = GetComponent<TextMeshProUGUI>();
            startText = netCarbonText.text;
        }
        
    }

    void Start(){
        UpdateNetCarbonCounter();
    }


    public void UpdateNetCarbonCounter(){
        if(netCarbonText != null){
            netCarbonText.text = startText + " " + LevelManager.LM.NetCarbon;
        }
    }
}
