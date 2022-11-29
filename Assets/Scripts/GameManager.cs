using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    public event EventHandler OnGameEnded;
    
    private bool isGameEnded;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        MissionSystem.Instance.OnMissionComplete += MissionSystem_OnMissionComplete;
        MissionSystem.Instance.OnMissionFailed += MissionSystem_OnMissionFailed;
    }

    private void MissionSystem_OnMissionComplete(object sender, EventArgs e)
    {
        EndGame();
    }
    
    private void MissionSystem_OnMissionFailed(object sender, EventArgs e)
    {
        EndGame();
    }

    private void EndGame()
    {
        isGameEnded = true;
        OnGameEnded?.Invoke(this, EventArgs.Empty);
    }

    public bool IsGameEnded()
    {
        return isGameEnded;
    }
}
