using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CarbonDial : MonoBehaviour
{
    public bool percentTextSmoothMove = true;
    public TextMeshProUGUI carbonPercentText;
    public TextMeshProUGUI netCarbonText;
    public RotateBetweenValues pointerRotate;
    public CarbonDialColor carbonDialColor;
    public CarbonDialSmoothMove dialSmoothMove;
    public ImageFillAmount carbonDialOuterRing;

    //Singleton
    public static CarbonDial current;


    void Start(){
        if(current == null){
            current = this;
        }else{
            Destroy(this);
        }
        dialSmoothMove = GetComponent<CarbonDialSmoothMove>();
        TickManager.TM.EndOfPollutionTick.AddListener(EndOfPollutionTick);
        GameEventManager.current.NetCarbonUpdated.AddListener(NetCarbonUpdated);
    }

    //Updates Carbon percent after each pollution tick
    private void EndOfPollutionTick(){
        UpdateCarbonDialUI();
    }

    //Updates net carbon text when net carbon changes
    private void NetCarbonUpdated(){
        UpdateNetCarbonText();
    }

    public void UpdateCarbonDialUI(){

        //Gets carbon variable values to update the UI
        float maxCarbon = LevelManager.LM.getMaxCarbon();
        float currentCarbon = LevelManager.LM.GetCarbon();
        float percentOfCarbonCapacityFilled = (maxCarbon == 0) ? 0 : currentCarbon / maxCarbon;

        //Caps carbon percent between 0.0 and 1.0
        percentOfCarbonCapacityFilled = (percentOfCarbonCapacityFilled > 1f) ? 1f : percentOfCarbonCapacityFilled;
        percentOfCarbonCapacityFilled = (percentOfCarbonCapacityFilled < 0f) ? 0f : percentOfCarbonCapacityFilled;

        if(dialSmoothMove != null){
            dialSmoothMove.BeginSmoothMoveToPercentFill(percentOfCarbonCapacityFilled);
        }
            

        //Updates carbon capacity percent text
        if(!percentTextSmoothMove){
            carbonPercentText.text = (int)(percentOfCarbonCapacityFilled * 100f) + "%";
        }
        
        

    }

    public void UnhideCarbonDial(){
        if(GetComponent<UnhideUIElement>() != null){
            GetComponent<UnhideUIElement>().ActivateUIElement();
        }
    }

    

    public void SetCarbonDialPercentFill(float percent){
        //Updates percent UI
        carbonDialColor.SetColorAsPercent(percent);
        pointerRotate.SetRotationAsPercent(percent);
        carbonDialOuterRing.SetFillAsPercent(percent);

        if(percentTextSmoothMove){
            carbonPercentText.text = (int)(percent * 100f) + "%";
        }
    }


    public void UpdateNetCarbonText(){
        float netCarbon = LevelManager.LM.NetCarbon;
        netCarbonText.text = "" + netCarbon;
    }
}
