using UnityEngine;

public class ErrorGraphicDisplayer : MonoBehaviour
{
    bool wasJustDisplaying = false;
    float graphicTimer = 0f;
    float timeToShowGraphic;
    GameObject errorGraphic;
    

    public ErrorGraphicDisplayer(float _timeToShowGraphic, GameObject _errorGraphic){
        timeToShowGraphic = _timeToShowGraphic;
        errorGraphic = _errorGraphic;
        Debug.Log("Constructor");
    }

    public void SetValues(float _timeToShowGraphic, GameObject _errorGraphic){
        timeToShowGraphic = _timeToShowGraphic;
        errorGraphic = _errorGraphic;
        Debug.Log("Constructor");
    }
    
    public void DisplayErrorGraphic(){
        graphicTimer = timeToShowGraphic;
        wasJustDisplaying = true;
        errorGraphic.SetActive(true);

        ShakeGraphic shakeGraphic = errorGraphic.GetComponent<ShakeGraphic>();
        if(shakeGraphic != null) shakeGraphic.ShakeItUp();
        
        FadeGraphic fadeGraphic = errorGraphic.GetComponent<FadeGraphic>();
        if(fadeGraphic != null) fadeGraphic.StopFading();
    }

    void Update(){
        Debug.Log("UPDATING");
        //The next two if else structures time how long the graphics should be visible for
        if(graphicTimer > 0f){
            graphicTimer -= Time.deltaTime;
        } else{
            wasJustDisplaying = false;
            Debug.Log(errorGraphic);
            FadeGraphic fadeGraphic = errorGraphic.GetComponent<FadeGraphic>();
            if(fadeGraphic != null) fadeGraphic.beginFading();
        }
        
    }

}
