using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TileConnectionAdjacent : MonoBehaviour
{
    [Header("Current Adjacecy")]
    public List<GameObject> neighborGO = new List<GameObject>();
    private GameObject  tempNeighbor;//to handle floating neighbors

    public bool connected = false;
    public bool checkedConnectivity = false;
    public GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
        DataManager.tileConnectionReset.AddListener(OnTileConnectionReset);
    }

    // Update is called once per frame
    void Update()
    {
        if ((tempNeighbor != null) && (tempNeighbor.GetComponent<PlaceableObject>().placed))
        {

            AddNeighbor(tempNeighbor);
            RemoveTempNeighbor();
        }

            


    }
    private void LateUpdate()
    {
        ConnectivityCheck();
    }


    public void AddNeighbor( GameObject neighbor)
    {
        Debug.Log("Henlo");
        if (!neighbor.GetComponent<PlaceableObject>().placed)
        {
            tempNeighbor = neighbor;
        }
        else
        {
            neighborGO.Add(neighbor);
        }
    }

    public void RemoveTempNeighbor()
    {
        tempNeighbor = null;
    }

    public void RemoveNeighbor(GameObject neighbor)
    {
        if (neighbor == tempNeighbor)
        {
            RemoveTempNeighbor();
        }
        else
        {
            neighborGO.Remove(neighbor);
        }

    }//end remve neighbor


    private void ConnectivityCheck()
    {
        checkedConnectivity = true;
        foreach (var neighbor in neighborGO){
            if (neighbor != null)
            {
                if (neighbor.GetComponent<ConnectedTileHandler>().distToSource != -1)
                {
                    connected = true;
                    canvas.SetActive(!connected);
                    return;
                }
            }
        }
        connected = false;
        canvas.SetActive(!connected);
        return;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Direction"))
        {
            Debug.Log("!!!!!!!!!!!");
            AddNeighbor(other.transform.parent.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("Leaving Direction");
        if (other.CompareTag("Direction"))
            RemoveNeighbor(other.transform.parent.gameObject);
    }

    public void OnTileConnectionReset()
    {
        connected = false;
    }
}
