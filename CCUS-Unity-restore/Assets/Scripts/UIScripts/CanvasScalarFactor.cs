using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//When the resolution of the screen changes, this corrects the pixel measurements that certain UI Placements depend on
public class CanvasScalarFactor : MonoBehaviour
{
    public static CanvasScalarFactor CSF;

    public float correctHeight;

    private float scaleFactor;

    public TextMeshProUGUI myText;

    public Canvas myCanvas;

    void Awake()
    {
        if(CSF == null){
            CSF = this;
        }

        scaleFactor = Screen.height / correctHeight;//myCanvas.GetComponent<RectTransform>().rect.height / correctHeight;
        //Debug.Log("Height: " + myCanvas.GetComponent<RectTransform>().rect.height);
        //Debug.Log("Canvas: " + myCanvas.GetComponent<RectTransform>().rect.height);
        //Debug.Log("Correct: " + correctHeight);
        if(myText != null)
        myText.text = "My height: " + GetComponent<RectTransform>().rect.height + "\nCanvas height: " + myCanvas.GetComponent<RectTransform>().rect.height;

    }

    void Update(){
        scaleFactor = Screen.height / correctHeight;
        //Debug.Log("Canvas: " + myCanvas.GetComponent<RectTransform>().rect.height);
        //Debug.Log("Correct: " + correctHeight);
        //scaleFactor = myCanvas.GetComponent<RectTransform>().rect.height / correctHeight;
    }

    public float GetScaleFactor(){
        return scaleFactor;
    }

}
