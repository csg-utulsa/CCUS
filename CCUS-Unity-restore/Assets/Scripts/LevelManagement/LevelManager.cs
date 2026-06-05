using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    #region Singleton
    static public LevelManager LM { get { return lm; } }
    static private LevelManager lm;

    void LoadManager()
    {
        if (lm == null)
            lm = this;
        else
            Destroy(this.gameObject);
    }
    #endregion
    [Header("Initial Stats")]
    [SerializeField] private int maxCarbon = 200;
    [SerializeField] private int carbonCapPastMax = 0;
    [SerializeField] int startingMoney;
    [SerializeField] int startingCarbon;
    [SerializeField] int startingYear;
    [Header("Current Stats")]
    //public GameState levelState = GameState.Inactive;
    [SerializeField] private int money;
    [SerializeField] int yearlyMoney;
    [SerializeField] int yearlyCarbon;
    [SerializeField] int carbon;
    [SerializeField] int year;
    [SerializeField] int storageCapacity;
    [SerializeField] int stored;


    private int netCarbon = 0;
    [SerializeField] public int NetCarbon {
        get{
            return netCarbon;
        }
        set{
            if(netCarbon != value){
                netCarbon = value;
                GameEventManager.current.GetEvent(EventType.E.NetCarbonUpdated).Invoke();
            }
        }
    }
    private int netMoney = 0;
    [SerializeField] public int NetMoney {
        get{
            return netMoney;
        }
        set{
            if(netMoney != value){
                netMoney = value;
                GameEventManager.current.GetEvent(EventType.E.NetMoneyUpdated).Invoke();
            }
        }
    }

    

    public static UnityEvent tileConnectionReset;
    [Header("Game Speed and Limits")]
    public float secBetweenYears = 4;//time between ticks in seconds
    public UnityEvent Tick { get; private set; }
    
    float timer;

    //At runtime, these floats store the highest amount of each that any tile makes // Updated from each tile script
    public float currentMaxTileIncome = 0f;
    public float currentMaxTileCarbon = 0f;
    public float currentMinTileCarbon = 0f;



    
    public float getMaxCarbon(){
        return maxCarbon;
    }

    public void setMaxCarbon(int _maxCarbon){
        maxCarbon = _maxCarbon;
    }


    // The next six functions keep track of the tiles with largest and smallest incomes & carbon production
    // levels. They have an obscure use in determining what size the over-object UI pop ups should be.
    public float getCurrentMaxTileIncome(){
        return currentMaxTileIncome;
    }

    public float getCurrentMaxTileCarbon(){
        return currentMaxTileCarbon;
    }

    public float getCurrentMinTileCarbon(){
        return currentMinTileCarbon;
    }

    public void setCurrentMaxTileIncome(float _currentMaxTileIncome){
        currentMaxTileIncome = _currentMaxTileIncome;
    }

    public void setCurrentMaxTileCarbon(float _currentMaxTileCarbon){
        currentMaxTileCarbon = _currentMaxTileCarbon;
    }

    public void setCurrentMinTileCarbon(float _currentMinTileCarbon){
        currentMinTileCarbon = _currentMinTileCarbon;
    }


    
    public void AdjustNetMoney(int _adjustNetMoney){
        NetMoney += _adjustNetMoney;
    }

    public void AdjustNetCarbon(int _adjustNetCarbon){

        NetCarbon += _adjustNetCarbon;

    }

    private void Awake()
    {
        LoadManager();
        TickManager.TM.PollutionTick.AddListener(OnPollutionTick);
        TickManager.TM.MoneyTick.AddListener(OnMoneyTick);
    }

    //Moved these functions to Start() from Awake() because resetData tries to reference Tile Select Panel.TSP before it's initialized
    void Start(){
        ResetData();
        tileConnectionReset = new UnityEvent();
    }

    private void OnMoneyTick(){
        GetComponent<UIPopUps>().displayMoneyPopUps();
        AdjustMoney(NetMoney);
    }


    private void OnPollutionTick(){
        AdjustCarbon(NetCarbon);
        GetComponent<UIPopUps>().displayCarbonPopUps();
    }


    public static bool overMaxCarbon(){
        return (LM.carbon >= LM.maxCarbon);
    }


    /// <summary>
    /// Sets the data values to their default values.
    /// </summary>
    public void ResetData()
    {
        SetMoney(startingMoney);
        carbon = startingCarbon;
        year = startingYear;
        storageCapacity = 0;
        stored = 0;
    }

    /// <summary>
    /// Increase or decrease the current simulation money balance. 
    /// Positive value adds, negative value subtracts.
    /// </summary>
    /// <param name="value"></param>
    public void AdjustMoney(int value)
    {
        SetMoney(GetMoney() + value);
        if(value > 0){
            GameEventManager.current.GetEvent(EventType.E.NewMoney).Invoke();
        }
    }

    /// <summary>
    /// Increase or decrease the current simulation carbon presence. 
    /// Positive value adds, negative value subtracts.
    /// </summary>
    /// <param name="value"></param>
    public void AdjustCarbon(int value)
    {
        //Caps the amount of carbon that can accumulate at maxCarbon +  carbonCapPastMax
        if(carbon >= (carbonCapPastMax + maxCarbon)){ // Carbon Maxes out
            if(value < 0){
                carbon += value;
                if (carbon < 0) carbon = 0;
            }
            GameEventManager.current.GetEvent(EventType.E.MaxCarbonAmountHit).Invoke();
        } else{ //Carbon increases a normal amount
            
            carbon += value;

            if (carbon < 0) carbon = 0;
            if(carbon > (carbonCapPastMax + maxCarbon)) carbon = carbonCapPastMax + maxCarbon;
        }

    }



    //Obsolete -- TODO: Remove eventually
    public void IncrementYear()
    {
        year++;
    }

    /// <summary>
    /// Returns the current money balance of the simulation.
    /// </summary>
    /// <returns></returns>
    
    public int GetStartingMoney(){
        return startingMoney;
    }

    public int GetMoney(){
        return money;
    }

    public void SetMoney(int _money){
        money = _money;
        GameEventManager.current.GetEvent(EventType.E.MoneyAmountUpdated).Invoke();
    }

    public void SetCarbon(int _carbon){
        carbon = _carbon;
    }

    /// <summary>
    /// Returns the current carbon value of the simulation.
    /// </summary>
    /// <returns></returns>
    public int GetCarbon()
    {
        return carbon;
    }


}

