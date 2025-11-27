using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/***
 * Made By; Aidan Pohl
 * Made: Nov 12,2023
 * 
 * Edited  By: N/A
 * Edited: N/A
 * 
 * Description: Handles variations in placable object models, such as model varients, rotation, and scale
 */
public class ObjectVariation : MonoBehaviour
{
    public Transform modelObjectTransform;
    
    public bool rotationLocked = false; //Determines if the object model needs a static rotation
    public bool scaleLocked = false; //determines if the object model has a static scale
    public bool squareRotation = false; //Determines if the object will always face a side.

    [SerializeField] private float scaleMin = .75f;
    [SerializeField] private float scaleMax = 1.25f;


    private Vector3 baseScale;
    private Quaternion baseRot;

    // Start is called before the first frame update
    void Start()
    {   
        if(modelObjectTransform == null)
        {
            modelObjectTransform = transform.GetChild(0).transform;
        }
        baseScale = modelObjectTransform.localScale;
        baseRot = modelObjectTransform.localRotation;

        if (!rotationLocked) { RandomizeRotation(); }
        if (!scaleLocked) { RandomizeScale(); }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RandomizeRotation()
    {
        float rotation;

        if (squareRotation) { rotation = Random.Range(0, 3) * 90; }//randomizes on 90 degree intervals
        else { rotation = Random.value * 360; }//randomizes at random angle

        modelObjectTransform.Rotate(Vector3.forward, rotation);

    }

    public void RandomizeScale()
    {
        modelObjectTransform.localScale = baseScale * Random.Range(scaleMin, scaleMax);
    }
}
