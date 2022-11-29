using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionSystem : MonoBehaviour
{
    public event EventHandler OnMissionComplete;
    
    public static MissionSystem Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Debug.Log("Start");
        TableWithSuitcase.OnAnyTreasureCollected += TableWithSuitcase_OnAnyTreasureCollected;
    }

    private void TableWithSuitcase_OnAnyTreasureCollected(object sender, EventArgs e)
    {
        Debug.Log("Treasure collected...");
        OnMissionComplete?.Invoke(this, EventArgs.Empty);
    }

    private void OnDestroy()
    {
        TableWithSuitcase.OnAnyTreasureCollected -= TableWithSuitcase_OnAnyTreasureCollected;
    }
}
