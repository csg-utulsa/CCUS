using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.IO;

//[CreateAssetMenu(fileName = "New Tile", menuName = "Tile System/Tile")]
//[CustomEditor(typeof(tileFactory))]
public class tileFactoryEditor : EditorWindow
{
    public GameObject blankTilePrefab;
    //public string newTileName = "Untitled Tile";
    bool importErrors = false;

    public string newTileName = "Untitled Tile";
    public int moneyPerYear = 0;
    public int pollutionPerYear = 0;
    public bool isPolluter = false;
    public string newTileMeshFilePath;
    bool closeThisWindow = false;

    GameObject newTilePrefab;



    [MenuItem("Tools/Tile Factory")]
    public static void CreateNewTile()
    {
        tileFactoryEditor wnd = GetWindow<tileFactoryEditor>();
        wnd.minSize = new Vector2(600,600);
        //GetWindow<TileFactoryEditor>("tileFactoryEditor");
    }

    private GameObject newTileMesh;

    void OnGUI()
        {
            //Set style for TILE FACTORY 3000
            GUIStyle welcomeStyle = new GUIStyle();
            welcomeStyle.fontSize = 35;
            welcomeStyle.normal.textColor = Color.white;
            welcomeStyle.fontStyle = FontStyle.Bold;

            //Set style for Button
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.fontSize = 25;
            buttonStyle.normal.textColor = Color.white;
            buttonStyle.fontStyle = FontStyle.Bold;

            //Set style for subtext
            GUIStyle subtextStyle = new GUIStyle();
            subtextStyle.fontSize = 18;
            subtextStyle.normal.textColor = Color.white;

            //Print the title area
            GUILayout.BeginArea(new Rect(120, 50, 500, 500)); // 1
            GUILayout.Label("TILE FACTORY 3000", welcomeStyle, GUILayout.Width(150));
            GUILayout.Label("A Project I'm Unreasonably Happy About", subtextStyle, GUILayout.Width(300));
            GUILayout.EndArea(); //1

            //Print out the instructions
            GUILayout.BeginArea(new Rect(50, 140, 500, 1000)); //2
            GUILayout.Label("Enter all of the data for your new tile, and when you're done, press \"Create New Tile\"");
            GUILayout.Label("If you need any help with this tool, or if you need anything, ask Graydon");
            GUILayout.Label("");


            //Ask for all of the input data

            //Check input mesh is of type .fbx
            //GameObject newTileMesh = null;
            //newTileMesh = (Object)EditorGUILayout.ObjectField("New Object Mesh", newTileMesh, typeof(Object), true);
            //newTileMeshFilePath = AssetDatabase.GetAssetPath(newTileMesh);
            //newTileMeshFilePath = EditorUtility.OpenFilePanel("Select a File", "Assets/Models", "");
            //remove the && false from the next line

            newTileMesh = (GameObject)EditorGUILayout.ObjectField("FBX File", newTileMesh, typeof(GameObject), false);
            newTileMeshFilePath = AssetDatabase.GetAssetPath(newTileMesh);


            // if(Path.GetExtension(newTileMeshFilePath) != ".fbx") {
            //     importErrors = true;
            //     Debug.LogError("As of now, this tool can only import 3D model files of the type \".fbx\". .Fbx files are useful because they also contain all of the colors/materials with a model.");
            // }

            newTileName = EditorGUILayout.TextField("Enter Tile Name", newTileName);

            moneyPerYear = EditorGUILayout.IntField("Money Per Year", moneyPerYear);

            pollutionPerYear = EditorGUILayout.IntField("Pollution Per Year", pollutionPerYear);

            isPolluter = EditorGUILayout.Toggle("Polluting Tile?", isPolluter);

            GUILayout.Label("");
            

            //Create the new Tile
            if (GUILayout.Button("Create New Tile", buttonStyle))
            {
                if(Path.GetExtension(newTileMeshFilePath) != ".fbx") {
                    importErrors = true;
                    Debug.LogError("As of now, this tool can only import 3D model files of the type \".fbx\". .Fbx files are useful because they also contain all of the colors/materials with a model.");
                }
                if(importErrors == true){
                    Debug.LogError("I'm so sorry! You have to fix all the import errors before creating a new tile :(");
                } else {
                    //Debug.LogError("I'm so sorry if there are any bugs! I haven't finished this tool yet :(");
                    CreateNewPrefab();
                    
                    closeThisWindow = true; //this.Close();
                }
                
                
            }
            GUILayout.EndArea();
            if(closeThisWindow){
                this.Close();
            }
           //GUILayout.EndArea(); //2
        }

        public void CreateNewPrefab() {
            Mesh loadedNewTileMesh = AssetDatabase.LoadAssetAtPath<Mesh>(newTileMeshFilePath);
            //Debug.Log("filepath = " + newTileMeshFilePath);
            newTilePrefab = (GameObject)PrefabUtility.InstantiatePrefab(blankTilePrefab);//Instantiate(blankTilePrefab);
            if(newTileName != null) {
                newTilePrefab.name = newTileName;
            }

            Mesh instanceOfNewTileMesh = Instantiate(loadedNewTileMesh);
            
            newTilePrefab.GetComponentInChildren<MeshFilter>().mesh = instanceOfNewTileMesh;
            
            //Delete the next two lines
            //GameObject spawnedObject = (GameObject)PrefabUtility.InstantiatePrefab(newTilePrefab);
            //spawnedObject.transform.position = Vector3.zero;
            //Selection.activeGameObject = spawnedObject;
        }

    // public void CreateNewTile() {

    //     EditorGUILayout.LabelField("Create a new tile:", EditorStyles.boldLabel);
    //     GUILayout.Label("Fill out the fields below for the new tile");
    //     GUILayout.Label("");
    //     if(GUILayout.Button("Create a New Tile")) {
    //         tileFactory.CreateTile();
    //         //Editor.Repaint();
    //     }
    //     GUILayout.Label("");
    //     GUILayout.Label("");

    //     //DrawDefaultInspector();
        
        

    // }


}
