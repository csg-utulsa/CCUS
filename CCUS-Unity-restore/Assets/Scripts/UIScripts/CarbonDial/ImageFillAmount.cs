using UnityEngine;
using UnityEngine.UI;

public class ImageFillAmount : MonoBehaviour
{
    public Image image;
    public float minFill = 0.14f;
    public float maxFill = 0.87f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(GetComponent<Image>() != null){
            image = GetComponent<Image>();
        }
    }

    public void SetFillAsPercent(float percent){
        if(image != null){
            image.fillAmount = minFill + (percent * (maxFill - minFill));
        }
    }
}
