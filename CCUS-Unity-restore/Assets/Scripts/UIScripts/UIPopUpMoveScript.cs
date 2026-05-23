using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopUpMoveScript : MonoBehaviour
{
    public float timeTillDestruction = .5f;
    public float speedOfMovement = 5f;
    private float timerCounter = 0f;

    void Awake(){
        
    }

    // Update is called once per frame
    void Update()
    {
        timerCounter += Time.deltaTime;
        if(timerCounter > timeTillDestruction){
            Destroy(this.gameObject);
        }
        transform.position += new Vector3(0f, speedOfMovement * Time.deltaTime, 0f);
    }
}
