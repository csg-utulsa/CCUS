using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameEventManager : MonoBehaviour
{
    public UnityEvent TileJustPlaced {get; set;}
    public UnityEvent ProgressEventJustCalled {get; set;}
    public ProgressionManager.ProgressEventType TypeOfLastProgressEventCalled {get; set;}
    
    public UnityEvent PersonJustAdded {get; set;}

    public static GameEventManager current;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        TileJustPlaced = new UnityEvent();
        ProgressEventJustCalled = new UnityEvent();
        PersonJustAdded = new UnityEvent();
        if(current == null){
            current = this;
        }else{
            Destroy(this);
        }
    }


}
