/*
 * Manages placing contracts in the UI, updating their status 
 * 
 */ 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractManager : MonoBehaviour
{
    [SerializeField]
    private List<ContractData> availableContracts; // List of all contracts available in the level, set in inspector
    
    [SerializeField]
    private List<ContractData> activeContracts; // List containing every contract currently active, set at runtime

    [SerializeField]
    private List<ContractUIElement> contractUIElements; // List of all the contract UI elements, set in inspector

    public int maxActiveContracts = 3;

    public void Start()
    {
        if(availableContracts.Count > 0)
        {
            MakeContractActive(0);
        }
    }

    // Overload to grab a contract from the available contracts list at a given index
    public void MakeContractActive(int index)
    {
        MakeContractActive(availableContracts[index]);
    }

    // Takes a ContractData object and adds it to the contract UI if there's space
    public void MakeContractActive(ContractData contract)
    {
        if(maxActiveContracts > activeContracts.Count)
        {
            contractUIElements[activeContracts.Count].AddContract(contract);
            activeContracts.Add(contract);
        }
        
    }

    // Removes a contract from the active contracts list at the given index and removes it from the UI
    public void MakeContractInactive(int index)
    {
        if (index < availableContracts.Count)
        {
            contractUIElements[index].RemoveContract();
            activeContracts.RemoveAt(index);
        }
    }

    // Removes the given contract from the active contracts list and removes it from the UI
    public void MakeContractInactive(ContractData contract)
    {
        if(activeContracts.Contains(contract))
        {
            contractUIElements[activeContracts.IndexOf(contract)].RemoveContract();
            activeContracts.Remove(contract);
        }
    }
    
    // TODO: Add functionality to pass resource updates to contract UI and check if contracts are completed
}
