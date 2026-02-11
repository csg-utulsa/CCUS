using UnityEngine;

public class creativeModeScript : MonoBehaviour
{

    bool creativeMode = false;
    bool wasJustInCreativeMode = false;
    float previousMaxCarbon;
    int previousMoney;
    public GridVisualizer gridVisualizer;
    
    void Start(){
        previousMaxCarbon = LevelManager.LM.getMaxCarbon();
        previousMoney = LevelManager.LM.GetMoney();
    }

    void Update()
    {
        //Activate creative mode on Tab + G
        //You must press G Last
        //Prevents it from running in build if included: Debug.isDebugBuild && 
        if(Input.GetKey(KeyCode.Tab) && Input.GetKeyDown(KeyCode.G)){
            creativeMode = !creativeMode;
            previousMaxCarbon = LevelManager.LM.getMaxCarbon();
            previousMoney = LevelManager.LM.GetMoney();
        }
        if(creativeMode){
            LevelManager.LM.SetMoney(9999999);
            LevelManager.LM.setMaxCarbon(999999);
            LevelManager.LM.AdjustCarbon(-9999);
            wasJustInCreativeMode = true;
            if(Input.GetKeyDown(KeyCode.O)){
                int[] progressionEventsToCall = new int[ProgressionManager.PM.progressEvents.Length];
                for(int i = 0; i < ProgressionManager.PM.progressEvents.Length; i++){
                    progressionEventsToCall[i] = i;
                }
                ProgressionManager.PM.CallProgressEvents(progressionEventsToCall);
            }
            if(Input.GetKeyDown(KeyCode.T)){
                gridVisualizer.ActivateGridVisualization();
            }
        }else if(wasJustInCreativeMode){
            wasJustInCreativeMode = false;
            LevelManager.LM.SetMoney(0);
            LevelManager.LM.setMaxCarbon((int)previousMaxCarbon);
        }
    }
}





