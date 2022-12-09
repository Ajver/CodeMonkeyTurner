using System;
using UnityEngine;

public class MenuStateController : MonoBehaviour
{
    public static MenuStateController Instance { get; private set; }

    public event EventHandler OnMainMenuScreenEntered;
    public event EventHandler OnMissionPickEntered;

    private void Awake()
    {
        Instance = this;
    }

    public void OnStartCamaignBtnPressed()
    {
        OnMissionPickEntered?.Invoke(this, EventArgs.Empty);
    }

    public void OnBackBtnPressed()
    {
        OnMainMenuScreenEntered?.Invoke(this, EventArgs.Empty);
    }

    public void OnExitGameBtnPressed()
    {
        Debug.Log("Quiting game...");
        Application.Quit();
    }
    
}
