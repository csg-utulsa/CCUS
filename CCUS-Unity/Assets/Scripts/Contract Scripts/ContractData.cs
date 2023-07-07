/*
 * Scriptable object for a contract that can be used by the contract manager
 */ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractData : ScriptableObject
{
    public string title;
    public string fullDescription;

    public string goalResource;
    public float goalAmount;
    public string goalDescription;
    public string goalUnits;

    public float reward;

    // TODO: Add field for sprite and name of person who gave the contract
    // TODO (optional): Add time limit field
}
