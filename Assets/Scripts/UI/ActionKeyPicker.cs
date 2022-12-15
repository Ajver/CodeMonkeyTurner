using System;
using UnityEngine;

public class ActionKeyPicker : MonoBehaviour
{
    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;

        EnableOrDisable();
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        EnableOrDisable();
    }

    private void EnableOrDisable()
    {
        bool shouldBeEnabled = UnitActionSystem.Instance.GetSelectedUnit() != null;
        enabled = shouldBeEnabled;
    }
    
    private void Update()
    {
        int actionIdx = InputManager.Instance.GetKeySelectedActionIndex();
        
        if (actionIdx == -1)
        {
            return;
        }

        Unit unit = UnitActionSystem.Instance.GetSelectedUnit();
        BaseAction[] actions = unit.GetBaseActionsArray();

        if (actionIdx < actions.Length)
        {
            UnitActionSystem.Instance.SetSelectedAction(actions[actionIdx]);
        }
        else
        {
            Debug.LogError($"Action Index out of bounds! index: {actionIdx}");
        }
    }
    
}
