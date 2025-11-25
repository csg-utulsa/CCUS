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
    [SerializeField] int startingMoney;
    [SerializeField] int startingCarbon;
    [SerializeField] int startingYear;
    [Header("Current Stats")]
    public GameState levelState = GameState.Inactive;
    [SerializeField] private int money;
    [SerializeField] int yearlyMoney;
    [SerializeField] int yearlyCarbon;
    [SerializeField] int carbon;
    [SerializeField] int year;
    [SerializeField] int storageCapacity;
    [SerializeField] int stored;

    public static UnityEvent tileConnectionReset;
    [Header("Game Speed and Limits")]
    public float secBetweenYears = 4;//time between ticks in seconds
    public UnityEvent Tick { get; private set; }
    
    float timer;

    //At runtime, these floats store the highest amount of each that any tile makes
    public float currentMaxTileIncome = 0f;
    public float currentMaxTileCarbon = 0f;
    public float currentMinTileCarbon = 0f;

    
    public float getMaxCarbon(){
        return maxCarbon;
    }

    public void setMaxCarbon(int _maxCarbon){
        maxCarbon = _maxCarbon;
    }

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
    

    private void Awake()
    {
        LoadManager();
        
        //Tick = new UnityEvent();

        levelState = GameState.Active;

        TickManager.TM.PollutionTick.AddListener(OnPollutionTick);
        TickManager.TM.MoneyTick.AddListener(OnMoneyTick);
    }

    //Moved these functions to Start() from Awake() because resetData tries to reference Tile Select Panel.TSP before it's initialized
    void Start(){
        ResetData();
        tileConnectionReset = new UnityEvent();
    }

    private void Update()
    {
        #region Tick Timer
        // if (levelState == GameState.Active)//time should only pass when the game is in an Active State
        // {
        //     timer += Time.deltaTime;
        //     if (timer > secBetweenYears)
        //     {
        //         timer = 0;
        //         Tick.Invoke();
        //         IncrementYear();
        //     }
        // }
        #endregion
    }

    public void OnMoneyTick(){
        GetComponent<UIPopUps>().displayMoneyPopUps();
        StartCoroutine(endOfMoneyTick());
    }
    //Runs at the end of each money tick
    IEnumerator endOfMoneyTick(){
        yield return null;
        TileSelectPanel.TSP.checkPricesOfTiles(money);
    }

    public void OnPollutionTick(){
        GetComponent<UIPopUps>().displayCarbonPopUps();
        StartCoroutine(endOfPollutionTick());
    }
    //Runs at the end of each money tick
    IEnumerator endOfPollutionTick(){
        yield return null;
        if(carbon > maxCarbon){
            TileSelectPanel.TSP.disablePolluters();
        } else {
            TileSelectPanel.TSP.enablePolluters();
        }
        
    }

    public static bool overMaxCarbon(){
        return (LM.carbon > LM.maxCarbon);
    }





    #region Stat Get/Set/Adjust
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
    }

    public void AdjustYearlyIncome(int moneyChange)
    {
        yearlyMoney += moneyChange;
    }
    /// <summary>
    /// Increase or decrease the current simulation carbon presence. 
    /// Positive value adds, negative value subtracts.
    /// </summary>
    /// <param name="value"></param>
    public void AdjustCarbon(int value)
    {
        //carbonChangedSinceUpdate = true;
        //print("Carbon Update Called");
        carbon += value;
        if (carbon < 0) carbon = 0;
    }

    /// <summary>
    /// Increase or decrease the current simulation yearly carbon change. 
    /// Positive value adds, negative value subtracts.
    /// </summary>
    /// <param name="value"></param>
    public void AdjustYearlyCarbon(int value)
    {
        print("Carbon Yearly Update Called");
        yearlyCarbon += value;
    }

    /// <summary>
    /// Increase or decrease the current simulation carbon storage size. If decreased, release all carbon over capacity.
    /// Positive value adds, negative value subtracts.
    /// </summary>
    /// <param name="value"></param>
    public void AdjustStorageSize(int value)
    {
        storageCapacity += value;

        // If storage size decreased, release over capacity carbon into the atmosphere
        if (stored > storageCapacity)
        {
            AdjustCarbon(stored - storageCapacity);
            SetStored(storageCapacity);
        }

    }

    /// <summary>
    /// Increase or decrease the current amonut of carbon stored. If the amount of carbon added goes over the storage capacity, the carbon is released rather than 
    /// stored. 
    /// Positive value adds, negative value subtracts.
    /// </summary>
    /// <param name="value"></param>
    public void AdjustStored(int value)
    {
        if (stored + value > storageCapacity)
        {
            value = storageCapacity - stored;
            SetStored(storageCapacity);
            AdjustCarbon(value);
        }
        stored += value;

        if(stored <0) stored = 0;
    }

    /// <summary>
    /// Increments the year counter by one.
    /// </summary>
    public void IncrementYear()
    {
        year++;
    }

    /// <summary>
    /// Returns the current money balance of the simulation.
    /// </summary>
    /// <returns></returns>
    
    public int GetMoney(){
        return money;
    }

    public void SetMoney(int _money){
        money = _money;
        TileSelectPanel.TSP.checkPricesOfTiles(GetMoney());
    }

    /// <summary>
    /// Returns the current carbon value of the simulation.
    /// </summary>
    /// <returns></returns>
    public int GetCarbon()
    {
        return carbon;
    }

    /// <summary>
    /// Returns the current year of the simulation.
    /// </summary>
    /// <returns></returns>
    public int GetYear()
    {
        return year;
    }

    /// <summary>
    /// Returns the current amount of stored carbon of the simulation.
    /// </summary>
    /// <returns></returns>
    public int GetStored()
    {
        return stored;
    }

    /// <summary>
    /// Sets the current amount of stored carbon of the simulation.
    /// </summary>
    /// <returns></returns>
    public void SetStored(int val)
    {
        stored = val;
    }

    /// <summary>
    /// Sets GameState of current scene
    /// </summary>
    public void SetLevelState(GameState newState)
    {
        levelState = newState;
    }
    #endregion

    public enum GameState {Active, Inactive, Pause, Lose, Win}
}


// Plan is to eventually make a priority queue for different requests. I.E. Processing all money requests, then carbon storage, then carbon additions, etc
// This will also allow us to track what type of source these requests are made from, highly benefiting the contract system
class ModifyRequest
{

}
