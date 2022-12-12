
using System;
using Cinemachine;
using UnityEngine;

public class MenuMissionPickVirtualCamera : MonoBehaviour
{

    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private void Awake()
    {
        virtualCamera.enabled = MenuStateStore.inMissionPick;
    }

    private void Start()
    {
        MenuStateController.Instance.OnMissionPickEntered += MenuStateController_OnMissionPickEntered;
        MenuStateController.Instance.OnMainMenuScreenEntered += MenuStateController_OnMainMenuScreenEntered;
    }

    private void MenuStateController_OnMissionPickEntered(object sender, EventArgs e)
    {
        virtualCamera.enabled = true;
    }
    
    private void MenuStateController_OnMainMenuScreenEntered(object sender, EventArgs e)
    {
        virtualCamera.enabled = false;
    }
    
}
