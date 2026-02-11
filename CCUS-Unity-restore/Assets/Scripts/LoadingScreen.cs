using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LoadingScreen : MonoBehaviour
{
    public Image loadingRing;
    public GameObject darkBackground;
    public Image darkBGImage;

    
    private float startFillAmount = 0f;
    private float targetFillAmount = 1f;

    public float timeToFill = 10f;
    private float currentTime = 0f;

    public bool isFilling = true;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        darkBGImage = darkBackground.GetComponent<Image>();
        loadingRing.fillAmount = startFillAmount;
        darkBackground.SetActive(true);
        loadingRing.gameObject.SetActive(true);
    }

    private bool hasChangedSpeed = false;


    // Update is called once per frame
    void Update()
    {
        if(isFilling){
            currentTime += Time.deltaTime;
            if(currentTime > 2f && !hasChangedSpeed){
                hasChangedSpeed = true;
                timeToFill = 6f;
            }
            if(currentTime > timeToFill){
                isFilling = false;
                StartCoroutine(WaitToHideGraphic());
            } else{
                float percentFilled = (currentTime / timeToFill) * targetFillAmount;
                loadingRing.fillAmount = percentFilled;
            }
        }
        
    }

    public IEnumerator WaitToHideGraphic(){
        yield return new WaitForSeconds(1.8f);
        this.gameObject.SetActive(false);
    }
}
