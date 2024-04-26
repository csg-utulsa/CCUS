using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class LevelComplete : MonoBehaviour
{   
    DataManager dm = DataManager.DM;

    public GameObject[] NeedRoads;//Tiles that should be connected to a source road for level to be complete

    public Vector2 WinPollutionRange = new Vector2(float.MinValue, 1000f);//Valid Pollution levels for winning Game;
    public float GameOverPollutionLevel = 2000f; //Max amount of polution before gameover is hit

    public Vector2 WinMoneyRange = new Vector2(0f, float.MaxValue);
    public float GameOverMoneyLevel = -500f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    /*Checks whether the level is in a "Win State" according to variables
     * Should be triggered by player submission or when time runs out
     * returns true if all win conditions are fulfilled returns false otherwise
     */
    public bool CheckWinConditions()
    {   
        foreach (var connection in NeedRoads)
        {
            if (!connection.GetComponent<TileConnectionAdjacent>().connected) //if any tile that should be connected isnt, return false;
                Debug.Log(connection.gameObject.name + "not connected.");
                return false;
        }

        //Check if Carbon is in Range
        if (dm.GetCarbon() < WinPollutionRange.x || dm.GetCarbon() > WinPollutionRange.y) return false;
        
        //If Money is in range
        if(dm.GetMoney() < WinMoneyRange.x || dm.GetMoney() > WinMoneyRange.y) return false;


        return true;
    }//end CkeckWinConditions()

    /*Checks whether the level is in an immediate lose state
     * Checks at each Tick()
     * return true if any lose condition is fulfilled, returns false otherwise
     */
    public bool CheckLoseConditions()
    {   
        //If Carbon is way too high
        if (dm.GetCarbon() > GameOverPollutionLevel) return true;
        //if money are way too high
        if (dm.GetMoney() < GameOverMoneyLevel) return true;
        //later lose conditions can be handled here
        return true;
    }
}
