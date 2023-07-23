using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{

    static public DataManager DM { get { return dm; } }
    static private DataManager dm;

    void LoadManager()
    {
        if (dm == null)
            dm = this;
        else
            Destroy(this.gameObject);
    }


    [SerializeField] float startingMoney;
    [SerializeField] float startingCarbon;
    public float money;
    public float carbon;

    private void Awake()
    {
        LoadManager();
    }
}
