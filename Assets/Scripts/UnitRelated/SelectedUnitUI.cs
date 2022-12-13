using System;
using UnityEngine;

public class SelectedUnitUI : MonoBehaviour
{
    
    void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        
        UpdateVisibility();
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        UpdateVisibility();
    }

    private void UpdateVisibility()
    {
        Unit unit = UnitActionSystem.Instance.GetSelectedUnit();

        bool shouldBeVisible = unit != null;
        gameObject.SetActive(shouldBeVisible);
    }

}
