using System;
using UnityEngine;
using UnityEngine.UI;

public class SelectedUnitUI : MonoBehaviour
{

    [SerializeField] private Image healthBar;
    
    void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        Unit.OnAnyUnitDamaged += Unit_OnAnyUnitDamaged;
        
        UpdateVisibility();
        UpdateHealthBar();
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        UpdateVisibility();
        UpdateHealthBar();
    }

    private void UpdateVisibility()
    {
        Unit unit = UnitActionSystem.Instance.GetSelectedUnit();

        bool shouldBeVisible = unit != null;
        gameObject.SetActive(shouldBeVisible);
    }

    private void Unit_OnAnyUnitDamaged(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }
    
    private void UpdateHealthBar()
    {
        Unit unit = UnitActionSystem.Instance.GetSelectedUnit();

        if (unit == null)
        {
            return;
        }

        healthBar.fillAmount = unit.GetHealthNormalized();
    }
    
    public void OnFocusBtnClicked()
    {
        UnitActionSystem.Instance.FocusOnUnit();
    }

    public void OnPreviousUnitBtnClicked()
    {
        UnitActionSystem.Instance.ChangeSelectedUnit(-1);
    }

    public void OnNextUnitBtnClicked()
    {
        UnitActionSystem.Instance.ChangeSelectedUnit(+1);
    }

    private void OnDestroy()
    {
        Unit.OnAnyUnitDamaged -= Unit_OnAnyUnitDamaged;
    }
}
