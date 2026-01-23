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
 * Edited Again By: Graydon B.
 * Edited: 1/10/2026
 * 
 * Description: Handles the Matrial changes for draggable tiles and objects
 * 
 * NOTE: This requires the model to be a child of the object this is placed in and that the Materials of that model to be transparent
 * Not completely functional.
 *
 * Later Note from Graydon: To achieve a transparency effect that didn't become increasingly worse with as a model's geometry became
 * more complex, it now uses a dither transparency shader for transparency.
 *
 */ 
public class TileMaterialHandler : MonoBehaviour
{
    //Stores all of the renderers for each model in the tile's children
    Renderer[] matRenderers;

    //stores the original colors that all the matRenderers are set to
    Color[][] originalColorsForEachRenderer;

    //Changes the resolution of the dither transparency shader (Leave at 1)
    //Note: The dither transparency looks weird in the Unity editor, but is correct in the build versions.
    //You can probably get an approximation of what it will look like in the editor by using the Scale option
    //in the Game window and setting it to a 1x scale. That will make it a 1:1 pixel ratio.
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
        //Stores all the Renderers in the Tile's children
        matRenderers = gameObject.GetComponentsInChildren<Renderer>();

        //A 2D array to store the original material colors of each renderer's materials
        originalColorsForEachRenderer = new Color[matRenderers.Length][];

        bool modelHasNonDitherShader = false;

        //Stores the original colors for each renderer's list of materials in the array originalColorsForEachRenderer
        for(int ir = 0; ir < matRenderers.Length; ir++){
            originalColorsForEachRenderer[ir] = new Color[matRenderers[ir].materials.Length];
            for(int ic = 0; ic < matRenderers[ir].materials.Length; ic++){
                //Stores color for materials using the standard Shader
                if(matRenderers[ir].materials[ic].shader == Shader.Find("Standard")){
                    Color color = matRenderers[ir].materials[ic].GetColor("_Color");
                    originalColorsForEachRenderer[ir][ic] = color;
                    modelHasNonDitherShader = true;
                }
                //Stores colors for materials using dither shader
                else if (matRenderers[ir].materials[ic].shader == Shader.Find("Shader Graphs/Dither Shader")){
                    originalColorsForEachRenderer[ir][ic] = matRenderers[ir].materials[ic].GetColor("_BaseColor");
                } else{
                    originalColorsForEachRenderer[ir][ic] = new Color(1f, 1f, 1f, 1f);
                    modelHasNonDitherShader = true;
                }
            }  
        }
        //Debug.Log("Number Of Mesh Renderers: " + matRenderers.Length);
        
        if(modelHasNonDitherShader){
            Debug.LogError("One of the models you're using has a standard transparency shader, rather than a Dither Shader. That will prevent it from being made transparent.");
            Debug.LogError("To assign the Dither Transparency Shader to your model, use the tool found in the Unity menu under: Tools -> Set Object Material Shaders");
        }

        

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


    //Sets transparency for all renderers in children
    public void SetDitherTransparency(float transparency){
        for(int i = 0; i < matRenderers.Length; i++){
            SetDitherTransparencyForMaterialRenderer(transparency, matRenderers[i], originalColorsForEachRenderer[i]);
        }
    }

    public void SetDitherTransparencyForMaterialRenderer(float transparency, Renderer matRenderer, Color[] originalColors){
        
        for (int i = 0; i < matRenderer.materials.Length; i++){
            if(matRenderer.materials[i].shader == Shader.Find("Shader Graphs/Dither Shader")){
                matRenderer.materials[i].SetColor("_BaseColor", originalColors[i] * new Color(1f, 1f, 1f, transparency));
            }
        }
    }

    //Returns transparency for first Renderer in children
    public float GetCurrentTransparency(){
        return GetCurrentTransparencyForMaterialRenderer(matRenderers[0]);
    }

    public float GetCurrentTransparencyForMaterialRenderer(Renderer matRenderer){
        if(matRenderer.materials[0].shader == Shader.Find("Shader Graphs/Dither Shader")){
            return matRenderer.materials[0].GetColor("_BaseColor").a;
        } else if(matRenderer.materials[0].shader == Shader.Find("Standard")){
            return matRenderer.materials[0].color.a;
        }
        return 1f;
    }

    //Sets material for each mat renderer component in children
    public void MaterialSet(matState state){
        //Debug.Log("Setting material for " + matRenderers.Length + "Renderers");
        for(int i = 0; i < matRenderers.Length; i++){
            MaterialSetForMaterialRenderer(state, matRenderers[i], originalColorsForEachRenderer[i]);
        }
    }

    //Sets all materials to a certain color
    public void MaterialSetForMaterialRenderer(matState state, Renderer matRenderer, Color[] originalColors)//sets the material to a certain color
    {
        for (int i = 0; i < matRenderer.materials.Length; i++){
            //Sets color for dither shader
            if(matRenderer.materials[i].shader == Shader.Find("Shader Graphs/Dither Shader")){
                switch (state){
                    case matState.Placed:
                        matRenderer.materials[i].SetColor("_BaseColor", originalColors[i]); break;//material is fully colored
                    case matState.HoveringValid:
                        matRenderer.materials[i].SetColor("_BaseColor", originalColors[i] * new Color(1.2f, 1.2f, 1.2f, 1f)); break; //Material shifts slightly green
                    case matState.HoveringInvalid:
                        matRenderer.materials[i].SetColor("_BaseColor", new Color(1f, .1f, .1f, 1f));  break;//material is 50% transparent and also red
                    case matState.PartiallyTransparentPlaced:
                        matRenderer.materials[i].SetColor("_BaseColor", originalColors[i] * new Color(1f, 1f, 1f, 1f)); break; //Material is 50% visible
                    default:
                        matRenderer.materials[i].SetColor("_BaseColor", originalColors[i]); break;//material is fully colored
                }

                //Sets color for standard shader
            } else if(matRenderer.materials[i].shader == Shader.Find("Standard")){
                switch (state){
                    case matState.Placed:
                        matRenderer.materials[i].color = originalColors[i]; break;//material is fully colored
                    case matState.HoveringValid:
                        matRenderer.materials[i].color = originalColors[i] * new Color(.1f, 1f, .3f, 1f); break; //Material shifts slightly green
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