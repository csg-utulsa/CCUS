using UnityEngine;
using UnityEngine.UI;

public class FadeChildGraphicsToTransparency : MonoBehaviour
{
    [HideInInspector] public float timeToFade = .25f;
    public float startImageTransparency = 0f;
    private Image[] childObjectImages;
    private Image[] allObjectImages;
    private Color[] originalImageColors;


    private bool isFading;
    private float previousTransparency = 0f;
    private float currentTransparency = 0f;
    private float targetTransparency = 0f;
    private float timer;

    void Start(){
        //Gets all images of child objects
        childObjectImages = GetComponentsInChildren<Image>(true);

        //If this object has an image, it also stores the image on this object
        if(GetComponent<Image>() != null){
            allObjectImages = new Image[childObjectImages.Length + 1];
            allObjectImages[allObjectImages.Length - 1] = GetComponent<Image>();
            for(int i = 0; i < allObjectImages.Length - 1; i++){
                allObjectImages[i] = childObjectImages[i];
            }
        }else{
            allObjectImages = childObjectImages;
        }
        
        //Stores the original colors of each Image
        originalImageColors = new Color[allObjectImages.Length];
        for(int i = 0; i < allObjectImages.Length; i++){
            originalImageColors[i] = allObjectImages[i].color;
        }

        SetTransparencyOfAllImages(startImageTransparency);
    }

    void Update(){
        if(isFading){
            timer += Time.deltaTime;
            
            if(timer > timeToFade){
                //Images have finished fading
                isFading = false;
                SetTransparencyOfAllImages(targetTransparency);
                previousTransparency = targetTransparency;
                currentTransparency = targetTransparency;
                timer = 0f;
            }else{
                //Sets image transparency to correct point based on how far into the timeToFade the timer has gotten
                float percentageFaded = timer / timeToFade;
                currentTransparency = previousTransparency + (percentageFaded * (targetTransparency - previousTransparency));
                SetTransparencyOfAllImages(currentTransparency);
            }
        }
    }

    //public function called to begin the fade process
    public void FadeAllChildGraphicsToTransparency(float transparency){
        previousTransparency = currentTransparency;
        targetTransparency = transparency;
        timer = 0f;
        isFading = true;
    }

    //public function called to begin the fade process
    public void FadeAllChildGraphicsBetweenTransparencies(float startTransparency, float endTransparency){
        SetTransparencyOfAllImages(startTransparency);
        previousTransparency = startTransparency;
        targetTransparency = endTransparency;
        timer = 0f;
        isFading = true;
    }

    //Immediately sets the transparenct of the images to the input transparency
    private void SetTransparencyOfAllImages(float transparency){
        
        for(int i = 0; i < allObjectImages.Length; i++){
            if(allObjectImages[i] != null){
                allObjectImages[i].color = new Color(originalImageColors[i].r, originalImageColors[i].g, originalImageColors[i].b, transparency);
            }
        }
    }

}
