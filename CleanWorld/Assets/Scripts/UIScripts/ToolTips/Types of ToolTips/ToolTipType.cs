using UnityEngine;
using TMPro;

public class ToolTipType : MonoBehaviour
{
    public TextMeshProUGUI textObject;
    public string textBefore = "";
    public string textAfter = "";

    //Defined in child classes to check if the tool tip should be enabled
    public virtual bool ShouldEnableToolTip(Tile tile){
        return false;
    }

    //Enables tool tip
    public virtual void EnableToolTip(Tile tile){
        if(this.gameObject != null){
            this.gameObject.SetActive(true);
        }
    }

    //Disables tool tip
    public virtual void DisableToolTip(){
        if(this.gameObject != null){
            this.gameObject.SetActive(false);
        }
    }

    protected virtual void SetTipText(string newText){
        if(textObject != null){
            textObject.text = textBefore + newText + textAfter;
        }
    }


}
