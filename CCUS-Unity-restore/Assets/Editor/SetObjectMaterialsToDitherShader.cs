using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;
using System.IO;
using System;
using System.Linq;
using UnityEngine.UI;
using UnityEditor.Events;

public class SetObjectMaterialsToDitherShader : EditorWindow
{
    public GameObject ObjectToChangeShader;

    [MenuItem("Tools/Set Object Material Shaders")]
    public static void SetShaders()
    {
        SetObjectMaterialsToDitherShader wnd = GetWindow<SetObjectMaterialsToDitherShader>();
        //wnd.myWindow = wnd;
        wnd.minSize = new Vector2(600,600);
        //GetWindow<TileFactoryEditor>("tileFactoryEditor");
    }

    void OnGUI(){

        //Set style for text
        GUIStyle textStyle = new GUIStyle();
        textStyle.fontSize = 18;
        textStyle.normal.textColor = Color.white;

        //Set style for Button
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = 25;
        buttonStyle.normal.textColor = Color.white;
        buttonStyle.fontStyle = FontStyle.Bold;

        GUILayout.Label("Switch Object Material Shaders", textStyle, GUILayout.Width(150));

        ObjectToChangeShader = (GameObject)EditorGUILayout.ObjectField("Game Object: ", ObjectToChangeShader, typeof(GameObject), false);

        if (GUILayout.Button("Set to Dither Shader", buttonStyle)){
            Renderer matRenderer = ObjectToChangeShader.GetComponent<Renderer>();
            if(matRenderer != null){
                foreach (Material mat in matRenderer.sharedMaterials){
                    SetToDitherTransparencyShader(mat);
                }
            }
            this.Close();
        }

        if(GUILayout.Button("Set to Standard Shader", buttonStyle)){
            Renderer matRenderer = ObjectToChangeShader.GetComponent<Renderer>();
            if(matRenderer != null){
                foreach (Material mat in matRenderer.sharedMaterials){
                    SetToStandardShader(mat);
                }
            }
            this.Close();
        }
    }

    public void SetToDitherTransparencyShader(Material mat){
        if(mat.shader == Shader.Find("Standard")){
            //Stores this shader's properties
            float metallic = mat.GetFloat("_Metallic");
            float smoothness = mat.GetFloat("_Glossiness");
            Color color = mat.GetColor("_Color");
            Texture mainTex = mat.GetTexture("_MainTex");

            
            //Switches to the Dither Transparency Shader
            mat.shader = Shader.Find("Shader Graphs/Dither Shader");
            mat.SetFloat("_Metallic", metallic);
            mat.SetFloat("_Smoothness", smoothness);
            mat.SetColor("_BaseColor", color);
            mat.SetTexture("_Base_Texture", mainTex);
            //mat.SetFloat("_Dither_Size", ditherTransparencyResolution);

        }
    }

    public void SetToStandardShader(Material mat){
        if(mat.shader == Shader.Find("Shader Graphs/Dither Shader")){
            //Stores this shader's properties
            float metallic =  mat.GetFloat("_Metallic");
            float smoothness =  mat.GetFloat("_Smoothness");
            Color color =  mat.GetColor("_BaseColor");
            Texture mainTex = mat.GetTexture("_Base_Texture");

            
            //Switches to the Dither Transparency Shader
            mat.shader = Shader.Find("Standard");
            mat.SetFloat("_Metallic", metallic);
            mat.SetFloat("_Glossiness", smoothness);
            mat.SetColor("_Color", color);
            mat.SetTexture("_MainTex", mainTex);
            //mat.SetFloat("_Dither_Size", ditherTransparencyResolution);

        }
    }
}
