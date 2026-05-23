using UnityEngine;
using UnityEngine.UI;

public class CarbonDialColor : MonoBehaviour
{
    //Put the top images in first
    [Header("Input images in descending depth order. (Top on top)")]
    public Image[] imagesToFadeThrough;

    //Sets the carbon dial's center color based on what percentage the carbon capacity is at
    public void SetColorAsPercent(float colorPercent){
        int topVisibleImage = (int)(colorPercent * imagesToFadeThrough.Length);
        float alphaOfTopImage = 1f-((colorPercent * imagesToFadeThrough.Length) - topVisibleImage);
        HideAllImagesLowerThan(topVisibleImage);
        if(topVisibleImage < imagesToFadeThrough.Length){
            SetImageOpacity(imagesToFadeThrough[topVisibleImage], alphaOfTopImage);
        }
    }

    //Hides all the images in imagesToFadeThrough with an index lower than the input
    private void HideAllImagesLowerThan(int index){
        for(int i = index - 1; i >= 0; i--){
            if(i < 0) return;
            if(imagesToFadeThrough[i] != null){
                SetImageOpacity(imagesToFadeThrough[i], 0f);
            }
        }
    }

    private void SetImageOpacity(Image imageToSet, float opacity){
        if(imageToSet != null){
            Color originalColor = imageToSet.color;
            imageToSet.color = new Color(originalColor.r, originalColor.g, originalColor.b, opacity);
        }
    }

}
