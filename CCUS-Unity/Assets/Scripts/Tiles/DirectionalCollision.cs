using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Created by: Aidan Pohl
 * Created: 11/20/2023
 * 
 * Description: Handler for the collison box of a Directional Collision child object of a tile,
 * Designed to be used in conjuntion with a DirectionalCollisionHandler on the parent obkject
 */
public class DirectionalCollision : MonoBehaviour
{
    private ConnectedTileHandler handler;
    public AdjacencyFlag direction;
    // Start is called before the first frame update
    void Start()
    {
        handler = transform.parent.GetComponent<ConnectedTileHandler>();
        //Debug.Log(handler.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tag)&&other.gameObject.GetComponent<DirectionalCollision>().direction != direction)
        {
            //Debug.Log(other.name);
            handler.AddNeighbor(direction, other.transform.parent.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("Leaving Direction");
        if(other.CompareTag(tag) && other.gameObject.GetComponent<DirectionalCollision>().direction != direction)
        handler.RemoveNeighbor(direction, other.transform.parent.gameObject);
    }
}


