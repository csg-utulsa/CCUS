using UnityEngine;

public class TouchModeHandler : MonoBehaviour
{

    public static TouchModeHandler current;

    public bool IsInTouchMode {get; set;} = false;


    void Awake(){
        if(current == null){
            current = this;
        } else{
            Destroy(this);
        }
        
    }

    
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            if(Input.touchCount > 0){
                IsInTouchMode = true;
            } else{
                IsInTouchMode = false;
            }
        }
    }

}
