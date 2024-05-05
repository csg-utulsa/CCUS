using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TickManager : MonoBehaviour
{

    #region Singleton
    static public TickManager TM { get { return tm; } }
    static private TickManager tm;

    void LoadManager()
    {
        if (tm == null)
            tm = this;
        else
            Destroy(this.gameObject);
    }
    #endregion

    /// <summary>
    /// UnityEvent that is called every time a year is incremented in the simulation.
    /// </summary>
    public UnityEvent Tick { get; private set; }
    float timer;
    public float secBetweenYears = 4;//time between ticks in seconds

    private void Awake()
    {
        LoadManager();
        Tick = new UnityEvent();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > secBetweenYears)
        {
            timer = 0;
            Tick.Invoke();
            LevelManager.LM.IncrementYear();
        }
    }

    
}
