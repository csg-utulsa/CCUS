// Likely temporary script to manage swapping scenes
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public string gameScene;
    public string menuScene;

    #region Singleton
    static private SceneTransitionManager sceneManager;
    static public SceneTransitionManager SCENE_MANAGER { get { return sceneManager; } }

    void CheckSceneTransitionManagerIsInScene()
    {

        //Check if instnace is null
        if (sceneManager == null)
        {
            sceneManager = this; //set gm to this gm of the game object
            Debug.Log(sceneManager);
        }
        else //else if gm is not null a Game Manager must already exsist
        {
            Destroy(this.gameObject); //In this case you need to delete this gm
        }
        DontDestroyOnLoad(this); //Do not delete the GameManager when scenes load
        Debug.Log(sceneManager);
    }//end CheckGameManagerIsInScene()
    #endregion

    void Awake()
    {
        CheckSceneTransitionManagerIsInScene();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    //Exits the game
    public void QuitGame()
    {
        Debug.Log("Exited Game");
        Application.Quit();

    }

    // Transitions back to the menu
    public void GoToStartMenu()
    {
        SceneManager.LoadScene(menuScene);
    }

    // Transitions to the game scene
    public void GoToGameScene()
    {
        SceneManager.LoadScene(gameScene);
    }
}
