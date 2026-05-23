//This one is the tile editor
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine.UI;
using System.IO;

public class tilePrefabEditor : EditorWindow
{
    public TileScriptableObject[] tileScriptableObjects;
    public GameObject[] tilePrefabs;

    [MenuItem("Tools/Tile Editor")]
    public static void OpenTileEditor()
    {
        tilePrefabEditor wnd = GetWindow<tilePrefabEditor>();
        wnd.minSize = new Vector2(600,700);
        //GetWindow<TileFactoryEditor>("tileFactoryEditor");
    }

    void OnGUI(){
            //Set style for Main Title
            GUIStyle titleStyle = new GUIStyle();
            titleStyle.fontSize = 35;
            titleStyle.normal.textColor = Color.white;
            titleStyle.fontStyle = FontStyle.Bold;

            //Set style for Button
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.fontSize = 14;
            buttonStyle.normal.textColor = Color.white;
            buttonStyle.fontStyle = FontStyle.Bold;

            //Set style for subtext
            GUIStyle subtextStyle = new GUIStyle();
            subtextStyle.fontSize = 18;
            subtextStyle.normal.textColor = Color.white;

            //Print the title area
            GUILayout.BeginArea(new Rect(150, 50, 500, 500)); // 1
            GUILayout.Label("     TILE EDITOR   ", titleStyle, GUILayout.Width(150));
            GUILayout.Label("Click On A Tile To Edit Its Properties", subtextStyle, GUILayout.Width(300));
            GUILayout.EndArea(); //

            //Print out the instructions
            GUILayout.BeginArea(new Rect(50, 140, 500, 1000)); //2
            //GUILayout.Label("Enter all of the data for your new tile, and when you're done, press \"Create New Tile\"");
            GUILayout.Label("        If you need any help with this tool, or if you need anything else, ask Graydon");
            GUILayout.Label("");

            //Loads the tile prefabs and their scriptable objects into the local arrays
            string path = "Assets/Prefabs/Tiles/CurrentTiles";
            string[] files = Directory.GetFiles(path, "*.prefab", SearchOption.TopDirectoryOnly);
            tileScriptableObjects = new TileScriptableObject[files.Length];
            tilePrefabs = new GameObject[files.Length];
            for(int i = 0; i < files.Length; i++){
                tilePrefabs[i] = (AssetDatabase.LoadAssetAtPath(files[i], typeof(GameObject)) as GameObject);
                tileScriptableObjects[i] = tilePrefabs[i].GetComponent<Tile>().tileScriptableObject;
            }

            //Sets parameters of the Tile Factory Window
            for(int i = 0; i < tileScriptableObjects.Length; i++){
                if(GUILayout.Button(tileScriptableObjects[i].Name, buttonStyle)){
                    tileFactoryEditor tileEditorWindow = (tileFactoryEditor) EditorWindow.GetWindow(typeof(tileFactoryEditor), false, tileScriptableObjects[i].Name + " Tile Factory", true);
                    if(tilePrefabs[i] != null) tileEditorWindow.editingTilePrefab = tilePrefabs[i];
                    tileEditorWindow.isCreatingNewTile = false;
                    if(tileScriptableObjects[i].Name != null) tileEditorWindow.newTileName = tileScriptableObjects[i].Name;
                    if(tileScriptableObjects[i].AnnualCarbonAdded != null) tileEditorWindow.pollutionPerYear = tileScriptableObjects[i].AnnualCarbonAdded;
                    if(tileScriptableObjects[i].AnnualIncome != null) tileEditorWindow.moneyPerYear = tileScriptableObjects[i].AnnualIncome;

                    //Here I find the Button Manager in Scene and loop through each button to assign a new image to the one with a matching scriptable object
                    // TileSelectPanel[] tileButtonManagerArray = FindObjectsOfType(typeof(TileSelectPanel)) as TileSelectPanel[];
                    // if(tileButtonManagerArray[0] != null){
                    //     buttonScript[] allButtonScripts = tileButtonManagerArray[0].GetComponentsInChildren<buttonScript>();
                    //     foreach(buttonScript _buttonScript in allButtonScripts){
                    //         if(_buttonScript.tileToPlace.GetComponent<Tile>().tileScriptableObject == tileScriptableObject){
                    //             tileEditorWindow.setButtonImage(_buttonScript.gameObject.GetComponent<UnityEngine.UI.Image>());
                    //         }
                    //     }
                    // }   
                    tileEditorWindow.setButtonImage(tileFactoryEditor.getButtonWithScriptableObject(tileScriptableObjects[i]).GetComponent<UnityEngine.UI.Image>().sprite);
                    //if(tileScriptableObjects[i].MyButton != null) tileEditorWindow.setButtonImage(tileScriptableObjects[i].MyButton.GetComponent<UnityEngine.UI.Image>().sprite);

                }
            }

            GUILayout.EndArea();
            
    }
}
