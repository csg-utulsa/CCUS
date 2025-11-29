using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreatheAnimation : MonoBehaviour
{
    public float breatheTimer = 0f;
    public float breatheSize = 1f;
    public float breatheSpeed = 1f;

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

        float sizingPercentage = 1 + (breatheSize * ( ( (Mathf.Cos((breatheTimer) * Mathf.PI * breatheSpeed)) * 0.5f ) + 0.5f ));

        transform.localScale = new Vector3(sizingPercentage * defaultSizing.x, sizingPercentage * defaultSizing.y, sizingPercentage * defaultSizing.z);
    }
}
