using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    #region Singleton
    static public DataManager DM { get { return dm; } }
    static private DataManager dm;

    void LoadManager()
    {
        if (dm == null)
            dm = this;
        else
            Destroy(this.gameObject);
    }
    #endregion

    [SerializeField] int startingMoney;
    [SerializeField] int startingCarbon;
    [SerializeField] int startingYear;
    [SerializeField] int money;
    [SerializeField] int yearlyMoney;
    [SerializeField] int yearlyCarbon;
    [SerializeField] int carbon;
    [SerializeField] int year;
    [SerializeField] int storageCapacity;
    [SerializeField] int stored;

    private void Awake()
    {
        LoadManager();
        ResetData();
    }

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
        print("Carbon Update Called");
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
        //Debug.Log("Carbon Added:" + yearlyCarbon + "\n");
        //year++;
        //AdjustCarbon(yearlyCarbon);
        //AdjustStored(yearlyCarbon);
        //AdjustMoney(yearlyMoney);
        //Debug.Log("CurrentCarbon:" + carbon);
        //Debug.Log("\nCurrentCarbonStored:" + stored);
    }

    #region Getters and Setters

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

    #endregion
}


// Plan is to eventually make a priority queue for different requests. I.E. Processing all money requests, then carbon storage, then carbon additions, etc
// This will also allow us to track what type of source these requests are made from, highly benefiting the contract system
class ModifyRequest
{

}
