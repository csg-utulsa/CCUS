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
    Renderer renderer;
    Material[] baseMaterials;
    Material[] hoveringMaterials;
    Material[] invalidMaterials;
    // Start is called before the first frame update
    void Start()
    {   
        renderer = gameObject.GetComponentInChildren<Renderer>();
        //    //GetComponent<Renderer>();
        //baseMaterials = renderer.materials;
        //hoveringMaterials = renderer.materials;
        //invalidMaterials = renderer.materials;



        //foreach (Material mat in baseMaterials)
        //{
        //    mat.color = new Color(1f, .5f, .5f, .5f);
        //}
    }

    // Update is called once per frame


    public void MaterialSet(string set)//sets the material to a certain colot
    {
        switch (set)
        {
            case "placed":
                foreach (Material mat in renderer.materials)
                    mat.color = new Color(1f, 1f, 1f, 1f); break;//material is fully colored
            case "hovering":
                foreach (Material mat in renderer.materials)
                    mat.color = new Color(1f, 1f, 1f, .5f); break;//material is 50% transparent
            case "invalid":
                foreach (Material mat in renderer.materials)
                    mat.color = new Color(1f, .1f, .1f, .75f); break;//material is 50% transparent and also red
            default:
                foreach (Material mat in renderer.materials)
                    mat.color = new Color(1f, 1f, 1f, 1f); break;//material is fully colored

        }
    }
}
