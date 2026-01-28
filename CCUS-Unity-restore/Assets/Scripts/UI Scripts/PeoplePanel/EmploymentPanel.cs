//This script activates the employee panel when factories are unlocked

using UnityEngine;



public class EmploymentPanel : MonoBehaviour
{

    public bool EmploymentPanelActivated{get; set;} = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameEventManager.current.ProgressEventJustCalled.AddListener(ProgressEventCalled);
    }

    //Checks if factories have been unlocked. If so, it activates the employment panel.
    private void ProgressEventCalled(){

        //If the employment panel has already been activated, it doesn't bother checking to activate it again
        if(EmploymentPanelActivated) return;

        //If factories have been unlocked
        if(ProgressionManager.PM.progressEventHasOccurred[(int)ProgressionManager.ProgressEventType.FactoriesUnlocked]){
            ActivateEmploymentPanel();
        }
    }

    //Activates the employment panel
    private void ActivateEmploymentPanel(){

        UnhideUIElement unhideEmploymentPanel = GetComponent<UnhideUIElement>();

        if(unhideEmploymentPanel != null){
            Debug.Log("Activating people panel");
            unhideEmploymentPanel.ActivateUIElement();
            EmploymentPanelActivated = true;
        }
    }
}
