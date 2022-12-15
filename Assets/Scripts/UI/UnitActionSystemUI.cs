using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitActionSystemUI : MonoBehaviour
{

    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;
    [SerializeField] private TextMeshProUGUI actionPointsText;
    [SerializeField] private GameObject noUnitSelectedGameObject;
    
    [SerializeField] private GameObject currentUnitOutOfActionPointsUI;
    [SerializeField] private GameObject noUnitHasActionPointsUI;

    private List<ActionButtonUI> actionButtonsUi;

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        UnitActionSystem.Instance.OnActionStarted += UnitActionSystem_OnActionStarted;
        
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;

        if (UnitActionSystem.Instance.GetSelectedUnit() != null)
        {
            CreateUnitActionButtons();
            UpdateActionPoints();

            HideNoUnitSelectedUI();
        }
        else
        {
            ShowNoUnitSelectedUI();
        }
        
        HideNoUnitHasActionPointsUI();
        HideCurrentUnitOutOfActionPointsUI();
    }

    private void CreateUnitActionButtons()
    {
        foreach (Transform buttonTransform in actionButtonContainerTransform)
        {
            Destroy(buttonTransform.gameObject);
        }

        actionButtonsUi = new List<ActionButtonUI>();
        
        Unit unit = UnitActionSystem.Instance.GetSelectedUnit();
        
        int actionNumber = 1;

        foreach (BaseAction action in unit.GetBaseActionsArray())
        {
            Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
            ActionButtonUI button = actionButtonTransform.GetComponent<ActionButtonUI>();
            button.Setup(action, actionNumber);
            actionButtonsUi.Add(button);

            actionNumber++;
        }
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        if (UnitActionSystem.Instance.GetSelectedUnit() != null)
        {
            CreateUnitActionButtons();
            UpdateActionPoints();
            
            HideNoUnitSelectedUI();
        }
        else
        {
            ShowNoUnitSelectedUI();
        }

        UpdateNoActionPointsUIsVisibility();
    }

    private void ShowNoUnitSelectedUI()
    {
        noUnitSelectedGameObject.SetActive(true);
    }

    private void HideNoUnitSelectedUI()
    {
        noUnitSelectedGameObject.SetActive(false);
    }

    private void UpdateNoActionPointsUIsVisibility()
    {
        // By default hide both of them (then they are optionally shown)
        HideNoUnitHasActionPointsUI();
        HideCurrentUnitOutOfActionPointsUI();

        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            // Don't show these on Enemy turn
            return;
        }
        
        if (!HasAnyUnitWithActionPoints())
        {
            // Literally no action points on any unit
            ShowNoUnitHasActionPointsUI();
            return;
        }

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        if (selectedUnit != null && selectedUnit.GetActionPoints() <= 0)
        {
            ShowCurrentUnitOutOfActionPointsUI();
        }
    }

    private bool HasAnyUnitWithActionPoints()
    {
        foreach (Unit unit in UnitManager.Instance.GetFriendlyUnitList())
        {
            if (unit.GetActionPoints() > 0)
            {
                return true;
            }
        }

        return false;
    }
    
    private void ShowCurrentUnitOutOfActionPointsUI()
    {
        currentUnitOutOfActionPointsUI.SetActive(true);
    }
    
    private void HideCurrentUnitOutOfActionPointsUI()
    {
        currentUnitOutOfActionPointsUI.SetActive(false);
    }
    
    private void ShowNoUnitHasActionPointsUI()
    {
        noUnitHasActionPointsUI.SetActive(true);
    }
    
    private void HideNoUnitHasActionPointsUI()
    {
        noUnitHasActionPointsUI.SetActive(false);
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateButtonsSelectedVisuals();
    }

    private void UnitActionSystem_OnActionStarted(object sender, EventArgs e)
    {
        UpdateActionPoints();
        UpdateActionsUsagesLeft();
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }
    
    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        UpdateActionPoints();
        UpdateNoActionPointsUIsVisibility();
    }

    private void UpdateButtonsSelectedVisuals()
    {
        foreach (ActionButtonUI button in actionButtonsUi)
        {
            button.UpdateSelectedVisual();
        }
    }

    private void UpdateActionsUsagesLeft()
    {
        foreach (ActionButtonUI button in actionButtonsUi)
        {
            button.UpdateUsagesLeft();
        }
    }
    
    private void UpdateActionPoints()
    {
        if (UnitActionSystem.Instance.GetSelectedUnit() != null)
        {
            Unit unit = UnitActionSystem.Instance.GetSelectedUnit();
            int points = unit.GetActionPoints();
            actionPointsText.text = $"Action Points: {points}";
        }
    }

    private void OnDestroy()
    {
        Unit.OnAnyActionPointsChanged -= Unit_OnAnyActionPointsChanged;
    }
}
