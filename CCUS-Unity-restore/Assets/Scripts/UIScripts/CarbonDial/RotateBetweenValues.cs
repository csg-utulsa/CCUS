using UnityEngine;
using UnityEngine.UI;

public class RotateBetweenValues : MonoBehaviour
{
    public bool rotationIsFlipped = true;
    public float minRotation = 58f;
    public float maxRotation = 302f;
    public float minRotationCap = 58f;
    public float maxRotationCap = 302f;
    private RectTransform rectTransform;

    void Start(){
        if(GetComponent<RectTransform>() != null){
            rectTransform = GetComponent<RectTransform>();
        }
    }

    public void SetRotationAsPercent(float percentRotation){

        if(rotationIsFlipped){
            SetRectTransformRotation(maxRotation - (percentRotation * (maxRotation - minRotation)));
        }else{
            SetRectTransformRotation(minRotation + (percentRotation * (maxRotation - minRotation)));
        }
    }

    private void SetRectTransformRotation(float rotation){
        
        
        //Caps the rotation between two cap values
        float newRotation;
        if(rotation > maxRotationCap){
            newRotation = maxRotationCap;
        }else if (rotation < minRotationCap){
            newRotation = minRotationCap;
        }else{
            newRotation = rotation;
        }

        if(rectTransform != null){
            rectTransform.rotation = Quaternion.Euler(0f, 0f, newRotation);
        }
    }
}
