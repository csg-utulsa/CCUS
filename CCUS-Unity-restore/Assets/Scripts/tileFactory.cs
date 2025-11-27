using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class tileFactory : MonoBehaviour
{
    public GameObject newTileModel;

    public Object blankTilePrefab;

    public bool isPlaceableObject;

    public int moneyPerYear;

    public int pollutionPerYear;
    
    public bool isPolluter;
    
    public string tileName = "Untitled Tile";

    public static tileFactory TM;

       
	public static void CreateTile ()
	{

        //GUILayout.Label("Enter Name Of Tile: ", EditorStyles.boldLabel);

        Debug.Log("Experience pure joy, mortal.");
        if(false){
        
        //tileName = EditorGUILayout.TextField("Tile Name = ", tileName);

        Object blankTilePrefab = new GameObject();
        blankTilePrefab = blankTilePrefab;//(Object)EditorGUILayout.ObjectField(blankTilePrefab, typeof(GameObject), true);


        TileScriptableObject newScriptableObject = ScriptableObject.CreateInstance<TileScriptableObject>();
        AssetDatabase.CreateAsset(newScriptableObject, "Assets/Scriptables/Tiles/" + TM.tileName + ".asset");
        AssetDatabase.SaveAssets();

        PrefabUtility.InstantiatePrefab(blankTilePrefab);

	    //blankTilePrefab = PrefabUtility.GetPrefabParent (Selection.activeGameObject);
		
        }
        


	}
}


