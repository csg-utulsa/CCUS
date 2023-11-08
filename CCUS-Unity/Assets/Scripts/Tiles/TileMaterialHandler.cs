using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    public void MaterialSet(string set)
    {
        switch (set)
        {
            case "placed":
                foreach (Material mat in renderer.materials)
                    mat.color = new Color(1f, 1f, 1f, 1f); break;
            case "hovering":
                foreach (Material mat in renderer.materials)
                    mat.color = new Color(1f, 1f, 1f, .5f); break;
            case "invalid":
                foreach (Material mat in renderer.materials)
                    mat.color = new Color(1f, .5f, .5f, .5f); break;
        }
    }
}
