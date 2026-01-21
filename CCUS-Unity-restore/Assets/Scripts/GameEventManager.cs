using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameEventManager : MonoBehaviour
{
    public UnityEvent TileJustPlaced {get; set;}

    public UnityEvent ProgressEventJustCalled {get; set;}
    
    public UnityEvent PersonJustAdded {get; set;}

    public UnityEvent NetCarbonUpdated {get; set;}

    public UnityEvent NetMoneyUpdated {get; set;}

    public UnityEvent MoneyAmountUpdated {get; set;}

    public UnityEvent SwitchedCurrentGroundChunk {get; set;}

    public UnityEvent PurchasedCurrentGroundChunk {get; set;}

    public UnityEvent BeginSwitchingCurrentGroundChunk {get; set;}

    public UnityEvent ActivatableTileJustPlaced {get; set;}

    public UnityEvent ActivatableTileJustDestroyed {get; set;}

    public UnityEvent TileJustDestroyed {get; set;}

    public UnityEvent BuildingActivationStateChanged {get; set;}

    public static GameEventManager current;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        TileJustPlaced = new UnityEvent();
        ProgressEventJustCalled = new UnityEvent();
        PersonJustAdded = new UnityEvent();
        NetCarbonUpdated = new UnityEvent();
        NetMoneyUpdated = new UnityEvent();
        MoneyAmountUpdated = new UnityEvent();
        SwitchedCurrentGroundChunk = new UnityEvent();
        PurchasedCurrentGroundChunk = new UnityEvent();
        BeginSwitchingCurrentGroundChunk = new UnityEvent();
        ActivatableTileJustPlaced = new UnityEvent();
        ActivatableTileJustDestroyed = new UnityEvent();
        TileJustDestroyed = new UnityEvent();
        BuildingActivationStateChanged = new UnityEvent();

        if(current == null){
            current = this;
        }else{
            Destroy(this);
        }
    }


}
