using UnityEngine;

public class DisabledFactoryPopUp : MonoBehaviour
{
    public Tile myFactory;

    void Awake()
    {
        // GameEventManager.current.GetEvent(EventType.E.TileJustPlaced).AddListener(TryToTurnOffPopUp);
        GameEventManager.current.GetEvent(EventType.E.TileJustDestroyed).AddListener(TryToTurnOffPopUp);
        // GameEventManager.current.GetEvent(EventType.E.PersonJustAdded).AddListener(TryToTurnOffPopUp);

    }

    // void Update(){
    //     TryToTurnOffPopUp();
    // }

    private void TryToTurnOffPopUp(){

        Debug.Log("Trying to turn off pop up");

        if(myFactory == null) {
            Destroy(this.gameObject);
            //Debug.LogError("\"myFactory\" was never assigned!");
        }
    }

    private void TurnOffPopUp(){
        Destroy(this.gameObject);
    }

    
}
