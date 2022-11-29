using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    
    private void Start()
    {
        GameManager.Instance.OnGameEnded += GameManager_OnGameEnded;
    }

    private void GameManager_OnGameEnded(object sender, EventArgs e)
    {
        gameObject.SetActive(false);
    }
    
}
