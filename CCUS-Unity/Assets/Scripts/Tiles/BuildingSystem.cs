using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem current { get; private set; }

    public GridLayout gridLayout;
    private Grid grid;
    [SerializeField] private Tilemap MainTilemap;
    [SerializeField] private TileBase whiteTile;

    [SerializeField] GameObject[] prefabs;

    /*public GameObject prefab1;
    public GameObject prefab2;*/

    private GameObject activeObject;
    private PlaceableObject objectToPlace;

    #region Unity methods

    private void Awake() 
    {
        current = this;
        grid = gridLayout.gameObject.GetComponent<Grid>();
    }

    private void Update()
    {
        //THIS IS DUMB. DONT LEAVE AFTER THIS BUILD. new input system? UI instead? ~Coleton
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (1 <= prefabs.Length)
                InitializeWithObject(prefabs[0]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (2 <= prefabs.Length)
                InitializeWithObject(prefabs[1]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (3 <= prefabs.Length)
                InitializeWithObject(prefabs[2]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (4 <= prefabs.Length)
                InitializeWithObject(prefabs[3]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (5 <= prefabs.Length)
                InitializeWithObject(prefabs[4]);
        }

        if (!activeObject)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CanBePlaced(objectToPlace))
            {
                objectToPlace.Place();
                Vector3Int start = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
                TakeArea(start, objectToPlace.Size);
                activeObject = null;
                objectToPlace = null;
            }
            else
            {
                Destroy(activeObject);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(activeObject);
        }
    }

    #endregion

    #region Utils

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

    public Vector3 SnapCoordinateToGrid(Vector3 position) 
    {
        position = position + GetMouseWorldPosition();
        Vector3Int cellPos = gridLayout.WorldToCell(position);
        position = grid.GetCellCenterWorld(cellPos);
        return position;
    }

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
        if (objectToPlace != null) return;
        Vector3 position = SnapCoordinateToGrid(Vector3.zero);

        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        activeObject = obj;
        objectToPlace = obj.GetComponent<PlaceableObject>();
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

        TileBase[] baseArray = GetTilesBlock(area, MainTilemap);

        foreach (var b in baseArray)
        {
            if (b == whiteTile)
            {
                return false;
            }
        }

        return true;
    }

    public void TakeArea(Vector3Int start, Vector3Int size)
    {
        MainTilemap.BoxFill(start, whiteTile, start.x, start.y,
                            start.x + size.x, start.y + size.y);
    }

    #endregion
}
