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

    [SerializeField] float startingMoney;
    [SerializeField] float startingCarbon;
    [SerializeField] int startingYear;
    float money;
    float carbon;
    int year;

    private void Awake()
    {
        LoadManager();
        ResetData();
    }

    /// <summary>
    /// Sets the values of money and carbon to their default values.
    /// </summary>
    public void ResetData()
    {
        money = startingMoney;
        carbon = startingCarbon;
        year = startingYear;
    }

    /// <summary>
    /// Increase or decrease the current simulation money balance. Positive value adds, negative value subtracts.
    /// </summary>
    /// <param name="value"></param>
    public void AddMoney(float value)
    {
        money += value;
    }

    /// <summary>
    /// Increase or decrease the current simulation carbon presence. Positive value adds, negative value subtracts.
    /// </summary>
    /// <param name="value"></param>
    public void AddCarbon(float value)
    {
        carbon += value;
    }

    /// <summary>
    /// Increments the year counter by one.
    /// </summary>
    public void IncrementYear()
    {
        year++;
    }

    #region Getters and Setters

    /// <summary>
    /// Returns the current money balance of the simulation.
    /// </summary>
    /// <returns></returns>
    public float GetMoney()
    {
        return money;
    }

    /// <summary>
    /// Returns the current carbon value of the simulation.
    /// </summary>
    /// <returns></returns>
    public float GetCarbon()
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

    #endregion
}
