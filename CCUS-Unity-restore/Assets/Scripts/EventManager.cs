using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public UnityEvent TileJustPlaced {get; set;}
    public static EventManager current;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        TileJustPlaced = new UnityEvent();
        if(current == null){
            current = this;
        }else{
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
