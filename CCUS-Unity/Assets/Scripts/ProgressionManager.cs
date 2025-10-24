using System;
using System.Collections.Generic;
using UnityEngine;
/*
 * This class is intended to manage the progression of unlocking new features
 * in the game as the player advances through years or achieves certain milestones.
 */
public class ProgressionManager : MonoBehaviour
{
    [Header("UI References")]
    public TileSelectPanel tileSelectPanel;
    
    [Header("Milestones Configuration")]
    [SerializeField] private Milestone[] milestones;
    
    //Track processed milestones to avoid reprocessing
    private HashSet<Milestone> processedMilestones = new HashSet<Milestone>();

    private System.Collections.IEnumerator InitializeWhenReady()
    {
        //Wait until tileSelectPanel is assigned and its Start() has run
        yield return new WaitUntil(() => tileSelectPanel != null && tileSelectPanel.isActiveAndEnabled);

        InitializeMilestones();
    }

    private void InitializeMilestones()
    {
        //Initialize milestones that should be enabled at the start
        foreach (var milestone in milestones)
        {
            if (milestone.enableOnStart)
            {
                tileSelectPanel.AddButton(milestone.tileButton);
                AddToTSP(milestone);
            }
            else
            {
                //hide buttons not enabled at start
                milestone.tileButton.SetActive(false);
            }
        }
    }
    
    #region Unity Methods
    private void Start()
    {
        StartCoroutine(InitializeWhenReady());
    }
    
    void Update()
    {
        //Check for milestone conditions each frame
        foreach (var milestone in milestones)
        {
            if (processedMilestones.Contains(milestone))
                continue; //skip already processed milestones
            
            bool allConditionsMet = true;
            foreach (var condition in milestone.conditions)
            {
                if (!IsConditionMet(condition))
                {
                    allConditionsMet = false;
                    break;
                }
            }
            if (allConditionsMet)
            {
                AddToTSP(milestone);
            }
        }
    }
    #endregion
    
    private void AddToTSP(Milestone milestone)
    {
        milestone.tileButton.SetActive(true); //ensure button is visible
        processedMilestones.Add(milestone); //mark as processed
    }
    
    private bool IsConditionMet(MilestoneCondition condition)
    {
        switch (condition.type)
        {
            case MilestoneType.YearReached:
                return LevelManager.LM.GetYear() >= condition.threshold;
            case MilestoneType.CarbonLevel:
                return LevelManager.LM.GetCarbon() <= condition.threshold;
            case MilestoneType.MoneyEarned:
                return LevelManager.LM.GetMoney() >= condition.threshold;
            default:
                return false;
        }
    }
}

#region Helper Classes
[Serializable]
internal class Milestone
{
    [Tooltip("Object ref to the button prefab for tile")]
    public GameObject tileButton;
    
    [Tooltip("Condition(s) required to unlock this milestone")]
    public MilestoneCondition[] conditions;

    [Tooltip("Enable at the start of the game? (Ignores other conditions)")]
    public bool enableOnStart = false;
}

internal enum MilestoneType
{
    YearReached,
    CarbonLevel,
    MoneyEarned
}

[Serializable]
internal class MilestoneCondition
{
    public MilestoneType type;
    public int threshold;
}
#endregion
