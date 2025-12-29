using UnityEngine;

public class creativeModeScript : MonoBehaviour
{

    bool creativeMode = false;
    bool wasJustInCreativeMode = false;
    float previousMaxCarbon;
    int previousMoney;
    
    void Start(){
        previousMaxCarbon = LevelManager.LM.getMaxCarbon();
        previousMoney = LevelManager.LM.GetMoney();
    }

    void Update()
    {
        //Activate creative mode on G + Ctrl + Space
        //You must press space last
        if(Debug.isDebugBuild && Input.GetKey(KeyCode.G) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.Space)){
            creativeMode = !creativeMode;
            previousMaxCarbon = LevelManager.LM.getMaxCarbon();
            previousMoney = LevelManager.LM.GetMoney();
        }
        if(creativeMode){
            LevelManager.LM.SetMoney(9999999);
            LevelManager.LM.setMaxCarbon(999999);
            LevelManager.LM.AdjustCarbon(-9999);
            wasJustInCreativeMode = true;
            if(Input.GetKeyDown(KeyCode.P)){
                int[] progressionEventsToCall = new int[ProgressionManager.PM.progressEvents.Length];
                for(int i = 0; i < ProgressionManager.PM.progressEvents.Length; i++){
                    progressionEventsToCall[i] = i;
                }
                ProgressionManager.PM.CallProgressEvents(progressionEventsToCall);
            }
        }else if(wasJustInCreativeMode){
            wasJustInCreativeMode = false;
            LevelManager.LM.SetMoney(0);
            LevelManager.LM.setMaxCarbon((int)previousMaxCarbon);
        }
    }
}
