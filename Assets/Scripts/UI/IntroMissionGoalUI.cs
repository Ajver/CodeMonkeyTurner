using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IntroMissionGoalUI : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI missionDescriptionText;
    [SerializeField] private Animator animator;

    [SerializeField] private GameObject killAllEnemiesVisual;
    [SerializeField] private GameObject collectSuitcaseVisual;

    private void Start()
    {
        // By default disable all goal visuals 
        killAllEnemiesVisual.SetActive(false);
        collectSuitcaseVisual.SetActive(false);

        switch (MissionSystem.Instance.GetMissionGoal())
        {
            case MissionSystem.MissionCompleteReason.CollectTreasure:
                SetCollectTreasureDescription();
                break;
            case MissionSystem.MissionCompleteReason.KillEnemies:
                SetKillAllEnemiesDescription();
                break;
            default:
                throw new Exception("Unknown mission goal");
        }
    }

    private void SetCollectTreasureDescription()
    {
        missionDescriptionText.text = "Find and collect suitcase with money. Don't lose it!";
        collectSuitcaseVisual.SetActive(true);
    }

    private void SetKillAllEnemiesDescription()
    {
        missionDescriptionText.text = "You need to kill all enemies!";
        killAllEnemiesVisual.SetActive(true);
    }
    
    public void Hide()
    {
        animator.SetBool("IsVisible", false);
    }

    public void ActivateGameObject()
    {
        gameObject.SetActive(true);
    }
    
    public void DeactivateGameObject()
    {
        gameObject.SetActive(false);
    }
}
