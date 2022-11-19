using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystemUI : MonoBehaviour
{

    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;

    private List<ActionButtonUI> actionButtonsUi;

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        CreateUnitActionButtons();
    }

    private void CreateUnitActionButtons()
    {
        foreach (Transform buttonTransform in actionButtonContainerTransform)
        {
            Destroy(buttonTransform.gameObject);
        }

        actionButtonsUi = new List<ActionButtonUI>();
        
        Unit unit = UnitActionSystem.Instance.GetSelectedUnit();

        Debug.Log("Selected unit: " + unit);
        
        // BaseAction[] allActions = ;
        // Debug.Log("All actions:\n" + allActions);
        foreach (BaseAction action in unit.GetBaseActionsArray())
        {
            Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
            ActionButtonUI button = actionButtonTransform.GetComponent<ActionButtonUI>();
            button.SetBaseAction(action);
            actionButtonsUi.Add(button);
        }
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        CreateUnitActionButtons();
    }
    
    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateButtonsSelectedVisuals();
    }

    private void UpdateButtonsSelectedVisuals()
    {
        foreach (ActionButtonUI button in actionButtonsUi)
        {
            button.UpdateSelectedVisual();
        }
    }
    
}
