using UnityEngine;

public class SpinObject : MonoBehaviour
{
    public bool IsSpinning {get; set;} = true;
    float originalRotation;
    public float speedToSpin = 200f;
    float myRotation = 0f;

    void Start(){
        originalRotation = transform.eulerAngles.z;
    }
    // Update is called once per frame
    void Update()
    {
        if(!IsSpinning) return;

        myRotation += speedToSpin * Time.deltaTime;
        if(myRotation >= 360f){
            myRotation = 0f;
        }

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, myRotation);
    }
}
