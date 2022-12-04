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
    }

    private void CreateUnitActionButtons()
    {
        foreach (Transform buttonTransform in actionButtonContainerTransform)
        {
            Destroy(buttonTransform.gameObject);
        }

        actionButtonsUi = new List<ActionButtonUI>();
        
        Unit unit = UnitActionSystem.Instance.GetSelectedUnit();

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
    }

    private void ShowNoUnitSelectedUI()
    {
        noUnitSelectedGameObject.SetActive(true);
    }

    private void HideNoUnitSelectedUI()
    {
        noUnitSelectedGameObject.SetActive(false);
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
