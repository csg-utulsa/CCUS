using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaskUIControl : MonoBehaviour
{
    //This is the updated contract manager to fit the new scope
    //DATE: 10/17/2023
    public ContractComplete contractComplete; //Controls the checkbox for if the task is complete
    public TextMeshProUGUI contractText; //Controls the text for the tasks
    public ContractExample contractExample; //Contains all the data for the tasks
}
