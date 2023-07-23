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

    public UnityEvent tick { get; private set; }
    float timer;
    float secBetweenYears = 4;

    private void Awake()
    {
        LoadManager();
        tick = new UnityEvent();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > secBetweenYears)
        {
            timer = 0;
            tick.Invoke();
        }
    }
}
