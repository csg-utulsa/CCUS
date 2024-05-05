/** TODO: 
*   - Add method that spawns object under mouse when GUI is clicked 
*   - Have object follow mouse outside of world border
        -- Change click-to-drag to always follow w/ click to place
*   - Prevent multiple buildings to follow mouse (true/false)
**/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem current { get; private set; }

    public GridLayout gridLayout;
    private Grid grid;
    [SerializeField] private Tilemap TerrainTilemap;
    [SerializeField] private Tilemap PlaceablesTilemap;
    [SerializeField] private TileBase whiteTile;

    [SerializeField] GameObject[] prefabs;

    /*public GameObject prefab1;
    public GameObject prefab2;*/

    private GameObject activeObject;
    private PlaceableObject objectToPlace;
    private Tile activeTile;

    #region Unity methods

    private void Awake() 
    {
        current = this;
        grid = gridLayout.gameObject.GetComponent<Grid>();
    }

    private void Update()
    {


        if (!activeObject)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))//Object placing
        {
            if (CanBePlaced(objectToPlace))
            {
                objectToPlace.Place();
                Vector3Int start = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
                TakeArea(start, objectToPlace.Size);
                int cost = activeTile.tileScriptableObject.BuildCost;
                LevelManager.LM.AdjustMoney(-1 * cost);
                activeObject = null;
                objectToPlace = null;
                activeTile = null;
            }
            else
            {
                //Destroy(activeObject);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Destroy(activeObject);
        }
    }

    #endregion

    #region Utils

    // Raycast to get world position of mouse hover input
    public static Vector3 GetMouseWorldPosition() 
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            return raycastHit.point;
        }
        else 
        {
            return Vector3.zero;
        }
    }

    //gets cell center's world position
    public Vector3 SnapCoordinateToGrid(Vector3 position) 
    {
        //position = position + GetMouseWorldPosition();
        Vector3Int cellPos = gridLayout.WorldToCell(position);
        position = grid.GetCellCenterWorld(cellPos);
        return position;
    }

    /**
    // NOTE: Could work for objects that placed on top of terrain tiles BUT a separate grid might be better

    public Vector3 SnapCoordinateToGrid(Vector3 position, bool terrain) 
    {
        //position = position + GetMouseWorldPosition();
        Vector3Int cellPos = gridLayout.WorldToCell(position);
        position = grid.GetCellCenterWorld(cellPos);
        if (!terrain) position.y = position.y + 1f;
        return position;
    }
    **/
    
    

    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;

        foreach (var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }

        return array;
    }

    #endregion

    #region Building Placement

    public void InitializeWithObject(GameObject prefab)
    {
        //Destroy(activeObject);
        //if (objectToPlace != null) return;
        Vector3 position = SnapCoordinateToGrid(Vector3.zero);

        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        activeObject = obj;
        objectToPlace = obj.GetComponent<PlaceableObject>();
        activeTile = obj.GetComponent<Tile>();
    }

    public bool MoveObject(GameObject obj)
    {
        if (activeObject != null) return false;
        activeObject = obj;
        objectToPlace = activeObject.GetComponent<PlaceableObject>();
        return true;
    }

    public bool CanBePlaced(PlaceableObject placeableObject)
    {
        BoundsInt area = new BoundsInt();
        area.position = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
        area.size = new Vector3Int(area.size.x + 1, area.size.y + 1, area.size.z);

        TileBase[] baseArray = GetTilesBlock(area, TerrainTilemap);

        foreach (var b in baseArray)
        {
            if (b == whiteTile)
            {
                return false;
            }
        }

        int cost = activeTile.tileScriptableObject.BuildCost;
        if (cost != 0 && activeTile.state == TileState.Uninitialized)
        {
            if (LevelManager.LM.GetMoney() < cost)
            {
                return false;
            }
        }

        if (!activeTile.gameObject.GetComponent<ObjectDrag>().IsValidOverlap()) { return false; }//makes sure tile is not placed with invalid tiles

        return true;
    }

    public void TakeArea(Vector3Int start, Vector3Int size)
    {
        switch (activeObject.GetComponent<Tile>().GetTileType())
        {
            case TileType.Terrain:
                TerrainTilemap.BoxFill(start, whiteTile, start.x, start.y,
                            start.x + size.x, start.y + size.y);
                break;
            case TileType.Placeable:
                PlaceablesTilemap.BoxFill(start, whiteTile, start.x, start.y,
                            start.x + size.x, start.y + size.y);
                break;
        }
    }

    #endregion
}
