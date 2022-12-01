using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionSystem : MonoBehaviour
{
    public enum MissionCompleteReason
    {
        CollectedTreasure,
        KilledAllEnemies,
    }
    
    public enum MissionFailReason
    {
        AllUnitsDied,
        TreasureDestroyed,
    }
    
    public event EventHandler<MissionCompleteReason> OnMissionComplete;
    public event EventHandler<MissionFailReason> OnMissionFailed;
    
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
        OnMissionComplete?.Invoke(this, MissionCompleteReason.CollectedTreasure);
    }

    private void OnDestroy()
    {
        TableWithSuitcase.OnAnyTreasureCollected -= TableWithSuitcase_OnAnyTreasureCollected;
        TableWithSuitcase.OnAnyTreasureDestroyed -= TableWithSuitcase_OnAnyTreasureDestroyed;
    }

    private void UnitManager_OnAllFriendlyUnitsDied(object sender, EventArgs e)
    {
        FailMission(MissionFailReason.AllUnitsDied);
    }

    private void TableWithSuitcase_OnAnyTreasureDestroyed(object sender, EventArgs e)
    {
        FailMission(MissionFailReason.TreasureDestroyed);
    }
    
    private void FailMission(MissionFailReason reason)
    {
        OnMissionFailed?.Invoke(this, reason);
    }
}
