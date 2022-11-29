using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionCompleteUI : MonoBehaviour
{
    private void Start()
    {
        MissionSystem.Instance.OnMissionComplete += MissionSystem_OnMissionComplete;
        
        gameObject.SetActive(false);
    }

    public void OnContinueBtnClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void MissionSystem_OnMissionComplete(object sender, EventArgs e)
    {
        gameObject.SetActive(true);
    }

}
