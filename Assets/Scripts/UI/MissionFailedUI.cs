using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionFailedUI : MonoBehaviour
{
    private void Start()
    {
        MissionSystem.Instance.OnMissionFailed += MissionSystem_OnMissionFailed;
        
        gameObject.SetActive(false);
    }

    public void OnRetryBtnClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void MissionSystem_OnMissionFailed(object sender, EventArgs e)
    {
        gameObject.SetActive(true);
    }
}
