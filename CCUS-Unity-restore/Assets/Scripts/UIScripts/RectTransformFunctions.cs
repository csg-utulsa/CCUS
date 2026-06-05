using UnityEngine;

public class RectTransformFunctions : MonoBehaviour
{
    public static RectTransformFunctions current;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(current == null){
            current = this;
        }else{
            Destroy(this);
        }
    }

    //Set RectTransform value functions
    public void SetBottom(RectTransform rectTransform, float newPosition){
        float adjustedPosition = newPosition + (0.5f * rectTransform.rect.height);
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, adjustedPosition);
    }

    public void SetTop(RectTransform rectTransform, float newPosition){
        float adjustedPosition = newPosition - (0.5f * rectTransform.rect.height);
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, adjustedPosition);
    }

    public void SetLeft(RectTransform rectTransform, float newPosition){
        float adjustedPosition = newPosition + (0.5f * rectTransform.rect.width);
        rectTransform.anchoredPosition = new Vector2(adjustedPosition, rectTransform.anchoredPosition.y);
    }

    public void SetRight(RectTransform rectTransform, float newPosition){
        float adjustedPosition = newPosition - (0.5f * rectTransform.rect.width);
        rectTransform.anchoredPosition = new Vector2(adjustedPosition, rectTransform.anchoredPosition.y);
    }

    //Get RectTransform value functions
    public float GetBottom(RectTransform rectTransform){
        return(rectTransform.anchoredPosition.y - (0.5f * rectTransform.rect.height));
    }

    public float GetTop(RectTransform rectTransform){
        return(rectTransform.anchoredPosition.y + (0.5f * rectTransform.rect.height));
    }

    public float GetLeft(RectTransform rectTransform){
        return(rectTransform.anchoredPosition.x - (0.5f * rectTransform.rect.width));
    }

    public float GetRight(RectTransform rectTransform){
        return(rectTransform.anchoredPosition.x + (0.5f * rectTransform.rect.width));
    }

    public Rect RectTransformToScreenSpace(RectTransform inputRect)
    {
        Vector2 size = Vector2.Scale(inputRect.rect.size, inputRect.lossyScale);
        return new Rect((Vector2)inputRect.position - (size * 0.5f), size);
    }



    //Static functions
    public static void StaticSetLeft(RectTransform rectTransform, float newPosition){
        float adjustedPosition = newPosition + (0.5f * rectTransform.rect.width);
        rectTransform.anchoredPosition = new Vector2(adjustedPosition, rectTransform.anchoredPosition.y);
    }

    public static void StaticSetRight(RectTransform rectTransform, float newPosition){
        float adjustedPosition = newPosition - (0.5f * rectTransform.rect.width);
        rectTransform.anchoredPosition = new Vector2(adjustedPosition, rectTransform.anchoredPosition.y);
    }

    public static float StaticGetLeft(RectTransform rectTransform){
        return(rectTransform.anchoredPosition.x - (0.5f * rectTransform.rect.width));
    }

    public static float StaticGetRight(RectTransform rectTransform){
        return(rectTransform.anchoredPosition.x + (0.5f * rectTransform.rect.width));
    }

}
