using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreatheAnimation : MonoBehaviour
{
    public float breatheTimer = 0f;
    public float breatheSize = 1f;
    public float breatheSpeed = 1f;

    public bool resizeVertical = true;
    public bool resizeHorizontal = true;

    public Vector3 defaultSizing = new Vector3(0f, 0f, 0f);

    void Start(){
        defaultSizing = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        breatheTimer += Time.deltaTime;

        if(breatheTimer > 5f * (2 / breatheSpeed) ){
            breatheTimer = 0f;
        }

        //Prevents resizing on each axis if it's disabled
        float verticalResizing = (resizeVertical)? 1f : 0f;
        float horizontalResizing = (resizeHorizontal)? 1f : 0f;

        float sizingPercentageHorizontal = 1 + (horizontalResizing * (breatheSize * ( ( (Mathf.Cos((breatheTimer) * Mathf.PI * breatheSpeed)) * 0.5f ) + 0.5f )));
        float sizingPercentageVertical = 1 + (verticalResizing * (breatheSize * ( ( (Mathf.Cos((breatheTimer) * Mathf.PI * breatheSpeed)) * 0.5f ) + 0.5f )));

        transform.localScale = new Vector3(sizingPercentageHorizontal * defaultSizing.x, sizingPercentageVertical * defaultSizing.y, defaultSizing.z);
    }
}
