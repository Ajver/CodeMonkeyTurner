using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    
    [SerializeField] private string mainMenuSceneName;
    [SerializeField] private Animator animator;

    private void Start()
    {
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        GameManager.Instance.OnGameResumed += GameManager_OnGameResumed;
        
        // This needs to be in Start instead of Awake, so it has time to connect to GameManager
        DeactivateGameObject();
    }

    private void GameManager_OnGamePaused(object sender, EventArgs e)
    {
        Show();
    }
    
    private void GameManager_OnGameResumed(object sender, EventArgs e)
    {
        Hide();
    }
    
    public void OnContinueBtnClicked()
    {
        GameManager.Instance.ResumeGame();
    }

    public void OnRestartMissionBtnClicked()
    {
        Time.timeScale = 1f;
        SceneFader.Instance.FadeToScene(SceneManager.GetActiveScene().name);
    }

    public void OnReturnToMenuBtnClicked()
    {
        Time.timeScale = 1f;
        SceneFader.Instance.FadeToScene(mainMenuSceneName);
    }

    private void Show()
    {
        gameObject.SetActive(true);
        animator.SetBool("IsVisible", true);
    }
    
    private void Hide()
    {
        animator.SetBool("IsVisible", false);
    }

    // Called at the end of Fade Out animation
    public void DeactivateGameObject()
    {
        gameObject.SetActive(false);
    }
    
}
