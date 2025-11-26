using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class testTileSelectPanel : MonoBehaviour
{
    public GameObject ObjectToRemove;
    void Start(){
        StartCoroutine(testButtonCreation());
    }
    IEnumerator testButtonCreation(){
        yield return new WaitForSeconds(8f);
        TileSelectPanel.TSP.RemoveButton(ObjectToRemove);
    }
}