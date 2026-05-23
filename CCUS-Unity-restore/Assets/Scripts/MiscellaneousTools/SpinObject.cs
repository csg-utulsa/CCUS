using UnityEngine;

public class SpinObject : MonoBehaviour
{
    public bool IsSpinning {get; set;} = true;
    public bool rotateOnXAxis = false;
    float originalRotation;
    public float speedToSpin = 200f;
    float myRotation = 0f;

    void Start(){
        if(rotateOnXAxis){
            originalRotation = transform.eulerAngles.x;
        }else{
            originalRotation = transform.eulerAngles.z;
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        if(!IsSpinning) return;

        // myRotation += speedToSpin * Time.deltaTime;
        // if(myRotation >= 360f){
        //     myRotation = 0f;
        // }

        if(rotateOnXAxis){
            transform.Rotate(speedToSpin * Time.deltaTime, 0.0f, 0.0f, Space.Self);
            //transform.eulerAngles = new Vector3(myRotation, transform.eulerAngles.y, transform.eulerAngles.z);
        }else{
            transform.Rotate( 0.0f, 0.0f, speedToSpin * Time.deltaTime, Space.Self);
            //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, myRotation);
        }
        
    }
}
