/*
 * Manages placing contracts in the UI, updating their status 
 * 
 */ 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractManager : MonoBehaviour
{
    
    public List<Contract> availableContracts; // List of all contracts available in the level, set in inspector
    
    [SerializeField]
    private List<ContractData> activeContracts; // List containing every contract currently active, set at runtime

    public int maxActiveContracts = 3;

    // Adds a contract to the contract UI panel
    private void AddContractToUI(ContractData contract)
    {
        // TODO: AddContractToUI
    }

    // Removes a contract from the contract UI panel
    private void RemoveContractFromUI(ContractData contract)
    {
        // TODO: RemoveContractFromUI
    }

     // Overload to grab a contract from the available contracts list at a given index
    public void MakeContractActive(int index)
    {
        MakeContractActive(availableContracts[index]);
    }

    // Takes a ContractData object and adds it to the contract UI if there's space
    public void MakeContractActive(ContractData contract)
    {
        if(maxActiveContracts < activeContracts.Count)
        {
            AddContractToUI(contract);
            activeContracts.Add(contract);
        }
        
    }

    // Removes a contract from the active contracts list at the given index and removes it from the UI
    public void MakeContractInactive(int index)
    {
        if (index < availableContracts.Count)
        {
            RemoveContractFromUI(activeContracts[index]);
            activeContracts.RemoveAt(index);
        }
    }

    // Removes the given contract from the active contracts list and removes it from the UI
    public void MakeContractInactive(ContractData contract)
    {
        if(activeContracts.Contains(contract))
        {
            RemoveContractFromUI(contract);
            activeContracts.Remove(contract);
        }
    }
    
    // TODO: Add functionality to check if contracts are complete when receiving resource updates
}
