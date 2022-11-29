using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionSystem : MonoBehaviour
{
    public event EventHandler OnMissionComplete;
    public event EventHandler OnMissionFailed;
    
    public static MissionSystem Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        TableWithSuitcase.OnAnyTreasureCollected += TableWithSuitcase_OnAnyTreasureCollected;
        TableWithSuitcase.OnAnyTreasureDestroyed += TableWithSuitcase_OnAnyTreasureDestroyed;

        UnitManager.Instance.OnAllFriendlyUnitsDied += UnitManager_OnAllFriendlyUnitsDied;
    }

    private void TableWithSuitcase_OnAnyTreasureCollected(object sender, EventArgs e)
    {
        Debug.Log("Treasure collected...");
        OnMissionComplete?.Invoke(this, EventArgs.Empty);
    }

    private void OnDestroy()
    {
        TableWithSuitcase.OnAnyTreasureCollected -= TableWithSuitcase_OnAnyTreasureCollected;
        TableWithSuitcase.OnAnyTreasureDestroyed -= TableWithSuitcase_OnAnyTreasureDestroyed;
    }

    private void UnitManager_OnAllFriendlyUnitsDied(object sender, EventArgs e)
    {
        FailMission();
    }

    private void TableWithSuitcase_OnAnyTreasureDestroyed(object sender, EventArgs e)
    {
        FailMission();
    }
    
    private void FailMission()
    {
        OnMissionFailed?.Invoke(this, EventArgs.Empty);
    }
}
