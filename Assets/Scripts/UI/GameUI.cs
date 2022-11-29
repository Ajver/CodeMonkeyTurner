using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    
    void Start()
    {
        MissionSystem.Instance.OnMissionComplete += MissionSystem_OnMissionComplete;
    }

    private void MissionSystem_OnMissionComplete(object sender, EventArgs e)
    {
        Debug.Log("Deactivating game UI");
        gameObject.SetActive(false);
    }
    
}
