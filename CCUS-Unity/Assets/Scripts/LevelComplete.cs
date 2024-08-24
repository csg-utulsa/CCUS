using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
//using UnityEditor.MemoryProfiler;
using UnityEngine;
using UnityEngine.UI;

/**script for handling win/lose conditions for a level
 * Lastedited By: Aidan pohl
 * Las Edited May 5 2024
 */
public class LevelComplete : MonoBehaviour
{   
    LevelManager lm = LevelManager.LM;

    public GameObject[] NeedRoads;//Tiles that should be connected to a source road for level to be complete

    public Vector2 WinPollutionRange = new Vector2(float.MinValue, 1000f);//Valid Pollution levels for winning Game;
    public float GameOverPollutionLevel = 2000f; //Max amount of polution before gameover is hit

    public Vector2 WinMoneyRange = new Vector2(0f, float.MaxValue);
    public float GameOverMoneyLevel = -500f;

    public Button lvlEndButton; //button for confirming Level End;
    // Start is called before the first frame update

    public GameObject[] disablePool;//things to disable on game end
    public GameObject winText;
    public GameObject loseText;
    public GameObject errText;
    void Start()
    {
        lm = LevelManager.LM;
    }

    // Update is called once per frame
    void Update()
    {
        if (lm.levelState == LevelManager.GameState.Active)
        {
            if (CheckLoseConditions())
            {
                lm.SetLevelState(LevelManager.GameState.Lose);
                EndLevel();
            }
            
            lvlEndButton.interactable = CheckWinConditions();
        }
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
        if (lm.GetCarbon() < WinPollutionRange.x || lm.GetCarbon() > WinPollutionRange.y)
        {Debug.Log("Carbon too high");
            return false;
            
        }
        //If Money is in range
        if (lm.GetMoney() < WinMoneyRange.x || lm.GetMoney() > WinMoneyRange.y) { Debug.Log("Money too low"); return false; }


        return true;
    }//end CkeckWinConditions()

    /*Checks whether the level is in an immediate lose state
     * Checks at each Tick()
     * return true if any lose condition is fulfilled, returns false otherwise
     */
    public bool CheckLoseConditions()
    {
        //If Carbon is way too high
        if (lm.GetCarbon() > GameOverPollutionLevel) { Debug.Log(lm.GetCarbon() + "Carbon"); return true; }
        //if money are way too high
        if (lm.GetMoney() < GameOverMoneyLevel) { Debug.Log(lm.GetMoney()+ "Money"); return true; }
        //later lose conditions can be handled here
        return false ;
    }

    public void WinButton()
    {//TODO: Add 10 years speed up to confirm winstate is stable
        lm.SetLevelState(LevelManager.GameState.Win);
        EndLevel();
    }
    public void EndLevel()
    {
        
        foreach( GameObject go in disablePool)
        {
            go.SetActive(false);
        }
        switch(lm.levelState)
        {
            case LevelManager.GameState.Win:
                winText.SetActive(true); break;
            case LevelManager.GameState.Lose:
                loseText.SetActive(true); break;
            default: errText.SetActive(true); break;
        }
    }
}
