/*
 * Scriptable object for a contract that can be used by the contract manager
 */ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ContractData : ScriptableObject
{
    public string title;
    [TextArea(6,6)]
    public string fullDescription;

    public enum resourceOption { CARBON, MONEY }
    [Space(10)]
    public resourceOption goalResource;
    public float goalAmount;
    public string goalDescription;
    public string goalUnits;

    [Space(10)]
    public resourceOption rewardResource;
    public int reward;

    // TODO: Add field for sprite and name of person who gave the contract
    // TODO (optional): Add time limit field
}
