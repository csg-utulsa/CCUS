using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
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

            string path = "Assets/Prefabs/Tiles/CurrentTiles";
            string[] files = Directory.GetFiles(path, "*.prefab", SearchOption.TopDirectoryOnly);
            tileScriptableObjects = new TileScriptableObject[files.Length];
            tilePrefabs = new GameObject[files.Length];
            for(int i = 0; i < files.Length; i++){
                tilePrefabs[i] = (AssetDatabase.LoadAssetAtPath(files[i], typeof(GameObject)) as GameObject);
                tileScriptableObjects[i] = tilePrefabs[i].GetComponent<Tile>().tileScriptableObject;
            }

            for(int i = 0; i < tileScriptableObjects.Length; i++){// TileScriptableObject scriptableObject in ){
                if(GUILayout.Button(tileScriptableObjects[i].Name, buttonStyle)){
                    tileFactoryEditor tileEditorWindow = (tileFactoryEditor) EditorWindow.GetWindow(typeof(tileFactoryEditor), false, tileScriptableObjects[i].Name + " Tile Factory", true);
                    //tileEditorWindow.wnd = tileEditorWindow;
                    if(tilePrefabs[i] != null) tileEditorWindow.editingTilePrefab = tilePrefabs[i];
                    tileEditorWindow.isCreatingNewTile = false;
                    if(tileScriptableObjects[i].Name != null) tileEditorWindow.newTileName = tileScriptableObjects[i].Name;
                    if(tileScriptableObjects[i].AnnualCarbonAdded != null) tileEditorWindow.pollutionPerYear = tileScriptableObjects[i].AnnualCarbonAdded;
                    if(tileScriptableObjects[i].AnnualIncome != null) tileEditorWindow.moneyPerYear = tileScriptableObjects[i].AnnualIncome;
                    //if(scriptableObject.Name != null) tileEditorWindow.newTileName = scriptableObject.Name;
                    //if(scriptableObject.TileMesh != null) tileEditorWindow.newTileMesh = (GameObject)scriptableObject.TileMesh;
                }
            }

            GUILayout.EndArea();
            
    }
}
