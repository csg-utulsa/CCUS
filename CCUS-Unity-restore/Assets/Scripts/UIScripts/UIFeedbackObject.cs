//Replace this script with an animation. It will be much less resource intensive.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFeedbackObject : MonoBehaviour
{
    //public string feedBackText = "Error";

    public float _movementSpeed = 180f;
    private float movementSpeed {
        get{
            return _movementSpeed * CanvasScalarFactor.CSF.GetScaleFactor();
        }
    }

    public float movementTime = .5f;

    public float fadeTime = .1f;

    private float movementTimer = 0f;

    private float fadeTimer = 0f;

    private bool startedFading = false;




    void Awake(){
        
    }

    // Update is called once per frame
    void Update()
    {
        movementTimer += Time.deltaTime;
        if(!startedFading && movementTimer > movementTime){
            startedFading = true;
            if(GetComponent<FadeGraphic>() != null){
                GetComponent<FadeGraphic>().beginFading();
            }
        }
        transform.position += new Vector3( 0f, Time.deltaTime * movementSpeed, 0f );

    }


}
