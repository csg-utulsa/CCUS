using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.IO;
using System.Linq;

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
    public bool isCreatingNewTile = false;

    GameObject newTilePrefab;

    public GameObject editingTilePrefab;

    //public tileFactoryEditor myWindow;




    [MenuItem("Tools/Create Tile")]
    public static void CreateNewTile()
    {
        tileFactoryEditor wnd = GetWindow<tileFactoryEditor>();
        //wnd.myWindow = wnd;
        wnd.minSize = new Vector2(600,600);
        wnd.isCreatingNewTile = true;
        //GetWindow<TileFactoryEditor>("tileFactoryEditor");
    }

    public GameObject newTileMesh;

    void OnGUI()
        {
            string buttonText = (isCreatingNewTile) ? "Create New Tile" : "Save Tile Settings";
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

            //Set style for Delete Button
            GUIStyle deleteButtonStyle = new GUIStyle(GUI.skin.button);
            deleteButtonStyle.fontSize = 14;
            deleteButtonStyle.normal.textColor = Color.red;
            deleteButtonStyle.fontStyle = FontStyle.Bold;

            //Set style for subtext
            GUIStyle subtextStyle = new GUIStyle();
            subtextStyle.fontSize = 18;
            subtextStyle.normal.textColor = Color.white;

            //Print the title area
            GUILayout.BeginArea(new Rect(120, 50, 500, 500)); // 1
            if(isCreatingNewTile){
                GUILayout.Label("TILE CREATOR 3000", welcomeStyle, GUILayout.Width(150));
            } else{
                GUILayout.Label(" TILE EDITOR 3000", welcomeStyle, GUILayout.Width(150));
                
            }
            
            //GUILayout.Label("A Project I'm Unreasonably Happy About", subtextStyle, GUILayout.Width(300));
            GUILayout.EndArea(); //1

            //Print out the instructions
            GUILayout.BeginArea(new Rect(50, 140, 500, 1000)); //2
            if(!isCreatingNewTile){
                GUILayout.Label(newTileName + ": ", subtextStyle, GUILayout.Width(300));
            }
            GUILayout.Label("Enter all of the data for your new tile. When you're done, press \"" + buttonText + "\"");
            GUILayout.Label("If you need any help with this tool, or if you need anything, ask Graydon");
            GUILayout.Label("");


            //Ask for all of the input data

            //Check input mesh is of type .fbx
            //GameObject newTileMesh = null;
            //newTileMesh = (Object)EditorGUILayout.ObjectField("New Object Mesh", newTileMesh, typeof(Object), true);
            //newTileMeshFilePath = AssetDatabase.GetAssetPath(newTileMesh);
            //newTileMeshFilePath = EditorUtility.OpenFilePanel("Select a File", "Assets/Models", "");
            //remove the && false from the next line

            if(isCreatingNewTile){
                newTileMesh = (GameObject)EditorGUILayout.ObjectField("FBX File", newTileMesh, typeof(GameObject), false);
                newTileMeshFilePath = AssetDatabase.GetAssetPath(newTileMesh);
            }
            
            //Debug.Log(newTileMeshFilePath);


            // if(Path.GetExtension(newTileMeshFilePath) != ".fbx") {
            //     importErrors = true;
            //     Debug.LogError("As of now, this tool can only import 3D model files of the type \".fbx\". .Fbx files are useful because they also contain all of the colors/materials with a model.");
            // }

            newTileName = EditorGUILayout.TextField("Enter Tile Name", newTileName);

            moneyPerYear = EditorGUILayout.IntField("Money Per Year", moneyPerYear);

            pollutionPerYear = EditorGUILayout.IntField("Pollution Per Year", pollutionPerYear);

            //isPolluter = EditorGUILayout.Toggle("Polluting Tile?", isPolluter);

            GUILayout.Label("");
            

            //Create the new Tile
            //string buttonText = (isCreatingNewTile) ? "Create New Tile" : "Save Tile Settings";
            if(isCreatingNewTile){
                if (GUILayout.Button(buttonText, buttonStyle))
                {

                    if(Path.GetExtension(newTileMeshFilePath) != ".fbx") {
                        importErrors = true;
                        Debug.LogError("As of now, this tool can only import 3D model files of the type \".fbx\". .Fbx files are useful because they also contain all of the colors/materials with a model.");
                    }
                    if(importErrors == true){
                        Debug.LogError("TILE CREATION FAILED! (I'm so sorry) You have to fix all the import errors before creating a new tile :(");
                    } else {
                        //Debug.LogError("I'm so sorry if there are any bugs! I haven't finished this tool yet :(");
                        
                        bool prefabCreationSuccess = CreateNewPrefab();
                        if(!prefabCreationSuccess) Debug.LogError("Tile Creation Error! It couldn't successfully create a prefab in the prefabs folder.");   

                        closeThisWindow = true; //this.Close();
                    }
                    
                    
                }
            } else{
                if(GUILayout.Button(buttonText, buttonStyle)){
                    UpdatePrefabSettings();
                    closeThisWindow = true;
                }
            }
            

            //Add Space
            GUILayout.Label("");
            GUILayout.Label("");
            
            //Opens Delete Tile Dialog Warning
            if (!isCreatingNewTile && GUILayout.Button("DELETE THIS TILE", deleteButtonStyle)){
                deleteTileButton tileButtonDeleteWindow = (deleteTileButton) EditorWindow.GetWindow(typeof(deleteTileButton), false, "DELETE " + newTileName, true);
                tileButtonDeleteWindow.minSize = new Vector2(600,300);
                tileButtonDeleteWindow.myWindow = tileButtonDeleteWindow;
                tileButtonDeleteWindow.tileToDelete = editingTilePrefab;
            }

            GUILayout.EndArea();
            if(closeThisWindow){
                this.Close();
            }
           //GUILayout.EndArea(); //2
        }

        public void UpdatePrefabSettings(){
            
            if(editingTilePrefab == null){
                Debug.LogError("No prefab was assigned to the Tile Factory when opened by the Tile Editor! (That's Graydon's fault)");
            }

            //Updates mesh if the user dragged a new one in
            if(newTileMeshFilePath != null){
                if(Path.GetExtension(newTileMeshFilePath) != ".fbx") {
                    //importErrors = true;
                    Debug.LogError("As of now, this tool can only import 3D model files of the type \".fbx\". .Fbx files are useful because they also contain all of the colors/materials with a model.");
                    return;
                }


                //Sets new mesh:
                Mesh loadedNewTileMesh = AssetDatabase.LoadAssetAtPath<Mesh>(newTileMeshFilePath);
                Mesh instanceOfNewTileMesh = Instantiate(loadedNewTileMesh);
                editingTilePrefab.GetComponentInChildren<MeshFilter>().mesh = instanceOfNewTileMesh;
            }

            if(newTileName != null) editingTilePrefab.name = newTileName;
            
            if(editingTilePrefab.GetComponent<Tile>().tileScriptableObject != null){
                TileScriptableObject tileScriptableObject = editingTilePrefab.GetComponent<Tile>().tileScriptableObject;
                if(newTileName != null) tileScriptableObject.name = newTileName;
                if(pollutionPerYear != null) tileScriptableObject.AnnualCarbonAdded = pollutionPerYear;
                if(moneyPerYear != null) tileScriptableObject.AnnualIncome = moneyPerYear;
            }

        }

        public bool CreateNewPrefab() {
            if(isCreatingNewTile){

                //The next few lines save the prefab as an asset
                string prefabFilePath = "Assets/Prefabs/Tiles/CurrentTiles/";
                string fullPrefabFilePath = prefabFilePath + newTileName + ".prefab";
                fullPrefabFilePath = AssetDatabase.GenerateUniqueAssetPath(fullPrefabFilePath); //Prevents two tiles from having same file path
                bool success = false;
                newTilePrefab = (GameObject)PrefabUtility.SaveAsPrefabAsset(blankTilePrefab, fullPrefabFilePath, out success); //first arg of saveAsPrefabAsset() used to be blankTilePrefab
                if(!success){ return false; }

                //Temporarily Instantiates blankTilePrefab in the Scene
                //newTilePrefab = (GameObject)PrefabUtility.InstantiatePrefab(blankTilePrefab);

                //Gets New Tile's Mesh
                Mesh loadedNewTileMesh = AssetDatabase.LoadAssetAtPath<Mesh>(newTileMeshFilePath);

                //AssetDatabase.CreateAsset(loadedNewTileMesh, newTileMeshFilePath);

                //Sets Mesh of New Object
                Mesh instanceOfNewTileMesh = Instantiate(loadedNewTileMesh); //If the program works, delete this line
                newTilePrefab.GetComponentInChildren<MeshFilter>().mesh = loadedNewTileMesh;//instanceOfNewTileMesh;

                //Assigns materials to mesh renderer
                Material[] materials = AssetDatabase.LoadAllAssetsAtPath(newTileMeshFilePath).Where(x => x.GetType() == typeof(Material)).Cast<Material>().ToArray();
                newTilePrefab.GetComponentInChildren<MeshRenderer>().sharedMaterials = materials;
                
                

                //Deletes the tile from the scene since it's saved as an asset now.
                //DestroyImmediate(newInstantiatedTile);



                //Delete NExt LINE!
                //newTilePrefab = (GameObject)PrefabUtility.InstantiatePrefab(blankTilePrefab);//Instantiate(blankTilePrefab);

                

                


                // GameObject meshChild = (GameObject)PrefabUtility.InstantiatePrefab(meshPrefab);
                // meshChild.transform.SetParent(parentTile.transform);
                // meshChild.transform.localPosition = Vector3.zero;
                // meshChild.transform.localRotation = Quaternion.identity;
                // meshChild.transform.localScale = Vector3.one;

                

                //Creates and sets parameters of new tile's scriptable object
                //Creates new Scriptable Object
                ScriptableObject.CreateInstance<TileScriptableObject>();
                TileScriptableObject newScriptableObject = ScriptableObject.CreateInstance<TileScriptableObject>();// new TileScriptableObject();

                //Save Scriptable Object At Path
                string newScriptableObjectPath = "Assets/Scriptables/Tiles/" + newTileName + ".asset";
                newScriptableObjectPath = AssetDatabase.GenerateUniqueAssetPath(newScriptableObjectPath); //Prevents scriptables from having same file paths
                AssetDatabase.CreateAsset(newScriptableObject, newScriptableObjectPath);
                newTilePrefab.name = newTileName;
                newScriptableObject.Name = newTileName;
                newScriptableObject.AnnualCarbonAdded = pollutionPerYear;
                newScriptableObject.AnnualIncome = moneyPerYear;
                
                //Sets Scriptable Object Of New Object
                newTilePrefab.GetComponentInChildren<Tile>().tileScriptableObject = newScriptableObject;
                
            } else{
                
            }
            


            

            //Mesh instanceOfNewTileMesh = Instantiate(loadedNewTileMesh);
            
            //newTilePrefab.GetComponentInChildren<MeshFilter>().mesh = instanceOfNewTileMesh;
            
            return true;

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
