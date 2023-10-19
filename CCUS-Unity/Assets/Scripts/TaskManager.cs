using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public int goal;
    [SerializeField] ContractComplete checkBox;

    void Start()
    {

    }

    public void CompletionCheck(int current)
    {
        if (goal == current)
        {
            checkBox.StarUpdate();
        }
    }
}
