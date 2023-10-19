using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public int goal;
    public string type;
    private string stringCall = "Carbon";
    [SerializeField] ContractComplete checkBox;
    DataManager dataM;

    void Start()
    {
        dataM = DataManager.DM;
        stringCall = type;
    }

    void Update()
    {
        Invoke(stringCall,0f);
    }

    public void Carbon()
    {
        CompletionCheckLess(dataM.GetCarbon());
    }

    public void Money()
    {
        CompletionCheckLess(dataM.GetMoney());
    }

    public void Year()
    {
        CompletionCheckEqual(dataM.GetYear());
    }

    public void CompletionCheckEqual(int current)
    {
        if (goal == current)
        {
            checkBox.StarUpdate();
        }
    }

    public void CompletionCheckLess(int current)
    {
        if( goal >= current)
        {
            checkBox.StarUpdate();
        }
    }
}
