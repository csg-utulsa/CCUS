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
    [SerializeField] int startingMoney;
    [SerializeField] int startingCarbon;
    [SerializeField] int startingYear;
    [Header("Current Stats")]
    public GameState levelState = GameState.Inactive;
    [SerializeField] int money;
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
    

    private void Awake()
    {
        LoadManager();
        ResetData();
        tileConnectionReset = new UnityEvent();
        Tick = new UnityEvent();
        levelState = GameState.Active;
    }

    private void Update()
    {
        #region Tick Timer
        if (levelState == GameState.Active)//time should only pass when the game is in an Active State
        {
            timer += Time.deltaTime;
            if (timer > secBetweenYears)
            {
                timer = 0;
                Tick.Invoke();
                IncrementYear();
            }
        }
        #endregion
    }

    #region Stat Get/Set/Adjust
    /// <summary>
    /// Sets the data values to their default values.
    /// </summary>
    public void ResetData()
    {
        money = startingMoney;
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
        money += value;
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
    public int GetMoney()
    {
        return money;
    }

    public void SetMoney(int value)
    {
        money = value;
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
