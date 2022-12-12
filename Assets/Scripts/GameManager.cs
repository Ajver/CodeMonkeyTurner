using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    public event EventHandler OnGamePaused;
    public event EventHandler OnGameResumed;
    public event EventHandler OnGameEnded;

    private bool isGamePaused;
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

    private void Update()
    {
        if (isGameEnded)
        {
            return;
        }
        
        if (InputManager.Instance.IsPauseButtonPressedThisFrame())
        {
            ToggleGamePaused();
        }
    }

    private void ToggleGamePaused()
    {
        if (isGamePaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    // This method is public, so it can be called from Pause UI
    public void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0f;
        OnGamePaused?.Invoke(this, EventArgs.Empty);
    }
    
    // This method is public, so it can be called from UI
    public void ResumeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1f;
        OnGameResumed?.Invoke(this, EventArgs.Empty);
    }
    
    private void MissionSystem_OnMissionComplete(object sender, MissionSystem.MissionCompleteReason reason)
    {
        EndGame();
    }
    
    private void MissionSystem_OnMissionFailed(object sender, MissionSystem.MissionFailReason reason)
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
