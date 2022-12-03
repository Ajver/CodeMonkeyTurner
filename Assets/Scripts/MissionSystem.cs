using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionSystem : MonoBehaviour
{
    public enum MissionCompleteReason
    {
        CollectTreasure,
        KillEnemies,
    }
    
    public enum MissionFailReason
    {
        AllUnitsDied,
        TreasureDestroyed,
    }
    
    public event EventHandler<MissionCompleteReason> OnMissionComplete;
    public event EventHandler<MissionFailReason> OnMissionFailed;

    [SerializeField] private MissionCompleteReason missionGoal;
    
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
        UnitManager.Instance.OnAllEnemyUnitsDied += UnitManager_OnAllEnemyUnitsDied;
    }

    private void TableWithSuitcase_OnAnyTreasureCollected(object sender, EventArgs e)
    {
        if (missionGoal == MissionCompleteReason.CollectTreasure)
        {
            OnMissionComplete?.Invoke(this, missionGoal);
        }
    }

    private void UnitManager_OnAllEnemyUnitsDied(object sender, EventArgs e)
    {
        if (missionGoal == MissionCompleteReason.KillEnemies)
        {
            OnMissionComplete?.Invoke(this, missionGoal);
        }
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
        if (missionGoal == MissionCompleteReason.CollectTreasure)
        {
            // If it's not the goal of the mission - don't fail
            FailMission(MissionFailReason.TreasureDestroyed);
        }
    }
    
    private void FailMission(MissionFailReason reason)
    {
        OnMissionFailed?.Invoke(this, reason);
    }

    public MissionCompleteReason GetMissionGoal()
    {
        return missionGoal;
    }
}
