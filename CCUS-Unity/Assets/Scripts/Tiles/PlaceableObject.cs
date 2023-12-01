using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectDrag))]
public class PlaceableObject : MonoBehaviour
{
    public bool placed;
    public Vector3Int Size { get; private set; }

    public Vector3[] Vertices;

    [SerializeField] Tile tile;

    ObjectDrag drag;

    private void Awake()
    {
       drag = GetComponent<ObjectDrag>(); 
    }

    private void GetColliderVertexPositionsLocal()
    {
        BoxCollider b = gameObject.GetComponent<BoxCollider>();
        Vertices = new Vector3[4];
        Vertices[0] = b.center + new Vector3(-b.size.x, -b.size.y, -b.size.z) * 0.5f;
        Vertices[1] = b.center + new Vector3(b.size.x, -b.size.y, -b.size.z) * 0.5f;
        Vertices[2] = b.center + new Vector3(b.size.x, -b.size.y, b.size.z) * 0.5f;
        Vertices[3] = b.center + new Vector3(-b.size.x, -b.size.y, b.size.z) * 0.5f;
    }

    private void CalculateSizeInCells()
    {
        Vector3Int[] vertices = new Vector3Int[Vertices.Length];
    
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 worldPos = transform.TransformPoint(Vertices[i]);
            vertices[i] = BuildingSystem.current.gridLayout.WorldToCell(worldPos);
        }
        
        Size = new Vector3Int(Math.Abs((vertices[0] - vertices[1]).x), 
                                Math.Abs((vertices[0] - vertices[3]).y), 
                                1);
    }

    public Vector3 GetStartPosition() 
    {
        return transform.TransformPoint(Vertices[0]);
    }

    private void Start() 
    {
        GetColliderVertexPositionsLocal();
        CalculateSizeInCells();
        
        if (placed) { drag.Place(); }//For Tiles that start out placed, activates them
    }

    public virtual void Place()
    {
        drag.Place();
        placed = true;

        //invoke events of placement
        //tile.SetTileState(TileState.Static);
    }

    public virtual void Pickup()
    {
        drag.Pickup();

        placed = false;

        tile.SetTileState(TileState.Moveable);
    }



}
