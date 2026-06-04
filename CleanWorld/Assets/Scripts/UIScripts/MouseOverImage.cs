using UnityEngine;

public class MouseOverImage : MonoBehaviour
{   
    public bool IsMouseOverImage(){
        Vector2 mousePos = Input.mousePosition;
        if(GetComponent<RectTransform>() != null){
            return RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), mousePos);
        }
        return false;
    }


}
