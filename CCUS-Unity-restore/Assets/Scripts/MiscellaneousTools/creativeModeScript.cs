/*





*   This script allows developers to cheat at the game. 
*   In order to activate it, first type the password. Currently, it's "milton"
*   
*   After you type the password, you can use these cheats:
*   Tab + G -> enter creative mode: Infinite money, no carbon. Tab + G again to exit this mode
*   Tab + M -> give yourself $10,000.
*   Tab + O -> Unlock all progress events
*
*
*/

using UnityEngine;

public class creativeModeScript : MonoBehaviour
{

    public bool allowCheats = true;
    public bool disableCheatsInBuild = false;

    bool creativeMode = false;
    bool wasJustInCreativeMode = false;
    float previousMaxCarbon;
    int previousMoney;
    public GridVisualizer gridVisualizer;

    public static creativeModeScript current;

    void Awake(){
        if(current == null){
            current = this;
        } else{
            Destroy(this);
        }
    }
    
    void Start(){
        previousMaxCarbon = LevelManager.LM.getMaxCarbon();
        previousMoney = LevelManager.LM.GetMoney();
    }

    //Janky Method for trying to get the password from the player
    private int furthestLetterGotten = 0;
    [SerializeField]
    private string cheatPassword = "milton";
    private bool cheatsEnabled = false;
    private bool TryToEnableCheats(){
        if(furthestLetterGotten >= cheatPassword.Length - 1){
            
            return true;
        }
        if(Input.GetKey(KeyCode.Parse<KeyCode>(char.ToUpper(cheatPassword[furthestLetterGotten]).ToString()))){
            furthestLetterGotten++;
        } else{
            furthestLetterGotten = 0;
        }
        return false;
    }

    void Update()
    {   

        //Disables creative mode
        if(!allowCheats){
            return;
        }

        if(disableCheatsInBuild){
            #if DEVELOPMENT_BUILD
                return;
            #endif
            
        }

        //Janky system for trying to get the password from the player
        if(!cheatsEnabled){
            if(Input.anyKeyDown){
                if(TryToEnableCheats()){
                    cheatsEnabled = true;
                    Debug.Log("Cheats enabled!!!");
                    GameEventManager.current.GetEvent(EventType.E.CheatsEnabled).Invoke();
                }
            }
            return;
        }
     
        
        //Activate creative mode on Tab + G
        //You must press G Last
        //Prevents it from running in build if included: Debug.isDebugBuild && 
        if(Input.GetKey(KeyCode.Tab) && Input.GetKeyDown(KeyCode.G)){
            if(!creativeMode){
                previousMaxCarbon = LevelManager.LM.getMaxCarbon();
                previousMoney = LevelManager.LM.GetMoney();
            }
            creativeMode = !creativeMode;
            
            
        }

        //Gives you $10,000 when you hold tab and press M
        if(Input.GetKey(KeyCode.Tab) && Input.GetKeyDown(KeyCode.M)){
            LevelManager.LM.SetMoney(LevelManager.LM.GetMoney() + 10000);
        }
        
        //Unlock all progress events
        if(Input.GetKey(KeyCode.Tab) && Input.GetKeyDown(KeyCode.O)){
            int[] progressionEventsToCall = new int[ProgressionManager.PM.progressEvents.Length];
            for(int i = 0; i < ProgressionManager.PM.progressEvents.Length; i++){
                progressionEventsToCall[i] = i;
            }
            ProgressionManager.PM.CallProgressEvents(progressionEventsToCall);
        }

        //Increases the max number of ground areas by 1
        if(Input.GetKey(KeyCode.Tab) && Input.GetKeyDown(KeyCode.A)){
            if(GroundAreaExpansion.current.NumberOfGroundChunks == GroundAreaExpansion.current.MaxNumberOfChunks){
                GroundAreaExpansion.current.MaxNumberOfChunks++;
                GroundAreaExpansion.current.AddGroundChunk();
            }else{
                GroundAreaExpansion.current.MaxNumberOfChunks++;
            }
            
        }

        //Spawn cubes in each grid square (Bugged)
        if(Input.GetKey(KeyCode.Tab) && Input.GetKeyDown(KeyCode.T)){
            gridVisualizer.ActivateGridVisualization();
        }

        if(creativeMode){
            LevelManager.LM.SetMoney(9999999);
            LevelManager.LM.setMaxCarbon(999999);
            LevelManager.LM.SetCarbon(0);
            wasJustInCreativeMode = true;
            
        }else if(wasJustInCreativeMode){
            wasJustInCreativeMode = false;
            LevelManager.LM.SetMoney(previousMoney);
            LevelManager.LM.setMaxCarbon((int)previousMaxCarbon);
        }
    }
}





