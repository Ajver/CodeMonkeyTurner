using System;
using UnityEngine;

public class ActionBusyUI : MonoBehaviour
{
    void Start()
    {
        UnitActionSystem.Instance.OnBusyChanged += UnitActionSystem_OnBusyChanged;
        UpdateVisual(false);
    }

    private void UnitActionSystem_OnBusyChanged(object sender, bool isBusy)
    {
        UpdateVisual(isBusy);
    }
    
    private void UpdateVisual(bool show)
    {
        gameObject.SetActive(show);
    }
    
}
