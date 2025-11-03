using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.IO;
using System;

public class deleteTileButton : EditorWindow
{
    //Called Every Time the user moves their mouse or clicks over the open window

    public deleteTileButton myWindow;

    public GameObject tileToDelete;

    private bool closeWindow = false;
    

    void OnGUI()
    {
        //Set style for Delete Button
            GUIStyle deleteButtonStyle = new GUIStyle(GUI.skin.button);
            deleteButtonStyle.fontSize = 20;
            deleteButtonStyle.normal.textColor = Color.red;
            deleteButtonStyle.fontStyle = FontStyle.Bold;

        //Set style for Warning Subtext
            GUIStyle warningSubtextStyle = new GUIStyle();
            warningSubtextStyle.fontSize = 12;
            warningSubtextStyle.normal.textColor = Color.red;
            warningSubtextStyle.fontStyle = FontStyle.Bold;

         //Set style for Warning Title
            GUIStyle warningTitleStyle = new GUIStyle();
            warningTitleStyle.fontSize = 30;
            warningTitleStyle.normal.textColor = Color.red;
            warningTitleStyle.fontStyle = FontStyle.Bold;
        GUILayout.BeginArea(new Rect(50, 140, 500, 1000));
        GUILayout.Label("DELETE THIS TILE?", warningTitleStyle);
        GUILayout.Label("");
        GUILayout.Label("If this tile is included in the game, deleting it could", warningSubtextStyle);
        GUILayout.Label("cause some weird stuff to happen that's not easy to fix", warningSubtextStyle);
        GUILayout.Label("");
        GUILayout.Label("");
        GUILayout.Label("");
        if (GUILayout.Button("DELETE THIS TILE", deleteButtonStyle)){

            //Delete Scriptable Object
            try{
                string pathOfScriptableObjectToDelete = AssetDatabase.GetAssetPath(tileToDelete.GetComponent<Tile>().tileScriptableObject);
                bool deleted = AssetDatabase.DeleteAsset(pathOfScriptableObjectToDelete);
                if (!deleted) Debug.LogError("Couldn't delete Scriptable Object! (This error will also appear if there wasn't a scriptable object at all.)");
            } catch (Exception ex){
                Debug.LogError(ex);
            }

            //Delete Prefab
            try{
                string pathOfTileToDelete = AssetDatabase.GetAssetPath(tileToDelete);
                bool deleted = AssetDatabase.DeleteAsset(pathOfTileToDelete);
                if (!deleted) Debug.LogError("Couldn't delete Prefab!");

            } catch (Exception ex){
                Debug.LogError(ex);
            }
            
            closeWindow = true;
            

            
        }
        GUILayout.EndArea();
        if(closeWindow){
            this.Close();
        }
    }

}
