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
 * Not completely functional.
 *
 */ 
public class TileMaterialHandler : MonoBehaviour
{
    Renderer matRenderer;
    List<Color> originalColors = new List<Color>();
    private float ditherTransparencyResolution = 1f;


    public enum matState
    {
        Placed,
        HoveringValid,
        HoveringInvalid,
        PartiallyTransparentPlaced
    }
    private matState currentState;



    void Awake()
    {   
        matRenderer = gameObject.GetComponentInChildren<Renderer>();

        foreach(Material mat in matRenderer.materials){
            if(mat.shader == Shader.Find("Standard")){
                Color color = mat.GetColor("_Color");
                originalColors.Add(color);
            }else if (mat.shader == Shader.Find("Shader Graphs/Dither Shader")){
                originalColors.Add(mat.GetColor("_BaseColor"));
            } else{
                originalColors.Add(new Color(1f, 1f, 1f, 1f));
            }
        }
        //originalColors = new Color[matRenderer.materials.Length];

        

        // foreach (Material mat in matRenderer.materials){
        //     SetToDitherTransparencyShader(mat);
        // }
        // int numOfMissingColors = matRenderer.materials.Length - originalColors.Count;
        // if(numOfMissingColors > 0){
        //     for(int i = originalColors.Count - 1; i < matRenderer.materials.Length; i++){
        //         originalColors[i] = new Color( .1f, .1f, .1f, 1f);
        //     }
        // }
        // Debug.Log("Original Colors: ");
        // foreach(Color _Color in originalColors){
        //     //Debug.Log(_Color);
        // }
        // Debug.Log("That's all the original colors!");
        
    }

    void Update(){

    }

    public void SetDitherTransparency(float transparency){
        for (int i = 0; i < matRenderer.materials.Length; i++){
            if(matRenderer.materials[i].shader == Shader.Find("Shader Graphs/Dither Shader")){
                matRenderer.materials[i].SetColor("_BaseColor", originalColors[i] * new Color(1f, 1f, 1f, transparency));
            }
        }
    }

    public float GetCurrentTransparency(){
        if(matRenderer.materials[0].shader == Shader.Find("Shader Graphs/Dither Shader")){
            return matRenderer.materials[0].GetColor("_BaseColor").a;
        } else if(matRenderer.materials[0].shader == Shader.Find("Standard")){
            return matRenderer.materials[0].color.a;
        }
        return 1f;
    }


    public void MaterialSet(matState state)//sets the material to a certain color
    {
        for (int i = 0; i < matRenderer.materials.Length; i++){
            if(matRenderer.materials[i].shader == Shader.Find("Shader Graphs/Dither Shader")){
                switch (state){
                    case matState.Placed:
                        matRenderer.materials[i].SetColor("_BaseColor", originalColors[i]); break;//material is fully colored
                    case matState.HoveringValid:
                        matRenderer.materials[i].SetColor("_BaseColor", originalColors[i] * new Color(1f, 1f, 1f, 1f)); break; //Material is 50% transparent
                    case matState.HoveringInvalid:
                        matRenderer.materials[i].SetColor("_BaseColor", new Color(1f, .1f, .1f, 1f));  break;//material is 50% transparent and also red
                    case matState.PartiallyTransparentPlaced:
                        matRenderer.materials[i].SetColor("_BaseColor", originalColors[i] * new Color(1f, 1f, 1f, 1f)); break; //Material is 50% visible
                    default:
                        matRenderer.materials[i].SetColor("_BaseColor", originalColors[i]); break;//material is fully colored
                }
            } else if(matRenderer.materials[i].shader == Shader.Find("Standard")){
                switch (state){
                    case matState.Placed:
                        matRenderer.materials[i].color = originalColors[i]; break;//material is fully colored
                    case matState.HoveringValid:
                        matRenderer.materials[i].color = originalColors[i] * new Color(1f, 1f, 1f, 1f); break; //Material is 50% transparent
                    case matState.HoveringInvalid:
                        matRenderer.materials[i].color = new Color(1f, .1f, .1f, 1f); break;//material is 50% transparent and also red
                    case matState.PartiallyTransparentPlaced:
                        matRenderer.materials[i].color = originalColors[i] * new Color(1f, 1f, 1f, 1f); break; //Material is 25% visible
                    default:
                        matRenderer.materials[i].color = originalColors[i]; break;//material is fully colored
                }
            }
        }



        return;
        
        // switch (state)
        // {
        //     //This is the old code:
        //     // case matState.Placed:
        //     //     foreach (Material mat in matRenderer.materials)
        //     //         mat.color = new Color(1f, 1f, 1f, 1f); break;//material is fully colored
        //     // case matState.HoveringValid:
        //     //     foreach (Material mat in matRenderer.materials)
        //     //         mat.color = new Color(1f, 1f, 1f, .5f); break;//material is 50% transparent
        //     // case matState.HoveringInvalid:
        //     //     foreach (Material mat in matRenderer.materials)
        //     //         mat.color = new Color(1f, .1f, .1f, .75f); break;//material is 50% transparent and also red
        //     // default:
        //     //     foreach (Material mat in matRenderer.materials)
        //     //         mat.color = new Color(1f, 1f, 1f, 1f); break;//material is fully colored


        //     //New code that allows materials to retain their original color:
        //     case matState.Placed:
        //         for(int i = 0; i < matRenderer.materials.Length; i++)
        //             matRenderer.materials[i].color = originalColors[i]; break;//material is fully colored
        //     case matState.HoveringValid:
        //         for(int i = 0; i < matRenderer.materials.Length; i++)
        //             matRenderer.materials[i].color = originalColors[i] * new Color(1f, 1f, 1f, 0.5f); break; //Material is 50% transparent
        //     case matState.HoveringInvalid:
        //         foreach (Material mat in matRenderer.materials)
        //             mat.color = new Color(1f, .1f, .1f, .75f); break;//material is 50% transparent and also red
        //     case matState.PartiallyTransparentPlaced:
        //         Debug.Log("Number of Materials: " + matRenderer.materials.Length);
                
        //         for(int i = 0; i < matRenderer.materials.Length; i++)
        //             matRenderer.materials[i].color = originalColors[i] * new Color(1f, 1f, 1f, 0.5f);
        //         break; //Material is 25% visible
        //     default:
        //         for(int i = 0; i < matRenderer.materials.Length; i++)
        //             matRenderer.materials[i].color = originalColors[i]; break;//material is fully colored

        // }
    }

    // public void SetToDitherTransparencyShader(Material mat){
    //     if(mat.shader == Shader.Find("Standard")){
    //         //Stores this shader's properties
    //         float metallic = mat.GetFloat("_Metallic");
    //         float smoothness = mat.GetFloat("_Glossiness");
    //         Color color = mat.GetColor("_Color");
    //         originalColors.Add(color);

            
    //         //Switches to the Dither Transparency Shader
    //         mat.shader = Shader.Find("Shader Graphs/Dither Shader");
    //         mat.SetFloat("_Metallic", metallic);
    //         mat.SetFloat("_Smoothness", smoothness);
    //         mat.SetColor("_Base Color", color);
    //         mat.SetFloat("_Dither_Size", ditherTransparencyResolution);

    //     }else if (mat.shader == Shader.Find("Shader Graphs/Dither Shader")){
    //         originalColors.Add(mat.GetColor("_BaseColor"));
    //     }
    // }

}