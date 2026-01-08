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
    public UnityEvent PollutionTick { get; private set; }
    public UnityEvent MoneyTick { get; private set; }
    public UnityEvent EndOfMoneyAndPollutionTicks { get; private set; }
    float timer;
    float separateTickTimer = 0f;
    public float secBetweenYears = 4;//time between ticks in seconds
    int tickType = 0;

    private void Awake()
    {
        LoadManager();
        Tick = new UnityEvent();
        PollutionTick = new UnityEvent();
        MoneyTick = new UnityEvent();
        EndOfMoneyAndPollutionTicks = new UnityEvent();
    }

    private void Update()
    {
        //Legacy Ticks (REMOVE EVENTUALLY)
        timer += Time.deltaTime;
        if (timer > secBetweenYears)
        {
            timer = 0;
            Tick.Invoke();
            LevelManager.LM.IncrementYear();
        }

        //Executes separate ticks for money and pollution
        separateTickTimer += Time.deltaTime;
        if (separateTickTimer > secBetweenYears){
            if(tickType == 0){
                tickType++;
                MoneyTick.Invoke();
                EndOfMoneyAndPollutionTicks.Invoke();
            } else{
                tickType = 0;
                PollutionTick.Invoke();
                EndOfMoneyAndPollutionTicks.Invoke();
            }
            separateTickTimer = 0f;
        }

    }

    // IEnumerator endOfMoneyAndPollutionTicks(){
    //     yield return null;
    //     checkPlaceabilityOfTiles();
    // }

    
}
