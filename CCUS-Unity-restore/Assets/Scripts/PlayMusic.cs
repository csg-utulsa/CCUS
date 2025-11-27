using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    GameObject gameMusicManager;
    MusicManager musicManagerScript;
    public enum sceneType {Menu, Level}
    public sceneType thisSceneType = sceneType.Level;
    // Start is called before the first frame update
    void Start()
    {
        if (gameMusicManager==null){
            gameMusicManager = GameObject.FindGameObjectWithTag("MusicManager");
        }

        if (gameMusicManager!=null){
            musicManagerScript = gameMusicManager.GetComponent<MusicManager>();
            switch(thisSceneType){
                case sceneType.Menu:
                musicManagerScript.PlayMenuMusic();
                break;
                case sceneType.Level:
                musicManagerScript.PlayLevelMusic();
                break;
            }
        }
    }
}
