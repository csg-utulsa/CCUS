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

        //TODO: Delete this if statement later
        if(Input.GetKeyDown(KeyCode.Z)){
            LevelManager.LM.setMaxCarbon(500);
        }
        

        //Activate creative mode on Tab + G
        //You must press G Last
        //Prevents it from running in build if included: Debug.isDebugBuild && 
        if(Input.GetKey(KeyCode.Tab) && Input.GetKeyDown(KeyCode.G)){
            creativeMode = !creativeMode;
            previousMaxCarbon = LevelManager.LM.getMaxCarbon();
            previousMoney = LevelManager.LM.GetMoney();
        }

        //Gives you $10,000 when you hold tab and press M
        if(Input.GetKey(KeyCode.Tab) && Input.GetKeyDown(KeyCode.M)){
            LevelManager.LM.SetMoney(LevelManager.LM.GetMoney() + 10000);
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





