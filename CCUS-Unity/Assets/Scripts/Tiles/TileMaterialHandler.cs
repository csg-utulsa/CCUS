using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/***
 * 
 * Created By Aidan Pohl
 * Creares: 11/11/2023
 * 
 * Edited By: Aidan Pohl
 * Edited: 11/14/2023
 * 
 * Description: Handles the Matrial changes for draggable tiles and objects
 * 
 * NOTE: This requires the model to be a child of the object this is placed in and that the Materials of that model to be transparent
 *
 */ 
public class TileMaterialHandler : MonoBehaviour
{
    Renderer matRenderer;
    Material[] baseMaterials;
    Material[] hoveringMaterials;
    Material[] invalidMaterials;
<<<<<<< Updated upstream
=======
    public string debugState;

    public enum matState
    {
        Placed,
        HoveringValid,
        HoveringInvalid,
    }
    private matState currentState;

>>>>>>> Stashed changes
    // Start is called before the first frame update
    void Start()
    {   
        matRenderer = gameObject.GetComponentInChildren<Renderer>();
        
    }

    // Update is called once per frame
<<<<<<< Updated upstream

=======
    private void Update()
    {

    }
>>>>>>> Stashed changes

    public void MaterialSet(matState state)//sets the material to a certain colot
    {
        switch (state)
        {
            case matState.Placed:
                foreach (Material mat in matRenderer.materials)
                    mat.color = new Color(1f, 1f, 1f, 1f); break;//material is fully colored
            case matState.HoveringValid:
                foreach (Material mat in matRenderer.materials)
                    mat.color = new Color(1f, 1f, 1f, .5f); break;//material is 50% transparent
            case matState.HoveringInvalid:
                foreach (Material mat in matRenderer.materials)
                    mat.color = new Color(1f, .1f, .1f, .75f); break;//material is 50% transparent and also red
            default:
                foreach (Material mat in matRenderer.materials)
                    mat.color = new Color(1f, 1f, 1f, 1f); break;//material is fully colored

        }
    }
}
