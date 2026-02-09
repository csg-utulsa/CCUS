using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerImage : MonoBehaviour
{
    
    public bool isEnabled = false;
    public GameObject imageOne;
    public GameObject imageTwo;
    public bool flickerImage = true;

    void Start(){
        if(flickerImage){
            StartCoroutine(UpdateImage()); 
        }
       
    }

    // Update is called once per frame
    public IEnumerator UpdateImage() {
        while(true){
            yield return new WaitForSeconds(.2f);
            if(isEnabled == true){
                imageOne.SetActive(false);
                imageTwo.SetActive(true);
                isEnabled = false;
            }else {
                imageOne.SetActive(true);
                imageTwo.SetActive(false);
                isEnabled = true;
            }
        }
    }
}
